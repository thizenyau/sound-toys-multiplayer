using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.VFX;

public class Electricity : MonoBehaviour
{
    public BodyData bodyData;
    public VisualEffect visualEffect;
    private bool isShaking;
    void Start()
    {


        
    }

    void Update()
    {
        isShaking = bodyData.startShaking;
        if (isShaking)
            visualEffect.SendEvent("Thunder");
    }
}
