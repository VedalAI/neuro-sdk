#nullable enable

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NeuroSdk.Messages.API;
using NeuroSdk.Utilities;
using UnityEngine;

namespace NeuroSdk.Websocket
{
    [PublicAPI]
    public class CommandHandler : MonoBehaviour
    {
        protected readonly List<IIncomingMessageHandler> Handlers = new();

        public virtual void Start()
        {
            Handlers.AddRange(ReflectionHelpers.GetAllInDomain<IIncomingMessageHandler>(transform));
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
                    Debug.LogError(e);

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
