using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField chatInputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private TextMeshProUGUI messagesText;

    private void Start()
    {
        // Only allow sending messages if the player is the owner
        if (IsOwner)
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }
        else
        {
            chatInputField.interactable = false;
            sendButton.interactable = false;
        }
    }

    private void OnSendButtonClicked()
    {
        string message = chatInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            SendChatMessageServerRpc(message);
            chatInputField.text = string.Empty;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        // Broadcast the message to all clients
        ReceiveChatMessageClientRpc($"{OwnerClientId}: {message}");
    }

    [ClientRpc]
    private void ReceiveChatMessageClientRpc(string message)
    {
        // Display the message in the chat window
        messagesText.text += message + "\n";
    }
}
