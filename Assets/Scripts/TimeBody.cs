using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour {

    private bool isRewinding = false;
    Rigidbody rb;
    List<PointInTime> pointsInTime;


	// Use this for initialization
	void Start () {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Rewind")>0 && !GameManager.instance.isDead)
        {
            GameManager.instance.isRewinding = true;
            StartRewind();
        }
        else
        {
            GameManager.instance.isRewinding = false;
            StopRewind();
        }
	}

    private void FixedUpdate()
    {
        if (isRewinding) Rewind();
        else Record();
    }

    void Record()
    {

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    void Rewind()
    {
        if(pointsInTime.Count >0)
        {
            PointInTime pointInTime = pointsInTime[0];

            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            if (gameObject.CompareTag("Laser")) Destroy(gameObject);
            StopRewind();
        }
    }

    void StartRewind()
    {
        GameManager.instance.filter.SetActive(true);
        isRewinding = true;
        rb.isKinematic = true;
    }

    void StopRewind()
    {
        GameManager.instance.filter.SetActive(false);
        isRewinding = false;
        rb.isKinematic = false;

        if (gameObject.CompareTag("Laser"))
        {
            gameObject.GetComponent<Laser>().SetOrigin(false);
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Laser>().direction * 10;
        }
    }

    public void ClearRecord()
    {
        pointsInTime.Clear();
    }
}
