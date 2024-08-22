using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : NetworkBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    //public Transform bulletSpawnPoint; // The point from where the bullet will be fired
    public float bulletSpeed = 1f; // Speed of the bullet
    public float speed = 1f; // Speed of the bullet
    public Transform target; // Target to move towards
    //public Vector3 aDirection;
    Vector3 mousepos = Vector3.zero;
    //public int maxShots = 10;
    //private int currentShots;
    //private bool isReloading;
    //public float reloadTime = 2f;

    //void Start()
    //{
    //    currentShots = maxShots;
    //    isReloading = false;
    //}




    void Update()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = 0;

        mousepos = (mousepos - transform.position).normalized;

        /*if (mousepos != null)
        {
            // Move the bullet towards the target
            Vector3 direction = mousepos - transform.position;
            direction.z = 0;
            transform.up = direction;

            transform.position += direction * speed * Time.deltaTime;
        }*/
        if (Input.GetMouseButtonDown(0)) // very bad
        {

            //Shoot(); // SHOULD BE REMOVED
        }

    }

    /*public void Shoot() // this removed
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            GameObject Triangle = GameObject.FindGameObjectWithTag("Triangle");
            if (Triangle != null)
            {
                target = Triangle.transform;
            }
            rb.velocity = aDirection * bulletSpeed;
        }
    }*/
    }


