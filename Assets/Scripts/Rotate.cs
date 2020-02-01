﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        var t = transform;
        t.RotateAround(t.position, t.up, rotateSpeed * rotateSpeed * Time.deltaTime);
    }
}
