using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Grow : MonoBehaviour
{
    public BodyData bodyData;
    private bool isClaping = false;
    public VisualEffect visualEffect;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        isClaping = bodyData.clapBegin;
        if (isClaping)
        {
            visualEffect.SendEvent("GrowSend");

        }
    }
}
