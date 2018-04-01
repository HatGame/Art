using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {

    public Sprite victoryFlag;
    AudioSource au;
    float f;
    float waitForSound;

    private void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
        f = -1;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = victoryFlag;
            if(!au.isPlaying)
            {
                au.Play(0);
                f = Time.time;
                waitForSound = au.clip.length + Time.time;
                Camera.main.gameObject.GetComponent<AudioSource>().Stop();
            } 
        }

    }

    private void Update()
    {
        if (f > 0)
        {
            f = Time.time;

            if(f > waitForSound)
            {
                Application.Quit();
            }
        }
    }
}
