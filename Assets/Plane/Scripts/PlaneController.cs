using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlaneController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject machineGun1; 
    public GameObject machineGun2;
    public ParticleSystem impactEffect; // Hi?u ?ng sau khi va ch?m x?y ra

    private float speed = 0f; // T?c ?? máy bay hi?n t?i
    private float maxSpeed = 150f; // T?c ?? máy bay t?i ?a
    private float speedAcceleration = 0.3f; // Gia t?c máy bay
    private float speedDecay = 0.5f; // Giá tr? hao mòn t?c ??
    private float pitchSpeed = 60; // T?c ?? nâng h? ??u máy bay
    private float decayRotationSpeed = 0.5f;
    private float gravity = 2f; // ??i di?n cho tr?ng l?c ?? máy bay r?i xu?ng
    private float yawSpeed = 150f; // ?i?u khi?n t?c ?? l?t cánh máy bay
    private float rollSpeed = 150f;
    bool isThrottleOn = false; // N?u ??ng c? ?ang ???c b?t
    bool isGrounded = false; // N?u máy bay ?ã ti?p ??t
    float rotation = 0;
    bool isPlayingEngineSound = false;

    // Shoot
    float currentTimeShoot = 0;
    float currentTimeLauchRocket = 0; // Th?i gian l?n cu?i cùng b?n tên l?a

    // Rocket
    int numberRocket = 10;
    bool isHomingRadarActive = true; // Homing radar is active? Homing missile will go to target lock-on
    public GameObject missile; // Prefab tên l?a
    public GameObject targetLockOn; // M?c tiêu cho tên l?a
    public GameObject propeller; // Cánh qu?t máy bay
    public float propellerCurrentSpeed = 0; // T?c ?? cánh qu?t hi?n t?i
    public float propellerMaxSpeed = 30; // T?c ?? cánh qu?t t?i ?a
    public float propellerAcceleration = 0.2f; // Gia t?c cánh qu?t

    // Speech to text
    public string[] keywords = new string[] { "start", "stop", "kill" }; // Keyword ?? ?i?u khi?n máy bay b?ng gi?ng nói
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    // protected string word = "right";
    private KeywordRecognizer recognizer;

    void Start()
    {
        currentTimeShoot = currentTimeShoot = Time.time;

        recognizer = new KeywordRecognizer(keywords, confidence);
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //float translation = Input.GetAxis("Vertical") * speed;
        // float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        if (Input.GetKey(KeyCode.LeftShift)) // N?u nh?n shift, nâng ??u máy bay
        {
            //Debug.Log("Pitch forward");
            rotation = -Time.deltaTime * pitchSpeed;
            transform.Rotate(rotation, 0, 0);

        }
        if (Input.GetKey(KeyCode.LeftControl)) // Nh?n control, h? ??u máy bay xu?ng
        {
            //Debug.Log("Pitch forward");
            rotation = +Time.deltaTime * pitchSpeed;
            transform.Rotate(rotation, 0, 0);

        }
        /*else
        {
            //Vector3 to = new Vector3(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, Time.deltaTime * decayRotationSpeed);
            //Debug.Log(transform.eulerAngles.x);
        }*/
        if (Input.GetMouseButton(0)) // Shoot
        {
            /*if (Time.time - currentTimeShoot > 0.2f)
            {
                RaycastHit hit1;
                if (Physics.Raycast(machineGun1.transform.position, machineGun1.transform.forward, out hit1, 10000))
                {
                    Instantiate(impactEffect, hit1.point, Quaternion.LookRotation(hit1.normal));
                }
                RaycastHit hit2;
                if (Physics.Raycast(machineGun2.transform.position, machineGun2.transform.forward, out hit2, 10000))
                {
                    Instantiate(impactEffect, hit2.point, Quaternion.LookRotation(hit2.normal));
                }
                currentTimeShoot = Time.time;
            }*/

        }
        else if (Input.GetMouseButton(1)) // Lauch Rocket
        {
            if (Time.time - currentTimeLauchRocket > 2f) // Th?i gian b?t tên l?a ?ã kh? thi
            {
                if (targetLockOn != null) // If target is lock-on, spawn missile and move to target
                {
                    //Debug.Log("Collision");
                    GameObject missileClone = Instantiate(missile, missile.transform.position, missile.transform.rotation);
                    
                    missileClone.GetComponent<MissileController>().setTarget(targetLockOn);
                    missileClone.SetActive(true);
                }
                else // Clone and move missile forward
                {
                    Debug.Log(missile.transform);
                    GameObject missileClone = Instantiate(missile, missile.transform.position, missile.transform.rotation);
                    missileClone.SetActive(true);
                    missileClone.GetComponent<MissileController>().setTarget(null);
                }
                currentTimeLauchRocket = Time.time;
            }
        }

        // Make it move 10 meters per second instead of 10 meters per frame...
        //translation *= Time.deltaTime;
        //rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            isThrottleOn = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            isThrottleOn = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, rollSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -rollSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, yawSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -yawSpeed * Time.deltaTime, 0);
        }
        if (isThrottleOn) // N?u ??u máy bay ?ang ???c nâng
        {
            
            if (speed < maxSpeed)
            {
                speed += speedAcceleration; // T?ng speed hi?n t?i c?a máy bay
            }
            if (propellerCurrentSpeed < propellerMaxSpeed) // N?u cánh qu?t ch?a ??t t?c ?? t?i ?a
            {
                propellerCurrentSpeed += propellerAcceleration;
            }
            propeller.transform.Rotate(new Vector3(propellerCurrentSpeed, 0, 0)); // Quay cánh qu?t theo t?c ?? hi?n t?i
        }
        else
        {
            if (speed > 0)
            {
                speed -= speedDecay; // Gi?m speed theo m?c ?? decay
            }
            if (speedDecay < 0)
            {
                speed = 0; // Speed luôn >= 0, t?c máy bay không th? ?i lùi
            }
            if (propellerCurrentSpeed > 0) // N?u cánh qu?t ch?a gi?m t?c xu?ng 0
            {
                propellerCurrentSpeed -= propellerAcceleration;
            }
            propeller.transform.Rotate(new Vector3(propellerCurrentSpeed, 0, 0)); // Quay cánh qu?t theo t?c ?? hi?n t?i
        }
        if (!isPlayingEngineSound && isThrottleOn)
        {
            gameObject.GetComponent<AudioSource>().Play();
            isPlayingEngineSound = true;
        }
        if (isPlayingEngineSound && !isThrottleOn)
        {
            gameObject.GetComponent<AudioSource>().Stop();
            isPlayingEngineSound = false;
        }
        /*if (!isGrounded) // Ki?u ch? máy bay ?ã ch?m ??t ch?a
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 
                gravity * Time.deltaTime, gameObject.transform.position.z);
        }*/
    }
    void OnCollisionEnter(Collision theCollision) // G?i khi có va ch?m
    {
        // Console.Out.WriteLine("Collision");
        if (theCollision.gameObject.name == "Terrain")
        {
            isGrounded = true;
        }
    }
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args) // G?i khi phát hi?n ???c t? kh?p v?i list t? ?ã cho
    {
        Debug.Log(args.text);
        if (args.text.Equals("stop"))
        {
            isThrottleOn = false;
        }
    }
    private void OnApplicationQuit() // G?i khi quit ch??ng trình
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
