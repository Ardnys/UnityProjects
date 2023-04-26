using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    Rigidbody rb;
    float speed = 10F;
    public static int points = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ghost")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
            PlayerScript.ammo += 2;
            // Debug.Log("points " + points);
            points++;
            GameObject.Find("Player").GetComponent<HUDScript>().setAmmo();
            GameObject.Find("Player").GetComponent<HUDScript>().setScore();
        }
        // Debug.Log("not ded");
    }

    
}
