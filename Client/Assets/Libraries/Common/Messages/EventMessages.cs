using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace CommonLib.Messages
{
    public class PlayerConnectedMessage : PlayerSpecificMessage
    {
        public override GameMessageType GetMessageType()
        {
            return GameMessageType.PlayerConnected;
        }
    }

    public class PlayerDisconnectedMessage : PlayerSpecificMessage
    {
        public override GameMessageType GetMessageType()
        {
            return GameMessageType.PlayerDisconnected;
        }
    }
}
