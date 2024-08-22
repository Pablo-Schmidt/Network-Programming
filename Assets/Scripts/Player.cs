using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Bullet bullet;
    [SerializeField] InputReader inputReader;
    [SerializeField] int speed;
    [SerializeField] Vector3 mousepos = Vector3.zero;

    NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

    [SerializeField] Transform aFirePoint;
    [SerializeField] GameObject objectToSpawn;

    private bool isAlive = true;

    void Start()
    {
        if (inputReader != null && IsLocalPlayer)
        {
            inputReader.MoveEvent += OnMove;
            inputReader.ShootEvent += ShootRPC;
        }

        GameManager.Instance.RegisterPlayer(this); // Register the player with the GameManager
    }

    private void OnMove(Vector2 input)
    {
        MoveRPC(input);
    }

    private void Update()
    {
        if (!isAlive) return; // If the player is dead, do nothing

        if (IsServer)
        {
            transform.position += (Vector3)moveInput.Value * speed * Time.deltaTime;
        }

        if (!IsOwner || !Application.isFocused) return;

        if (Input.GetMouseButtonDown(0))
        {
            ShootRPC();
        }
    }

    [Rpc(SendTo.Server)]
    private void ShootRPC()
    {
        NetworkObject bullet = Instantiate(objectToSpawn, aFirePoint.position, aFirePoint.rotation).GetComponent<NetworkObject>();
        bullet.GetComponent<NetworkObject>().Spawn();

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(aFirePoint.up * speed, ForceMode2D.Impulse);
    }

    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 data)
    {
        moveInput.Value = data;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isAlive) return; // If the player is already dead, do nothing

        isAlive = false; // Mark the player as dead
        gameObject.SetActive(false); // Disable the player object

        GameManager.Instance.PlayerDied(this); // Notify the GameManager that this player died
    }
}
