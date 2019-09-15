using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject myObject, otherObject;

    void Start()
    {
        Type collidertype = myObject.GetComponent<Collider2D>().GetType();
        Component component=otherObject.AddComponent(collidertype);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
