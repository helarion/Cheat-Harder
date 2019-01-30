using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float FrozenTime;
    public float TimeUnfrozen;

    bool anyInput;
    bool isRestarting;

    private void Start()
    {
        anyInput = false;
    }

    // Update is called once per frame
    void Update () {
        anyInput = false;

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Rewind") != 0 || 
            Input.GetAxisRaw("Jump") != 0 || Input.GetAxisRaw("Menu")!=0 || 
            Input.GetAxisRaw("Forward")!=0 || Input.GetAxisRaw("Crouch")!=0)
        {
            anyInput = true;
        }
        if (Input.GetAxisRaw("Revive") != 0) isRestarting = true;

        if (!Input.anyKey || !anyInput)
        {
            Time.timeScale = FrozenTime;
        }
        if (anyInput || (isRestarting && GameManager.instance.isDead))
        {
             Time.timeScale = TimeUnfrozen;
        }
    }
}
