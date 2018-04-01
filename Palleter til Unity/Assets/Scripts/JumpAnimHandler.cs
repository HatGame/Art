using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimHandler : MonoBehaviour {

    //sikrer at hoppeanimationen ikke spiller to gange
    public void jumpHasHappened()
    {
        Debug.Log("asdasd");
        GameObject.Find("Body").GetComponent<Animator>().ResetTrigger("Jump");
    }

    public void JumpIsBegining()
    {
        gameObject.GetComponent<AudioSource>().Play(0);
    }
}
