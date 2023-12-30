using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Wind : MonoBehaviour
{
    public BodyData bodyData;
    public VisualEffect visualEffect;
    private bool startTurning = false;
    private bool endTurning = false;
    private bool isPatting = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startTurning = bodyData.startTurning;
        endTurning = bodyData.endTurning;
        isPatting = bodyData.isPatting;
        if (startTurning)
            visualEffect.SendEvent("WindStart");
        if (endTurning || isPatting)
            visualEffect.SendEvent("WindEnd");
    }
}
