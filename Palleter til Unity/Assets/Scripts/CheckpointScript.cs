using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {

    public Vector3 checkpointLocation;
    public Sprite usedCheckpoint;
    AudioSource au;

    private void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
        checkpointLocation = gameObject.GetComponent<Transform>().position;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (GameObject.Find("MANDFRED").GetComponent<MANDFRED_script>().Spawnlocation != checkpointLocation)
            {
                au.Play(0);
                GameObject.Find("MANDFRED").GetComponent<MANDFRED_script>().Spawnlocation = checkpointLocation;
                gameObject.GetComponent<SpriteRenderer>().sprite = usedCheckpoint;
            }
        }
        
    }
}
