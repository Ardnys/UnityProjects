using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    float h;
    float speed = 8F;

    Rigidbody2D rb;

    public GameObject ghost;
    private int interval = 150;
    public static int ghostCount = 0;
    private List<GameObject> ghostList = new List<GameObject>();
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
        
        if(transform.position.x > 10F)
            transform.position = new Vector2(10F, transform.position.y);
        if (transform.position.x <-8.67F)
            transform.position = new Vector2(-8.67F, transform.position.y);
    }

    void FixedUpdate() {
        if (Time.frameCount % interval == 0) {
            // create a ghost
            createGhost();
            interval--;
            ghostCount++;
        }
    }

    public void nuke() {
        for (int i = 0; i < ghostList.Count; i++) {
            Destroy(ghostList[i]);
        }
    }

    private void createGhost() {
        // x = -10, 1
        int colorId = randomColor();
        GameObject g = Instantiate(
            ghost, // should be set as an public object field
            new Vector2(randomPos(-7,9),randomPos(-1, 12)), // set the position of the ghost
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
        ghostList.Add(g);
    }

    private float randomPos(float low, float high) {
        return Random.Range(low, high); // generate a float between -4.0 and 4.0
    }

    private int randomColor() {
        return Random.Range(0, 4);
    }


}
