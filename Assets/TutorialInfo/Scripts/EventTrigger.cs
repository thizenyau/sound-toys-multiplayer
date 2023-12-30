using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EventTrigger : MonoBehaviour
{
    public VisualEffect visualEffect;

    // 在需要的时候调用此方法触发事件
    public void TriggerEvent()
    {
        if (visualEffect != null)
        {
            visualEffect.SendEvent("SpawnEvent");
        }
    }
}
