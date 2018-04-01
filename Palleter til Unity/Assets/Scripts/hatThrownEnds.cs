using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hatThrownEnds : MonoBehaviour {

    public bool hasCollidedWithPlayer;
    public bool hasCollidedWithOther;
    public bool hasColliedeWithNoHat;
    public GameObject collision;
    HatThrown_script parentScript;


    // Use this for initialization
    void Start()
    {
        hasCollidedWithPlayer = false;
        hasCollidedWithOther = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NoHat")
        {
            collision = other.gameObject;
            hasColliedeWithNoHat = true;
            gameObject.GetComponentInParent<HatThrown_script>().hasCollidedWithObject = true;
            Debug.Log("DenErGodFin");
        }
        else if (other.tag != "Player" && other.tag != "Checkpoint")
        {
            collision = other.gameObject;
            hasCollidedWithOther = true;
            gameObject.GetComponentInParent<HatThrown_script>().hasCollidedWithObject = true;
            Debug.Log("DenErGodFin");
        }
        else if (other.tag != "Checkpoint")
        {
            collision = other.gameObject;
            hasCollidedWithPlayer = true;
        }
        
    }
}
