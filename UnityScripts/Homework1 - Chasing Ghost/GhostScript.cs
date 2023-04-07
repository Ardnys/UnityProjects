using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    Rigidbody2D rb;
    bool crazy = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(randomVelocity(-5F, 5F), randomVelocity(-4F, 4F));
    }

    // Update is called once per frame
    void Update()
    {
        if (crazy) crazyMove();
    }

    void crazyMove() {
        // directions to follow
        float x = player.transform.position.x - gameObject.transform.position.x;
        float y = player.transform.position.y - gameObject.transform.position.y;
        float magnitude = Mathf.Sqrt(x*x + y*y);
        float xNormal = 3f * x / magnitude;
        float yNormal = 3f * y / magnitude;
        // float xBase = 4f;
        // float yBase = 4f;
        
        rb.velocity = new Vector2(xNormal, yNormal);
    }
    private float randomVelocity(float low, float high) {
        return Random.Range(low, high); // generate a float between low and high
    }

    public void makeItCrazy() {
        crazy = true;
        player = GameObject.Find("Player");
    }

}
