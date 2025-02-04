#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;
using NeuroSdk.Il2Cpp;
using NeuroSdk.Internal;
using NeuroSdk.Messages.API;
using UnityEngine;

namespace NeuroSdk.Websocket
{
#pragma warning disable CS0618 // Type or member is obsolete
    [RegisterInIl2Cpp]
#pragma warning restore CS0618 // Type or member is obsolete
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class CommandHandler : MonoBehaviour
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected readonly List<IIncomingMessageHandler> Handlers = new();

        public virtual void Start()
        {
            AddHandlersFromAssembly(Assembly.GetExecutingAssembly());
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public virtual void AddHandlersFromAssembly(Assembly assembly)
        {
            Handlers.AddRange(ReflectionHelpers.GetAllInAssembly<IIncomingMessageHandler>(assembly, transform));
        }

        public virtual void Handle(string command, MessageJData data)
        {
            foreach (IIncomingMessageHandler handler in Handlers)
            {
                if (!handler.CanHandle(command)) continue;

                ExecutionResult validationResult;
                object? parsedData;
                try
                {
                    validationResult = handler.Validate(command, data, out parsedData);
                }
                catch (Exception e)
                {
                    Debug.LogError("Caught exception during validation at WebsocketConnection level - this is bad.");
                    Debug.LogError(e.ToString());

                    validationResult = ExecutionResult.Failure(Strings.MessageHandlerFailedCaughtException.Format(e.Message));
                    parsedData = null;
                }

                if (!validationResult.Successful)
                {
                    Debug.LogWarning("Received unsuccessful execution result when handling a message");
                    Debug.LogWarning(validationResult.Message);
                    Debug.LogWarning(StackTraceUtility.ExtractStackTrace());
                }

                handler.ReportResult(parsedData, validationResult);

                if (validationResult.Successful)
                {
                    handler.Execute(parsedData);
                }
            }
        }
    }
}
