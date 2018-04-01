using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stationaryHatScript : MonoBehaviour {

    public int layer;

	// Update is called once per frame
    
	void Update () {
        Vector3 bodyPos = gameObject.GetComponent<Transform>().position;
        gameObject.GetComponent<Transform>().position = new Vector3(bodyPos.x, bodyPos.y, layer);
	}
}
