using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer r;
    
    private void Start() {
        r = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        if (Time.frameCount % 50 == 0) {
            r.flipX = !r.flipX;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0,-0.005f,0);

        if (transform.position.y < -20F) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("ghost count: " + PlayerScript.ghostCount);
        GameObject g = GameObject.Find("Player");
        g.GetComponent<PlayerScript>().nuke();
        Destroy(g); 
        //transform.position += new Vector3(transform.position.x,transform.position.y,transform.position.z);
        Time.timeScale = 0;
    }
}
