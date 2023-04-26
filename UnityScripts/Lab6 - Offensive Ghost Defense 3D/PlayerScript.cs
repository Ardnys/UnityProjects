using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    float h;
    float speed = 8F;
    private int interval = 800;
    Rigidbody rb;
    public GameObject Ghost;
    public GameObject Laser;
    public static int ammo = 2;

    public static bool stopFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(speed * h, 0, 0);
        if (Input.GetKeyDown("space") && stopFlag && ammo > 0) {
            fireLaser();
            ammo--;
            GetComponent<HUDScript>().setAmmo();
        }
        if (Time.frameCount % interval == 0 && stopFlag) {
            createGhost();
        }
    }
    private void fireLaser() {
        GameObject laser = Instantiate(
            Laser,
            new Vector3(transform.position.x, transform.position.y+1.5F, transform.position.z+1.5F),
            Quaternion.Euler(90,0,0)
        );
                // Debug.Log("ammo is  " + ammo.ToString());
    }
    private void createGhost() {       
        GameObject g = Instantiate(
            Ghost, // should be set as an public object field
            new Vector3(randomPos(-7F, 7F), 2.8F, randomPos(18F, 20F)), // set the position of the ghost
            Quaternion.Euler(0,180,0) // identity rotation (no rotation)
        );
        // ghostList.Add(g);
    }
    private float randomPos(float low, float high) {
        return Random.Range(low, high); // generate a float
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ghost")) {
            // game ends
            // Destroy(gameObject);
            Time.timeScale = 0;
            stopFlag = false;

        }
    }
}
