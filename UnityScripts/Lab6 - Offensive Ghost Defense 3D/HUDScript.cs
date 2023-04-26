using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
        ammoText.text = "Ammo: " + PlayerScript.ammo.ToString();
        scoreText.text = "Score: " + LaserScript.points.ToString();
    }

    public void setScore() {
        scoreText.text = "Score: " + LaserScript.points*100;
        // Debug.Log("Score: " + LaserScript.points.ToString());
    }

    public void setAmmo() {
        ammoText.text = "Ammo: " + PlayerScript.ammo;
        // Debug.Log("Ammo: " + PlayerScript.ammo.ToString());
    }
}
