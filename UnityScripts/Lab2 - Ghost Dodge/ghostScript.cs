using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostScript : MonoBehaviour
{
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
        transform.position += new Vector3(0,0,-0.005f);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("ghost count: " + playerScript.ghostCount);
        Destroy(GameObject.Find("Player")); 
        Time.timeScale = 0;
    }
}