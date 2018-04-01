using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxScript : MonoBehaviour {

    public GameObject player;
    public float speed;
    public float verticalMultiplier;
    public int layer;

    public void movePos()
    {
        Vector2 playerPos = player.GetComponent<Transform>().position;
        gameObject.GetComponent<Transform>().localPosition = new Vector3(-playerPos.x * speed / 10, -playerPos.y * speed / 10 * verticalMultiplier, layer);
    }


	// Use this for initialization
	void Start () {
        player = GameObject.Find("MANDFRED");
	}
	
	// Update is called once per frame
	void Update () {
        movePos();
	}
}
