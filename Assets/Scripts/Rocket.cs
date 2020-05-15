using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{


    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float upThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    ParticleSystem rocketThrust;
    
    

    bool soundPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rocketThrust = GetComponent<ParticleSystem>();
        
        

    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Dead": Destroy(gameObject);
                break;

              
        }
    }


    public void Rotate()
    {

        rigidBody.freezeRotation = true;
        float rotThisFrame = Time.deltaTime * rcsThrust;

        if (Input.GetKey(KeyCode.A)) // rotate left but cannot rotate right at the same time, so use if-elseif
        {
            /*rigidBody.AddRelativeTorque(Vector3.forward * 2);*/
            gameObject.transform.Rotate(Vector3.forward * rotThisFrame);



        }
        else if (Input.GetKey(KeyCode.D))
        {
            /*rigidBody.AddRelativeTorque(Vector3.back * 2);*/
            gameObject.transform.Rotate(Vector3.back * rotThisFrame);
        }
        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        float thrustThisFrame = Time.deltaTime * upThrust;
        if (Input.GetKey(KeyCode.W)) // can thrust while rotating
        {

            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!soundPlaying)
            {
                rocketThrust.Play();
                audioSource.Play();
                soundPlaying = true;
            }


        }
        else
        {
            if (soundPlaying)
            {
                audioSource.Stop();
                rocketThrust.Stop();
                soundPlaying = false;
            }

        }
    }
}
