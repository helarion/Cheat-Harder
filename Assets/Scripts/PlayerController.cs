using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform leftBulletSpawn;
    public Transform rightBulletSpawn;
    public GameObject bulletPrefab;
    public float speedMovement;
    public float speedJump;
    public Rigidbody rb;

    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;
    public GameObject Life4;

    public AudioSource jumpSource;
    public AudioSource hitSource;

    public bool isGrounded;

    public Transform corner1;
    public Transform corner2;

    public bool pressedJump;

    public int hp;
    public int savedHP;

    Animator animator;

    public GameObject body;

    public float fallMultiplier;
    public float lowJumpMultiplier;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        hp = 4;
        savedHP = hp;
        rb = GetComponent<Rigidbody>();
        //cl = GetComponent<Collider>();
        pressedJump = false;
        isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {

        isGrounded = CheckGrounded();
        Move();
        Jump();
        Crouch();

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !pressedJump)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void Crouch()
    {
        float crouchAxis = Input.GetAxis("Crouch");
        if (crouchAxis !=0 && !pressedJump)
        {
            animator.SetBool("IsCrouching", true);
        }
        else
        {
            animator.SetBool("IsCrouching", false);
        }
    }

    void Move()
    {
        float hAxis = Input.GetAxis("Horizontal");
        if (hAxis != 0)
        {
            Vector3 movement = new Vector3(hAxis * speedMovement * Time.deltaTime, 0);
            Vector3 newPos = transform.position + movement;
            rb.MovePosition(newPos);
        }
    }

    void Jump()
    {
        float jumpAxis = Input.GetAxis("Jump");
        if (jumpAxis > 0)
        {

            if (!pressedJump && isGrounded)
            {
                pressedJump = true;
                animator.SetBool("IsJumping", true);
                Vector3 jumpVector = new Vector3(0, jumpAxis * speedJump);
                rb.AddForce(jumpVector, ForceMode.VelocityChange);
                jumpSource.Play();
            }
        }
        else
        {
            pressedJump = false;
            if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("HasLanded", true);
            }
        }
    }

    bool CheckGrounded()
    {
        bool grounded1 = Physics.Raycast(corner1.position, -Vector3.up, 0.01f);
        bool grounded2 = Physics.Raycast(corner2.position, -Vector3.up, 0.01f);

        return (grounded1 || grounded2);
    }

    void OnTriggerEnter(Collider other)
    {
        string end = "end";
        int r = -1;

        for(int i=1;i<GameManager.instance.numberRoom;i++)
        {
            if (other.CompareTag(end + i))
            {
                r = i+1;
                break;
            }
        }

        if (r != -1)
        {
            //Debug.Log("opp="+opp);
            //Debug.Log("r=" + r);
            GameManager.instance.MoveCamera(r);
        }

        if (other.gameObject.CompareTag("Fall")) GameManager.instance.Dead();

        if (other.CompareTag("Bonus"))
        {
            GameManager.instance.GetBonus(other.gameObject);
            ResetHP(4); // on redonne toute sa vie au joueur
        }

        if (other.CompareTag("endLevel"))
        {
            GameManager.instance.EndLevel();
        }
    }

    public void TakeDamage()
    {
        GameManager.instance.shakeScreen.shakeDuration = 0.05f;
        hitSource.Play();
        hp --;
        UpdateLife();
        CheckDeath();
    }

    void CheckDeath()
    {
        if(hp<=0)
        {
            GameManager.instance.Dead();
        }
    }

    void UpdateLife()
    {
        switch(hp)
        {
            case 4:
                Life1.SetActive(true);
                Life2.SetActive(true);
                Life3.SetActive(true);
                Life4.SetActive(true);
                break;
            case 3:
                Life1.SetActive(true);
                Life2.SetActive(true);
                Life3.SetActive(true);
                Life4.SetActive(false);
                break;
            case 2:
                Life1.SetActive(true);
                Life2.SetActive(true);
                Life3.SetActive(false);
                Life4.SetActive(false);
                break;
            case 1:
                Life1.SetActive(true);
                Life2.SetActive(false);
                Life3.SetActive(false);
                Life4.SetActive(false);
                break;
            case 0:
                Life1.SetActive(false);
                Life2.SetActive(false);
                Life3.SetActive(false);
                Life4.SetActive(false);
                break;
        }
    }

    public void ResetHP(int newHP)
    {
        hp = newHP;
        UpdateLife();
    }
}
