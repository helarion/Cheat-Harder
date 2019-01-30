using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    bool playerProjectile;
    public AudioSource source;
    public Vector2 direction;

    public void Start()
    {
        //source = GetComponent<AudioSource>();
        source.pitch = Random.Range(-3, 3);
        //source.Play();
    }

    public void SetOrigin(bool playerOrigin)
    {
        playerProjectile = playerOrigin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Plat"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && !playerProjectile)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
