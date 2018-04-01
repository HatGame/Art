using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatThrown_script : MonoBehaviour {

    public float safeTime;
    public float outSpeed;
    public float backAccel;
    public float knockbackPowerHorizontal;
    public float knockbackPowerVertical;
    public bool isStuck;
    public bool onTheWayBack;
    public float speed;
    public bool hasCollidedWithObject;

    public Vector2 travelDir;
    public float timeCreated;
    public float stuckTime;
    public float stuckDuration;

    Rigidbody2D rBodyMandfred;
    GameObject mandfred;
    MANDFRED_script mandfredScript;
    Rigidbody2D rBody;
    AudioSource au;
    public GameObject sideCollider;
    public GameObject endCollider;

    public AudioClip stuckSound;
    public AudioClip noHatSound;
    public AudioClip thrownSound;
    public AudioClip wearOutSound;
    public AudioClip breakSound;
    public AudioClip catchSound;
    float f;
    float waitForSound;


    public void ThrowTowards(Vector2 s, Vector2 m)
    {
        travelDir = (m - s).normalized;
        gameObject.GetComponent<Rigidbody2D>().velocity = travelDir * outSpeed;
    }


    void Start () {
        // vi finder mandfred og gemmer ham samt diverse variabler
        mandfred = GameObject.Find("MANDFRED");
        mandfredScript = mandfred.GetComponent<MANDFRED_script>();
        rBodyMandfred = mandfred.GetComponent<Rigidbody2D>();
        rBody = gameObject.GetComponent<Rigidbody2D>();
        timeCreated = Time.time; //øjeblikket hatThrown bliver skabt
        onTheWayBack = false;
        au = gameObject.GetComponent<AudioSource>();
    }
	

	void FixedUpdate () {

        //Ødelægger hatten, hvis den rammer en "ingen hat" zone
        if (sideCollider.GetComponent<hatThrownEnds>().hasColliedeWithNoHat || endCollider.GetComponent<hatThrownEnds>().hasColliedeWithNoHat)
        {
            if (au.clip != noHatSound)
            {
                mandfredScript.hatIsOnHead = true;
                au.clip = noHatSound;
                au.Play(0);
                waitForSound = noHatSound.length + Time.time;
                f = 0;
                rBody.bodyType = RigidbodyType2D.Static;
                foreach (BoxCollider2D b in endCollider.GetComponents<BoxCollider2D>())
                {
                    Destroy(b);
                }
                Destroy(sideCollider.GetComponent<BoxCollider2D>());
            }
            if (f < waitForSound)
            {
                f = Time.time;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
            else
            {
                Destroy(gameObject);
            }
        }

            if (!isStuck)
            {
                if (endCollider.GetComponent<hatThrownEnds>().hasCollidedWithOther)
                {
                    if (au.clip != breakSound)
                    {
                        mandfredScript.hatIsOnHead = true;
                        au.clip = breakSound;
                        au.Play(0);
                        waitForSound = breakSound.length + Time.time;
                        f = 0;
                        rBody.bodyType = RigidbodyType2D.Static;
                    foreach (BoxCollider2D b in endCollider.GetComponents<BoxCollider2D>())
                    {
                        Destroy(b);
                    }
                        Destroy(sideCollider.GetComponent<BoxCollider2D>());

                    }
                    if(f < waitForSound)
                    {
                        f = Time.time;
                        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else if (sideCollider.GetComponent<hatThrownEnds>().hasCollidedWithOther)
                {
                    stuckTime = Time.time;
                    isStuck = true;
                    rBody.velocity = new Vector2(0, 0);
                    gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    gameObject.AddComponent<BoxCollider2D>();
                }
            }

        
        if (sideCollider.GetComponent<hatThrownEnds>().hasCollidedWithPlayer || endCollider.GetComponent<hatThrownEnds>().hasCollidedWithPlayer)
        {
            if(safeTime + timeCreated <= Time.time)
            {
                if (!sideCollider.GetComponent<hatThrownEnds>().hasCollidedWithOther)
                {
                    if (au.clip != catchSound)
                    {
                        mandfredScript.hastighed += rBody.velocity.x * knockbackPowerHorizontal;
                        mandfredScript.hatIsOnHead = true;
                        rBodyMandfred.AddForce(new Vector2(0, rBody.velocity.y * knockbackPowerVertical));

                        au.clip = catchSound;
                        au.Play(0);
                        waitForSound = catchSound.length + Time.time;
                        f = 0;

                        rBody.bodyType = RigidbodyType2D.Static;
                        foreach (BoxCollider2D b in endCollider.GetComponents<BoxCollider2D>())
                        {
                            Destroy(b);
                        }
                        Destroy(sideCollider.GetComponent<BoxCollider2D>());
                    }
                    if (f < waitForSound)
                    {
                        f = Time.time;
                        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                endCollider.GetComponent<hatThrownEnds>().hasCollidedWithPlayer = false;
                sideCollider.GetComponent<hatThrownEnds>().hasCollidedWithOther = false;
            }
        }
        
        if (isStuck)
        {
            gameObject.layer = 0;
            if(stuckTime + stuckDuration <= Time.time)
            {
                mandfred.GetComponent<MANDFRED_script>().hatIsOnHead = true;
                Destroy(gameObject);

            }
            if(au.clip != wearOutSound)
            {
                au.clip = wearOutSound;
                au.Play((ulong)0.25);
            }
        }
        else if (!onTheWayBack) //hatten flyver ud i en lige linje, og deaccelerer;
        {
            rBody.velocity = Vector2.MoveTowards(rBody.velocity, Vector2.zero, backAccel);

            if(rBody.velocity == new Vector2(0, 0))
            {
                onTheWayBack = true;
            }
        }
        else //Efter at hatten er kastet, boomerang'er den tilbage
        {
            if (!hasCollidedWithObject)
            {
                travelDir = new Vector2(rBodyMandfred.position.x - rBody.position.x, rBodyMandfred.position.y - rBody.position.y);
                Vector2 travelDirUnit = travelDir.normalized;
                speed += backAccel;
                rBody.velocity = travelDirUnit * speed;
            }
            else
            {
                rBody.bodyType = RigidbodyType2D.Static;
            }
        }
	}
}
