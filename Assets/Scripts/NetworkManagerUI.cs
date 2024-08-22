using Unity.Netcode;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] EnemySpawner spawner;

    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            networkManager.StartHost();
            spawner.Initialize();
        }

        if (GUILayout.Button("Join"))
        {
            networkManager.StartClient();
        }
        if (GUILayout.Button("Quit"))
        {
            Application.Quit(); 
        }
    }
}
