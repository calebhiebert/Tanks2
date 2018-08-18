using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using CommonLib;
using CommonLib.Messages;

public class NetworkManager : MonoBehaviour {

    private NetClient _client;
    private GameMessageHandler _messageHandler;

	// Use this for initialization
	void Start () {
        var approval = new ConnectionApprovalMessage
        {
            Name = "James"
        };

        var config = new NetPeerConfiguration("Tanks2");
        _client = new NetClient(config);
        _messageHandler = new GameMessageHandler(_client);
        _client.Start();
        _client.Connect("localhost", 4200, _messageHandler.Encode(approval));
	}
	
	// Update is called once per frame
	void Update () {
        NetIncomingMessage msg;
        while((msg = _client.ReadMessage()) != null)
        {
            ProcessMessage(msg);
            _client.Recycle(msg);
        }

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var controls = new PlayerInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), mousePos.x, mousePos.y);

        var approval = new ConnectionApprovalMessage
        {
            Name = "Test"
        };

        _client.SendMessage(_messageHandler.Encode(new PlayerControlUpdateMessage { Controls = controls }), NetDeliveryMethod.UnreliableSequenced, (int)GameMessageType.PlayerControls);
    }

    private void ProcessMessage(NetIncomingMessage msg)
    {
        switch (msg.MessageType)
        {
            case NetIncomingMessageType.VerboseDebugMessage:
            case NetIncomingMessageType.DebugMessage:
            case NetIncomingMessageType.WarningMessage:
            case NetIncomingMessageType.ErrorMessage:
                Debug.Log(msg.ReadString());
                break;
            case NetIncomingMessageType.StatusChanged:
                switch ((NetConnectionStatus)msg.ReadByte())
                {
                    case NetConnectionStatus.Connected:
                        OnConnect();
                        break;
                    default:
                        Debug.Log("Status Changed " + ((NetConnectionStatus)msg.ReadByte()).ToString());
                        break;
                }
                break;
            case NetIncomingMessageType.Data:
                HandleIncomingMessage(_messageHandler.Decode(msg));
                break;
            default:
                Debug.Log("Unhandled type: " + msg.MessageType);
                break;
        }
    }

    private void HandleIncomingMessage(GameMessage msg)
    {
        switch(msg.GetMessageType())
        {
            case GameMessageType.PlayerConnected:
                Debug.Log("Player Connected " + (msg as PlayerConnectedMessage).PlayerID);
                break;
            case GameMessageType.PlayerDisconnected:
                Debug.Log("Player Disconnected " + (msg as PlayerDisconnectedMessage).PlayerID);
                break;
        }
    }

    private void OnConnect()
    {
        var msg = _client.CreateMessage();
        msg.Write("Hello There!");
        _client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);

        Debug.Log("Connected to server. My ID Is " + _client.UniqueIdentifier);
    }
}
