#nullable enable

using System.Collections.Generic;
using NeuroSdk.Messages.API;
using NeuroSdk.Messages.Outgoing;
using NeuroSdk.Utilities.Il2Cpp;
using UnityEngine;

namespace NeuroSdk.Websocket
{
    [RegisterInIl2Cpp]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class MessageQueue : MonoBehaviour
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected readonly List<OutgoingMessageBuilder> Messages = new() { new Startup() };

        public virtual int Count
        {
            get
            {
                lock (Messages)
                {
                    return Messages.Count;
                }
            }
        }

        public virtual void Enqueue(OutgoingMessageBuilder message)
        {
            lock (Messages)
            {
                foreach (OutgoingMessageBuilder existingMessage in Messages)
                {
                    if (existingMessage.Merge(message)) return;
                }

                Messages.Add(message);
            }
        }

        public virtual OutgoingMessageBuilder? Dequeue()
        {
            lock (Messages)
            {
                if (Messages.Count == 0) return null;

                OutgoingMessageBuilder message = Messages[0];
                Messages.RemoveAt(0);

                return message;
            }
        }
    }
}
