using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;


    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float frequency;


    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = gameObject.transform.position;
        frequency = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        movementFactor = 0.5f * Mathf.Sin(Time.time * frequency) + 0.5f;
        

        Vector3 offset = movementVector * movementFactor;
        gameObject.transform.position = startingPos + offset;



        
    }
}
