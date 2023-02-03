using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    private float motorForce = 1000f;
    private float steerAngle = 30f;
    private float breakForce = 0f;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public GameObject turbo1;
    public GameObject turbo2;

    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource carAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;



    void Start()
    {
        carAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        if (Input.GetKey(KeyCode.Space))
        {
            // Debug.Log("Space");
            breakForce = 10000f;
        }
        else
        {
            breakForce = 0f;
        }
        frontRightWheelCollider.brakeTorque = breakForce;
        frontLeftWheelCollider.brakeTorque = breakForce;
        rearLeftWheelCollider.brakeTorque = breakForce;
        rearRightWheelCollider.brakeTorque = breakForce;
        // Debug.Log(horizontalInput);
        frontRightWheelCollider.steerAngle = horizontalInput * steerAngle;
        frontLeftWheelCollider.steerAngle = horizontalInput * steerAngle;
        // rearLeftWheelCollider.steerAngle = horizontalInput * steerAngle * Time.deltaTime;
        //rearRightWheelCollider.steerAngle =  * steerAngle * Time.deltaTime;

        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {

        }

        EngineSound();
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;
        pitchFromCar = carRb.velocity.magnitude / 60f;

        if (currentSpeed < minSpeed)
        {
            carAudio.pitch = minPitch;
        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            carAudio.pitch = minPitch + pitchFromCar;
        }

        if (currentSpeed > maxSpeed)
        {
            carAudio.pitch = maxPitch;
        }
    }
}
