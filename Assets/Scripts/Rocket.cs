using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProccessInput();
    }

    private void ProccessInput()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            print("Thrusting");
        }

        if (Input.GetKey(KeyCode.A)) // rotate left but cannot rotate right at the same time, so use if-elseif
        {
            print("Rotating Left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating Right");
        }


    }
}
