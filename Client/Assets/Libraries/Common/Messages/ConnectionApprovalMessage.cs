using CommonLib;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Messages
{
    public class ConnectionApprovalMessage : GameMessage
    {
        public string Name { get; set; }

        public override GameMessage Decode(NetIncomingMessage msg)
        {
            Name = msg.ReadString();
            return this;
        }

        public override NetOutgoingMessage Encode(NetOutgoingMessage msg)
        {
            msg.Write(Name);
            return msg;
        }

        public override GameMessageType GetMessageType()
        {
            return GameMessageType.Approval;
        }
    }
}
