using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Wu : MonoBehaviour
{
    public BodyData bodyData;
    public VisualEffect visualEffect;
    private bool isSurrounding = false;
    void Start()
    {
        
    }

    void Update()
    {
        isSurrounding = bodyData.isSurrounding;
        if (isSurrounding)
            visualEffect.SendEvent("FogSend");
    }
}
