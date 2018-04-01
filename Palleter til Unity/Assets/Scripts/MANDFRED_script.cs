﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MANDFRED_script : MonoBehaviour {


    public Vector3 Spawnlocation;

    public bool hatIsOnHead;
    public GameObject hatThrown;

    public float hastighed;
    public float movementAcceleration;
    public float groundDeAcceleration;
    public float airResistanceDeAcceleration;
    public float floatPower;
    public float fallPower;
    public float Speed;
    public float jumpPower;

    public float distToGround;
    public float distToLeft;
    public float distToRight;
    public bool isTouchingGround;

    public float jumpBuffer;

    public Animator animator;

    public AudioClip walkingSound;
    public AudioClip deathSound;

    GameObject[] jumpRayOrigins;
    Rigidbody2D rbody;
    GameObject hatOnhead;
    AudioSource au;
    Collider2D jumpCollider;
    
    Vector3 gpos;
    Vector2 mousePos;

    public GameObject hatJustThrown;
    public bool disableControls;


    void checkForColisions()
    {
        // Vi ser på om mandfred står på jorden
        
        foreach (GameObject origin in jumpRayOrigins)
        {
            RaycastHit2D downDist = Physics2D.Raycast(origin.transform.position, new Vector2(0, -1));
            distToGround = downDist.distance;
            if (distToGround <= jumpBuffer)
            {
                isTouchingGround = true;
                animator.SetBool("isOnGround", true);
                break;
            }
            else
            {
                isTouchingGround = false;
                animator.SetBool("isOnGround", false);
            }
        }
        
    }

    public Vector2 GetMousePosition() //finder musens position, og giver den tilbage som en Vector2
    {
        Event ev = Event.current;
        Vector2 holder;
        Camera mc = Camera.main;

        holder.x = ev.mousePosition.x;
        holder.y = mc.pixelHeight - ev.mousePosition.y;
        holder = mc.ScreenToWorldPoint(new Vector3(holder.x, holder.y, mc.nearClipPlane));

        return holder;
    }


    void Movement()
    {
        if (!disableControls)
        {
            // Horizontal bevægelse
            // når man står på jorden
            if (isTouchingGround)
            {
                if (Input.GetKey("a"))
                {
                    if (hastighed >= -Speed)
                    {
                        hastighed = Mathf.MoveTowards(hastighed, -Speed, movementAcceleration);
                    }
                }
                else if (Input.GetKey("d"))
                {
                    if (hastighed <= Speed)
                    {
                        hastighed = Mathf.MoveTowards(hastighed, Speed, movementAcceleration);
                    }
                }
                else //hvis hværken a eller d trykkes, stopper man helt.
                {
                    hastighed = Mathf.MoveTowards(hastighed, 0, groundDeAcceleration);
                }
            }
            // når man DI'er i luften
            else if (Input.GetKey("a"))
            {
                if (hastighed >= -Speed)
                {
                    hastighed = Mathf.MoveTowards(hastighed, -Speed, movementAcceleration);
                }
                else
                {
                    hastighed = Mathf.MoveTowards(hastighed, Speed, airResistanceDeAcceleration);
                }
            }
            else if (Input.GetKey("d"))
            {
                if (hastighed <= Speed)
                {
                    hastighed = Mathf.MoveTowards(hastighed, Speed, movementAcceleration);
                }
                else
                {
                    hastighed = Mathf.MoveTowards(hastighed, Speed, airResistanceDeAcceleration);
                }
            }
            else //hvis man ikke bevæger sig i luften, så beholder man den vertikale inerti. 
            {
                hastighed = Mathf.MoveTowards(hastighed, 0, airResistanceDeAcceleration);
            }

            rbody.velocity = new Vector2(hastighed, rbody.velocity.y);

            if (hastighed < 0)// vender spilleren
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (hastighed > 0)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }

            // Hop
            if (Input.GetKey(KeyCode.Space) || Input.GetKey("w"))
            {
                if (isTouchingGround)
                {
                    isTouchingGround = false;
                    animator.SetTrigger("Jump");
                    rbody.position = rbody.position - new Vector2(0, -distToGround -0.1f);
                    rbody.velocity = new Vector2(rbody.velocity.x, 0);
                    rbody.AddForce(new Vector2(0, 100 * jumpPower));
                }


            }
            if (!isTouchingGround) // forøg/forminsk faldehastighed
            {
                if (Input.GetKey("s"))
                {
                    rbody.AddForce(new Vector2(0, fallPower));
                }
                else if (Input.GetKey(KeyCode.Space) || Input.GetKey("w"))
                {
                    if (rbody.velocity.y > 0)
                    {
                        rbody.AddForce(new Vector2(0, floatPower));
                    }
                }
            }


            //animator/sound
            if (Input.GetKey("w") || Input.GetKey(KeyCode.Space))
            {
                animator.SetBool("isWalking", false);
            }
            else if (Input.GetKey("a") || Input.GetKey("d"))
            {
                animator.SetBool("isWalking", true);
                if (animator.GetBool("isOnGround") && !au.isPlaying)
                {
                    au.volume = 0.15f;
                    au.clip = walkingSound;
                    au.Play(0);
                }
            }
            else
            {
                if (au.clip == walkingSound)
                {
                    au.Stop();
                }
                animator.SetBool("isWalking", false);
            }
        }
    }


    void throwHat()
    {
        gpos = gameObject.transform.position;
        hatJustThrown = Instantiate(hatThrown, new Vector3(gpos.x, gpos.y, -9f), Quaternion.identity);
        hatJustThrown.GetComponent<HatThrown_script>().ThrowTowards(gpos, mousePos);
    }


    void hatHandler()
    {
        if (hatIsOnHead && isTouchingGround)
        {
            hatOnhead.SetActive(true);
        }
        else if (!hatOnhead.activeInHierarchy)
        {
            hatOnhead.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && hatOnhead.activeInHierarchy)
        {
            hatOnhead.SetActive(false);
            hatIsOnHead = false;
            throwHat();
        }
    }


    public void respawn()
    {
        rbody.velocity = Vector3.zero;
        hastighed = 0;
        rbody.position = Spawnlocation;
        Destroy(hatJustThrown);
        hatIsOnHead = true;
        au.volume = 1f;
        disableControls = false;
    }


    // Use this for initialization
    void Start () {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        jumpRayOrigins = GameObject.FindGameObjectsWithTag("JumpRayOrigin");
        hatOnhead = GameObject.FindGameObjectWithTag("HatOnHead");
        hatIsOnHead = true;
        animator = GameObject.Find("Body").GetComponent<Animator>();
        au = gameObject.GetComponent<AudioSource>();
    }


    private void OnGUI()
    {
        mousePos = GetMousePosition();
    }

    // Update is called once per frame
    void FixedUpdate () {
        checkForColisions();
        Movement();
        hatHandler();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.gameObject.tag == "deadly")
        {
            disableControls = true;
            respawn();
            au.clip = deathSound;
            au.Play(0);
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        
        if (coll.gameObject.tag == "deadly")
        {
            disableControls = true;
            respawn();
        }
    }
}
