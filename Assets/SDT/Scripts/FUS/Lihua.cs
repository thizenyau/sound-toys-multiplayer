using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Lihua : MonoBehaviour
{
    public BodyData bodyData;
    public VisualEffect visualEffect;
    private bool isRising;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isRising = bodyData.handRise;
        if(isRising)
            visualEffect.SendEvent("Burst");
    }
}
