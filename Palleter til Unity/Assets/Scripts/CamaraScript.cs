using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraScript : MonoBehaviour {

    public GameObject Landskab;
    public GameObject BaggrundFar;
    public GameObject BaggrundClose;


    public void ResizeSpriteToScreen(GameObject image)
    {
        var sr = image.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        image.GetComponent<Transform>().localScale = new Vector3((float)worldScreenWidth / height, (float)worldScreenHeight / height, 1);
    }


    public void followPlayer()
    {
        //if(GameObject.Find(""))
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        gameObject.GetComponent<Transform>().position = new Vector3(playerPos.x, playerPos.y, -10);
    }

    // Use this for initialization
    void Start () {
        Landskab = GameObject.Find("landskab");
        BaggrundFar = GameObject.Find("baggrundFar");
        BaggrundClose = GameObject.Find("baggrundClose");

    }

    // Update is called once per frame
    void Update () {
        ResizeSpriteToScreen(Landskab);
        ResizeSpriteToScreen(BaggrundFar);
        ResizeSpriteToScreen(BaggrundClose);
        followPlayer();

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
