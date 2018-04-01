using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicChanger : MonoBehaviour {

    public AudioClip newBackgroundMusic;
    public bool initiated;

    private void Start()
    {
        initiated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        initiated = true;
    }



    // Update is called once per frame
    void Update () {
		if (initiated)
        {
            
        Camera.main.gameObject.GetComponent<AudioSource>().clip = newBackgroundMusic;
        Camera.main.gameObject.GetComponent<AudioSource>().Play(0);
        Destroy(gameObject);
            
        }
	}
}
