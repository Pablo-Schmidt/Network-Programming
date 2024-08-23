using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField chatInputField;
    [SerializeField] private TextMeshProUGUI chatText;
    [SerializeField] private Button sendButton;

    private NetworkVariable<FixedString128Bytes> latestMessage = new NetworkVariable<FixedString128Bytes>();

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonClicked);
        Debug.Log("Clicked OnSendButton");
        // Display the latest message when it changes
        // latestMessage.OnValueChanged += OnMessageReceivedRpc;
    }

    private void OnSendButtonClicked()
    {
        if (string.IsNullOrEmpty(chatInputField.text)) return;


        // Send the message to the server
        SendMessageServerRpc(chatInputField.text);
        chatInputField.text = string.Empty;
    }

    [Rpc(SendTo.Server)]
    private void SendMessageServerRpc(string message)
    {
        // Update the latest message, which will sync across all clients!
        latestMessage.Value = message;
        Debug.Log("New Message Sent");
        OnMessageReceivedRpc(message);
    }

    [Rpc(SendTo.Everyone)]
    private void OnMessageReceivedRpc(FixedString128Bytes newMessage)
    {
        // Update the chat text with the new message :)
        chatText.text += "\n" + newMessage;
        Debug.Log("New Message Received");
    }
}
