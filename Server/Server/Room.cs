using System;
using System.Collections.Generic;
using Lidgren.Network;
using CommonLib;
using CommonLib.Messages;

namespace Server
{
    class Room
    {
        private Dictionary<long, Player> _players;
        private Server _server;

        public Room(Server server)
        {
            _server = server;
            _players = new Dictionary<long, Player>();
        }

        public void Tick(float delta)
        {
            foreach (KeyValuePair<long, Player> entry in _players)
            {
                if (entry.Value.Tank != null)
                {
                    var tank = entry.Value.Tank;
                    
                    if (entry.Value.Input != null)
                    {
                        tank.ApplyControls(entry.Value.Input, delta);
                        tank.Transform.Translate(tank.CalculatePositionStep(delta));
                        tank.Transform.Rotate(0, 0, tank.CalculateRotationStep(delta));

                        if (entry.Value.Input.Y == 0)
                        {
                            tank.ApplyLinearDrag(delta);
                        }

                        if (entry.Value.Input.X == 0)
                        {
                            tank.ApplyAngularDrag(delta);
                        }
                    }
                }
            }
        }

        public void AddPlayer(NetConnection connection, ConnectionApprovalMessage approval)
        {
            var player = new Player { Connection = connection, Name = approval.Name };
            player.Tank = new Tank();
            _players[connection.RemoteUniqueIdentifier] = player;
            Broadcast(new PlayerConnectedMessage { PlayerID = connection.RemoteUniqueIdentifier }, NetDeliveryMethod.ReliableOrdered);
        }

        public void RemovePlayer(NetConnection connection)
        {
            _players.Remove(connection.RemoteUniqueIdentifier);
            Broadcast(new PlayerDisconnectedMessage { PlayerID = connection.RemoteUniqueIdentifier }, NetDeliveryMethod.ReliableOrdered);
        }

        public void UpdatePlayerInput(long id, PlayerInput input)
        {
            try
            {
                _players[id].Input = input;
                Console.WriteLine("Player: {2} MX: {0} MY: {1}", input.MouseX, input.MouseY, _players[id].Name);
            } catch (KeyNotFoundException)
            {

            }
        }

        private void Broadcast(GameMessage message, NetDeliveryMethod method)
        {
            foreach (KeyValuePair<long, Player> entry in _players)
            {
                entry.Value.Connection.SendMessage(Server.MessageHandler.Encode(message), method, (int)message.GetMessageType());
            }
        }
    }
}
