using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target;
    private float speed = 200f;
    private float angularSpeed = 10f;
    public GameObject explosionEffect;
    public AudioSource audioSource;
    public AudioClip explosionAudioClip;
    private bool isPlaySound = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true) // N?u qu? tên l?a ?ang active
        {
            if (target != null) // Ki?m tra target
            {
                if (Vector3.Distance(target.transform.position, transform.position) >= 0.1) // Ki?m tra kho?ng cách
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime); // Di chuy?n
                    Vector3 lookDirection = target.transform.position - transform.position;
                    lookDirection.Normalize();
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), angularSpeed * Time.deltaTime); // ??i h??ng
                }

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
            }

        }


    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
        {
            if (!isPlaySound)
            {
                audioSource.clip = explosionAudioClip;
                audioSource.Play();
                isPlaySound = true;
                Debug.Log("BOOOOOOM");
                GameObject gameObjectClone = Instantiate(explosionEffect,
                    collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
                Destroy(gameObjectClone, 5f);
            }

            Destroy(gameObject, 3f);
        }

    }
    public void setTarget(GameObject gameObjectTarget)
    {
        target = gameObjectTarget;
    }
}
