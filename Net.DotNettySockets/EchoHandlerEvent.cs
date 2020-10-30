using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.DotNettySockets
{
    public class EchoHandlerEvent
    {
        public event Action<string, string> MessageReceived;
        public event Func<string, bool> IsAllowHandleAdd;
        public event Action<string, Exception> EchoExceptionCaughtEvent;
        public event Action<string> EchoClientRemovedEvent;
        public void OnMessageReceive(string adressPort,string msg) 
        { 
            MessageReceived?.Invoke(adressPort, msg); 
        }

        public void OnEchoExceptionCaught(string adressPort, Exception ex)
        {
            EchoExceptionCaughtEvent?.Invoke(adressPort,ex);
        }

        public bool OnAllowHandleAddClient(string adressPort)
        {
            if (IsAllowHandleAdd == null)
                return false;
            return IsAllowHandleAdd.Invoke(adressPort);
        }

        public void OnEchoClientRemoved(string adressPort)
        {
            EchoClientRemovedEvent?.Invoke(adressPort);
        }
    }
}
