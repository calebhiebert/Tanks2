using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace CommonLib.Messages
{
    public abstract class PlayerSpecificMessage : GameMessage
    {
        public long PlayerID { get; set; }

        public PlayerSpecificMessage() { }

        public PlayerSpecificMessage(long id)
        {
            this.PlayerID = id;
        }

        public override GameMessage Decode(NetIncomingMessage msg)
        {
            PlayerID = msg.ReadInt64();
            return this;
        }

        public override NetOutgoingMessage Encode(NetOutgoingMessage msg)
        {
            msg.Write(PlayerID);
            return msg;
        }
    }
}
