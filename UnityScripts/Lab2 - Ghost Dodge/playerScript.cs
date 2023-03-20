using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    // public float moveSpeed;

    float horizontalInput;
    public GameObject ghost;
    private int interval = 100;
    public static int ghostCount = 0;


    // Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {    
        horizontalInput = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector3(transform.position.x, transform.position.y, -50f);
        
        if(transform.position.x > 1.5)
            transform.position = new Vector3(1.5F, transform.position.y, transform.position.z);
        if (transform.position.x <-12)
            transform.position = new Vector3(-12, transform.position.y, transform.position.z);
    }
    void FixedUpdate() {
        if (Time.frameCount % interval == 0) {
            // create a ghost
            createGhost();
            interval--;
            ghostCount++;
        }
    }

    private void createGhost() {
        // x = -10, 1
        int colorId = randomColor();
        GameObject g = Instantiate(
            ghost, // should be set as an public object field
            new Vector3(randomPos(-10,1),transform.position.y,transform.position.z+randomPos(15,25)), // set the position of the ghost
            Quaternion.identity // identity rotation (no rotation)
        );
        if (colorId == 0) {
            // red
            g.GetComponent<Renderer>().material.color = Color.red;
        } else if (colorId == 1) {
            // blue
            g.GetComponent<Renderer>().material.color = Color.blue;
        } else if (colorId == 2){
            // yellow
            g.GetComponent<Renderer>().material.color = Color.yellow;
        } else {
            ;
        }
        

    }

    private float randomPos(float low, float high) {
        return Random.Range(low, high); // generate a float between -4.0 and 4.0
    }

    private int randomColor() {
        return Random.Range(0, 4);
    }
}
