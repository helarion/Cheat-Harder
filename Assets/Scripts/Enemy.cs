using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    Rigidbody rb;
    public float speedMovement;
    int direction;
    public float routine;
    float initPosition;

    public Transform leftBulletSpawn;
    public GameObject bulletPrefab;
    public float fireSpeed;

    float nextTime;

	// Use this for initialization
	void Start () {
        nextTime = Time.time + fireSpeed;
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position.x;
        direction = -1;
	}

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isDead && !GameManager.instance.isRewinding)
        {
            Move();
            if (transform.position.x < (initPosition - routine))
            {
                direction = 1;
            }
            if (transform.position.x > (initPosition + routine))
            {
                direction = -1;
            }

            if (Time.time > nextTime)
            {
                ShootPlayer();
                nextTime = Time.time + fireSpeed;
            }
        }
    }

    void Move()
    {
        Vector3 movement = new Vector3(speedMovement * Time.deltaTime * direction, 0);
        Vector3 newPos = transform.position + movement;
        rb.MovePosition(newPos);
    }

    /*void LookForPlayer()
    {
        //RaycastHit hit;
        Transform player=GameObject.FindWithTag("Player").transform;
        Vector3 rayDirection = player.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, hit))
        {
            if (hit.transform == player)
            {
                // enemy can see the player!
            }
            else
            {
                // there is something obstructing the view
            }
        }
    }*/

    void ShootPlayer()
    {
        if (!GameManager.instance.isDead)
        {
            Vector3 target = GameObject.FindWithTag("Player").transform.position;
            Vector2 direction = target - leftBulletSpawn.position;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = (GameObject)Instantiate(bulletPrefab, leftBulletSpawn.position, rotation);
            projectile.GetComponent<Laser>().SetOrigin(false);
            projectile.GetComponent<Rigidbody>().velocity = direction;
            projectile.GetComponent<Laser>().direction = direction;
            projectile.GetComponent<Laser>().source.Play();


            // Add velocity to the bullet
            /*leftBullet.GetComponent<Rigidbody>().velocity = leftBullet.transform.forward * 6;
            rightBullet.GetComponent<Rigidbody>().velocity = leftBullet.transform.forward * 6;*/

            // Destroy the bullet after 2 seconds
            /* Destroy(leftBullet, 2.0f);
             Destroy(rightBullet, 2.0f);*/
            Destroy(projectile, 5f);
        }
    }
}
