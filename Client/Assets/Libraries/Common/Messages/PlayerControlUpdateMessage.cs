using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace CommonLib.Messages
{
    public class PlayerControlUpdateMessage : GameMessage
    {
        public PlayerInput Controls { get; set; }

        public override GameMessage Decode(NetIncomingMessage msg)
        {
            Controls = new PlayerInput
            {
                X = msg.ReadFloat(),
                Y = msg.ReadFloat(),
                MouseX = msg.ReadFloat(),
                MouseY = msg.ReadFloat()
            };
            return this;
        }

        public override NetOutgoingMessage Encode(NetOutgoingMessage msg)
        {
            msg.Write(Controls.X);
            msg.Write(Controls.Y);
            msg.Write(Controls.MouseX);
            msg.Write(Controls.MouseY);
            return msg;
        }

        public override GameMessageType GetMessageType()
        {
            return GameMessageType.PlayerControls;
        }
    }
}
