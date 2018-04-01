using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimHandler : MonoBehaviour {

    public AudioClip walkSound1;
    public AudioClip walkSound2;
    public AudioClip jumpSound;
    AudioSource au;
    private void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
    }


    //sikrer at hoppeanimationen ikke spiller to gange
    public void jumpHasHappened()
    {
        gameObject.GetComponent<Animator>().ResetTrigger("Jump");
        GameObject.Find("MANDFRED").GetComponent<MANDFRED_script>().isJumping = false;
    }

    public void JumpIsBegining()
    {
        au.clip = jumpSound;
        au.volume = 1;
        au.Play(0);
    }

    public void playWalkSound1()
    {
        au.clip = walkSound1;
        au.volume = 0.15f;
        au.Play(0);
    }

    public void playWalkSound2()
    {
        au.clip = walkSound2;
        au.volume = 0.15f;
        au.Play(0);
    }
}
