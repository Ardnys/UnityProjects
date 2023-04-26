using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -6F);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -7) {
            Destroy(gameObject);
        }
    }
}
