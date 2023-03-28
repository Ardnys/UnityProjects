using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    float verticalSpeed;
    float horizontalSpeed;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(randomVelocity(-3F,3F), randomVelocity(-2F, -4F));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y <= -4.5F) {
            Time.timeScale = 0;
            PlayerScript.stopFlag = false;
            Debug.Log("your score: "+LaserScript.points);
            Destroy(gameObject);
        }
    }

    private float randomVelocity(float low, float high) {
        return Random.Range(low, high); // generate a float between low and high
    }



}
