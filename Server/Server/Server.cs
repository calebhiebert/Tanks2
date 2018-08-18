using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;
using CommonLib;
using CommonLib.Messages;


namespace Server
{
    class Server
    {
        public static GameMessageHandler MessageHandler { get; private set; }

        private NetServer _server;
        private bool _stopped = false;
        private float _tps;
        private Room _room;

        public Server(float tps)
        {
            _tps = tps;
            var config = new NetPeerConfiguration("Tanks2");
            config.Port = 4200;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(config);
            MessageHandler = new GameMessageHandler(_server);
            _room = new Room(this);
        }

        public void Start()
        {
            _server.Start();
            StartGameLoop();
        }

        private void StartGameLoop()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var lastTick = stopWatch.ElapsedMilliseconds;

            while(!_stopped)
            {
                Thread.Sleep(1);
                var elapsed = (float)(stopWatch.ElapsedMilliseconds - lastTick);

                if (elapsed >= 1000f / _tps)
                {
                    Tick(elapsed / 1000);
                    lastTick = stopWatch.ElapsedMilliseconds;
                }

            }
        }

        private void Tick(float delta)
        {
            ReadMessages(_server);
            _room.Tick(delta);
        }

        private bool IsApproved(ConnectionApprovalMessage approval)
        {
            return approval.Name.Trim() != "";
        }

        private void HandleIncomingMessage(GameMessage msg, NetIncomingMessage original)
        {
            if (msg == null)
            {
                return;
            }

            switch(msg.GetMessageType())
            {
                case GameMessageType.PlayerControls:
                    PlayerControlUpdateMessage controls = msg as PlayerControlUpdateMessage;
                    _room.UpdatePlayerInput(original.SenderConnection.RemoteUniqueIdentifier, controls.Controls);
                    break;
                default:
                    Console.WriteLine("Unhandled Data Type " + msg.GetMessageType().ToString());
                    break;
            }
        }

        private void ReadMessages(NetServer server)
        {
            NetIncomingMessage msg;

            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        HandleIncomingMessage(MessageHandler.Decode(msg), msg);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)msg.ReadByte();
                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                Console.WriteLine("Connection Established " + msg.SenderConnection.RemoteUniqueIdentifier);
                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine("User Disconnected" + msg.SenderConnection.RemoteUniqueIdentifier);
                                _room.RemovePlayer(msg.SenderConnection);
                                break;
                            default:
                                Console.WriteLine("Connection Status: " + status.ToString());
                                break;
                        }
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        ConnectionApprovalMessage approval = MessageHandler.Decode(msg) as ConnectionApprovalMessage;
                        Console.WriteLine(approval.Name);
                        if (IsApproved(approval))
                        {
                            msg.SenderConnection.Approve();
                            _room.AddPlayer(msg.SenderConnection, approval);
                        }
                        else
                        {
                            msg.SenderConnection.Deny();
                        }
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }

                server.Recycle(msg);
            }

        }
    }
}
