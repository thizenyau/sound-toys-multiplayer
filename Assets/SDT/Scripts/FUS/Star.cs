using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Star : MonoBehaviour
{
    public BodyData bodyData;
    private float handDIstance0_2;
    private bool isPressing = false;
    private float leftHandHeight = 0f;
    private bool isTouchingHead = false;
    private string name1 = "Range";
    private string name2 = "VD";
    private string name3 = "Shape";
    public VisualEffect visualEffect;
    void Start()
    {
        visualEffect = gameObject.GetComponent<VisualEffect>();
    }

    
    void Update()
    {
        handDIstance0_2 = bodyData.HandDistance0_1() * 2;
        isPressing = bodyData.isPressing;
        leftHandHeight = bodyData.LeftHandHeight0_1();
        isTouchingHead = bodyData.isTouchingHead;
        if (isPressing)
            visualEffect.SetFloat(name2 , Mathf.Clamp(((0.5f - leftHandHeight)*16 +1), 1 ,8));
        visualEffect.SetFloat(name1 , handDIstance0_2);
        if (isTouchingHead)
            visualEffect.SetFloat(name3 , 0f);
        else
            visualEffect.SetFloat(name3 , 6f);
    }
}
