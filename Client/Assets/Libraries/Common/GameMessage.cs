using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace CommonLib
{
    public enum GameMessageType
    {
        PlayerConnected, PlayerDisconnected, Approval, PlayerControls
    };

    public abstract class GameMessage
    {
        public abstract NetOutgoingMessage Encode(NetOutgoingMessage msg);
        public abstract GameMessage Decode(NetIncomingMessage msg);
        public abstract GameMessageType GetMessageType();
    }
}
