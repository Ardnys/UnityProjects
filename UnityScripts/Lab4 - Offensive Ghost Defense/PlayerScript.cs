using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float h;
    float speed = 7F;
    private int interval = 250;
    Rigidbody2D rb;
    public GameObject Ghost;
    public GameObject Laser;

    public static bool stopFlag = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * speed, 0);

        if (Input.GetKeyDown("space") && stopFlag) {
            fireLaser();
        }
        if (Time.frameCount % interval == 0 && stopFlag) {
            createGhost();
        }
    }

    private void fireLaser() {
        GameObject laser = Instantiate(
            Laser,
            new Vector2(transform.position.x, transform.position.y+0.9F),
            Quaternion.identity
        );
    }
    private void createGhost() {
        // -3.4f,4.2f - 3.6, 3.1
        int colorId = randomColor();
        GameObject g = Instantiate(
            Ghost, // should be set as an public object field
            new Vector2(randomPos(-3.4F, 3.6F), randomPos(3.1F, 4.2F)), // set the position of the ghost
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
        // ghostList.Add(g);
    }

    private float randomPos(float low, float high) {
        return Random.Range(low, high); // generate a float between -4.0 and 4.0
    }

    private int randomColor() {
        return Random.Range(0, 4);
    }
}
