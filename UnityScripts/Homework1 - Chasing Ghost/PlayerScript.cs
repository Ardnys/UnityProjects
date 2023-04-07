using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float horizontal;
    float vertical;
    float speed = 6F;
    Rigidbody2D rb;
    public GameObject ghost;
    public GameObject endScreen;
    public int level = 1;
    public List<GameObject> points = new List<GameObject>();
    private List<GameObject> pointsInLevel = new List<GameObject>();
    public List<GameObject> ghosts = new List<GameObject>();
    public GameObject pointArray;
    private int pointIdx = 0;
    private float XMIN = -6.60F;
    private float XMAX = 6.60F;
    private float YMIN = -3.3F;
    private float YMAX = 3.3F;  
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        generatePoints();
        generateGhosts();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        // if (pointIdx == level-1) {
        //     // next level
            
        // }
    }

    void generatePoints() {
        for (int i = 0; i < level; i++) {
            GameObject point = Instantiate(
                points[i],
                new Vector2(randomPos(XMIN, XMAX), randomPos(YMIN, YMAX)),
                Quaternion.identity
            );
            pointsInLevel.Add(point);
        }
    }

    void generateGhosts() {
        for (int i = 0; i < level; i++) {
            GameObject g = Instantiate(
                ghost,
                overlyComplicatedPos(),
                Quaternion.identity
            );
            if (i == 0) {
                // initialize the crazy ghost
                g.GetComponent<GhostScript>().makeItCrazy();
            }
            int color = randomColor();
            if (color == 0) {
                g.GetComponent<Renderer>().material.color = Color.magenta;
            } else if (color == 1) {
                g.GetComponent<Renderer>().material.color = Color.green;
            } else {
                g.GetComponent<Renderer>().material.color = Color.cyan;
            }
            ghosts.Add(g);
        }
    }
    private void generateEndScreen() {
        GameObject point = Instantiate(
                points[level],
                new Vector2(0, -3F), // TODO points appear on top of one another
                Quaternion.identity
        );
        endScreen = Instantiate(
                    endScreen,
                    new Vector3(0,0,0),
                    Quaternion.identity
        );
        Time.timeScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ghost")){
            Destroy(gameObject);
            ghosts.ForEach(ghost => Destroy(ghost));
            ghosts.Clear();
            pointsInLevel.ForEach(p => Destroy(p));
            pointsInLevel.Clear();
            generateEndScreen();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Point")) {
            int charVal = pointIdx + '0';
            if (other.name.Contains((char) charVal)) {
                pointIdx++;
                Destroy(other.gameObject);
            }
            if (pointIdx == level && level <= 10) {
                // Debug.Log("success");
                ghosts.ForEach(ghost => Destroy(ghost));
                ghosts.Clear();
                level++;
                pointIdx = 0;
                if (level < 10) {
                    // continue
                    generateGhosts();
                    generatePoints();
                    return;
                }
                generateEndScreen();
            }
        }
    }

    private Vector2 overlyComplicatedPos() {
        float x = randomPos(XMIN, XMAX);
        float y = randomPos(YMIN, YMAX);
        float playerX = gameObject.transform.position.x;
        float playerY = gameObject.transform.position.y;
        // make sure it's not too close
        while (Abs(playerX - x) < 3f) {
            x = randomPos(XMIN, XMAX);
        }
        while (Abs(playerY - y) < 3f) {
            y = randomPos(YMIN, YMAX);
        }
        return new Vector2(x,y);
    }

    private float randomPos(float low, float high) {
        return Random.Range(low, high); // generate a float 
    }

    private int randomColor() {
        return Random.Range(0, 3);
    }
}
