using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI gameOverText;
    private List<Player> players = new List<Player>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPlayer(Player player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void PlayerDied(Player player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

        if (players.Count == 0)
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
