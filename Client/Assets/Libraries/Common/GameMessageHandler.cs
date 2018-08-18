using CommonLib.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonLib
{
    public class GameMessageHandler
    {
        private static Dictionary<GameMessageType, Func<GameMessage>> _messageTypes = new Dictionary<GameMessageType, Func<GameMessage>>();
        private NetPeer _peer;

        public GameMessageHandler(NetPeer peer)
        {
            this._peer = peer;
            _messageTypes[GameMessageType.PlayerConnected] = () => new PlayerConnectedMessage();
            _messageTypes[GameMessageType.PlayerDisconnected] = () => new PlayerDisconnectedMessage();
            _messageTypes[GameMessageType.Approval] = () => new ConnectionApprovalMessage();
            _messageTypes[GameMessageType.PlayerControls] = () => new PlayerControlUpdateMessage();
        }

        public NetOutgoingMessage Encode(GameMessage message)
        {
            var msg = _peer.CreateMessage();
            var msgCode = (int)message.GetMessageType();
            msg.Write(msgCode);
            return message.Encode(msg);
        }

        public GameMessage Decode(NetIncomingMessage message)
        {
            var messageType = (GameMessageType)message.ReadInt32();

            try
            {
                var freshMessage = _messageTypes[messageType]();
                return freshMessage.Decode(message);
            } catch (KeyNotFoundException err)
            {
                return null;
            }
        }
    }
}
