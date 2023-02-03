using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAutoLight = false;
    public float lightSpeed = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutoLight)
        {
            gameObject.transform.Rotate(lightSpeed, 0, 0);
        }
    }
}
