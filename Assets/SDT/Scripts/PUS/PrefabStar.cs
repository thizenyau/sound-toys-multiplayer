using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.VFX;



public class PrefabStar : NetworkBehaviour
{


    #region 定义变量
    public BodyData bodyData;
    

    #endregion
    #region Electricity
    public VisualEffect electricity;
    private bool isShaking;
    private void Electricity()
    {
        if (!IsServer)
            isShaking = bodyData._isShakingS2C.Value;
        else
            isShaking = bodyData._isShakingC2S.Value;
        if (isShaking)
                electricity.SendEvent("Thunder");
    }

    #endregion
    #region Growing
    public VisualEffect growing;
    private bool isGrowing;
    private void Growing()
    {
        if (!IsServer)
            isGrowing = bodyData._isGrowingS2C.Value;
        else
            isGrowing = bodyData._isGrowingC2S.Value;
        if (isGrowing)
                growing.SendEvent("GrowSend");
    }

    #endregion
    #region Wu
    public VisualEffect wu;
    private bool isSurrounding;
    private void Wu()
    {
        if (!IsServer)
            isSurrounding = bodyData._isSurroundingS2C.Value;
        else
            isSurrounding = bodyData._isSurroundingC2S.Value;
        if (isSurrounding)
                wu.SendEvent("FogSend");
    }

    #endregion
    #region Star
    public VisualEffect star;
    private string name1 = "Range";
    private string name2 = "VD";
    private string name3 = "Shape";
    private float handDIstance0_2;
    private bool isPressing = false;
    private float leftHandHeight = 0f;
    private bool isTouchingHead = false;
    private void Star()
    {
        if (!IsServer)
        {
            handDIstance0_2 = bodyData._handDistanceS2C.Value;
            isPressing = bodyData._isPressingS2C.Value;
            leftHandHeight = bodyData._handHeightS2C.Value;
            isTouchingHead = bodyData._isTouchingHeadS2C.Value;
        }
        else
        {
            handDIstance0_2 = bodyData._handDistanceC2S.Value;
            isPressing = bodyData._isPressingC2S.Value;
            leftHandHeight = bodyData._handHeightC2S.Value;
            isTouchingHead = bodyData._isTouchingHeadC2S.Value;            
        }
        star.SetFloat(name1 , handDIstance0_2);
        if (isPressing)
            star.SetFloat(name2 , Mathf.Clamp(((0.5f - leftHandHeight)*16 +1), 1 ,8));
        if (isTouchingHead)
            star.SetFloat(name3 , 0f);
        else
            star.SetFloat(name3 , 6f);

    }

    #endregion
    #region Wind
    public VisualEffect wind;
    private bool startTurning = false;
    private bool endTurning = false;
    private bool isPatting = false;
    private void Wind()
    {
        if (!IsServer)
        {
            startTurning = bodyData._startTurningS2C.Value;
            endTurning = bodyData._endTurningS2C.Value;
            isPatting = bodyData._isPattingS2C.Value;
        }
        else
        {
            startTurning = bodyData._startTurningC2S.Value;
            endTurning = bodyData._endTurningC2S.Value;
            isPatting = bodyData._isPattingC2S.Value;            
        }
        if (startTurning)
            wind.SendEvent("WindStart");
        if (endTurning || isPatting)
            wind.SendEvent("WindEnd");


    }


    #endregion
    #region Lihua
    public VisualEffect lihua;
    private bool isRising;
    private void Lihua()
    {
        if (!IsServer)
            isRising = bodyData._isRisingS2C.Value;
        else
            isRising = bodyData._isRisingC2S.Value;
        if(isRising)
            lihua.SendEvent("Burst");


    }


    #endregion



    [ClientRpc]
    public void RpcDestroyObjectClientRpc()
    {
        if (IsServer)
            Destroy(gameObject);
    }

    public void RequestDestroyObject()
    {
        if (IsClient && IsOwner)
            RpcDestroyObjectClientRpc();
    }

    void Start()
    {
        RequestDestroyObject();
        bodyData = GameObject.Find("BodyData").GetComponent<BodyData>();
        //transform = GetComponent<Transform>();


    }



    void Update()
    {
        if (bodyData == null)
        {
            bodyData = GameObject.Find("BodyData").GetComponent<BodyData>();
            return;

        }
        if (!IsServer)
            transform.position = bodyData._positionS2C.Value;
        if (IsServer)
            transform.position = bodyData._positionC2S.Value;


        Electricity();
        Growing();
        Wu();
        Star();
        Wind();
        Lihua();
    }
}
