using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.Netcode;

public class BodyData : NetworkBehaviour
{
    #region 身体部件transform定义
    public Transform head;
    public Transform chest;
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform leftShoulder;
    public Transform rightShoulder;
    public Transform star;

    #endregion
    #region 1、手间距
    private float HandDistance()//手间距具体数值
    {
        float handDistance = Vector3.Distance(leftHand.position, rightHand.position);
        return handDistance;
    }
    public float HandDistance0_1()//手间距归一化数值
    {
        float handDistance = Vector3.Distance(leftHand.position, rightHand.position);
        float handDistance0_1 = Mathf.Clamp(handDistance / 1.524972f , 0f , 1f);
        return handDistance0_1;
    }
    #endregion
    #region 2、拍手
    private bool isClaping = false;//是否拍手的判断
    private bool lastClaping = false;//上一帧是否拍手的判断
    public bool clapBegin = false;//拍手开始的判断
    private bool clapEnd = false;//拍手结束的判断
    private void Clap()//拍手计算
    {
        //初始化
        clapBegin = false;
        clapEnd = false;
        lastClaping = isClaping;
        if (HandDistance() < 0.2f && !isSurrounding)
        {
            isClaping = true;
        }
        else
        {
            isClaping = false;
        }
        if (isClaping && !lastClaping)
        {
            clapBegin = true;
        }
        else if (!isClaping && lastClaping)
        {
            clapEnd = true;
        }
    }
    #endregion
    #region 3、手的高度
    public bool handRise = false;
    private bool isHighested = false;
    
    public float LeftHandHeight0_1()//左手高度归一化数值
    {
        float leftHandHeight = leftHand.position.y - leftFoot.position.y;
        float leftHandHeight0_1 = Mathf.Clamp(leftHandHeight / 2.120617f, 0f, 1f);
        return leftHandHeight0_1;
    }
    private float RightHandHeight0_1()//右手高度归一化数值
    {
        float rightHandHeight = rightHand.position.y - rightFoot.position.y;
        float rightHandHeight0_1 = Mathf.Clamp(rightHandHeight / 2.120617f, 0f, 1f);
        return rightHandHeight0_1;
    }
    private bool isHighest()//双手是否为最高点的判断
    {
        bool isHighest = false;
        if (LeftHandHeight0_1() > 0.9f && RightHandHeight0_1() > 0.9f)
        {
            isHighest = true;
        }
        return isHighest;
    }
    private void Rise()
    {
        if (isHighest() && !isHighested)
            handRise = true;
        else 
            handRise = false;
        isHighested = isHighest();
    }
    #endregion
    #region 4、按压
    public bool isPressing = false;//是否按压的判断
    private bool heightCheck = false;//高度检测
    private bool distanceCheck = false;//距离检测
    private void Press()//按压判断
    {
        heightCheck = (leftHand.position.y - rightHand.position.y) < 0.2f;
        distanceCheck = HandDistance() < 0.4f && HandDistance() > 0.2f;
        if (heightCheck && distanceCheck && !isHighest())
        {
            isPressing = true;
        }
        else
        {
            isPressing = false;
        }
    }
    #endregion
    #region 5、摸头
    public bool isTouchingHead = false;//是否摸头的判断

    private void TouchHead()//摸头判断
    {
        if (Vector3.Distance(leftHand.position, head.position) < 0.3f && Vector3.Distance(rightHand.position, head.position) < 0.3f && !isHighest())
        {
            isTouchingHead = true;
        }
        else
        {
            isTouchingHead = false;
        }
    }
    #endregion
    #region 6、触摸——还没写需要引入视觉的位置
    #endregion
    #region 7、头的角度
    private bool isAngleing = false;
    private float headAngle = 0f;
    private float chestAngle = 0f;
    private float angleDiff = 0f;
    private float diff1 = 0f;
    private float diff2 = 0f;
    private void HeadAngle()
    {
        headAngle = head.eulerAngles.y;
        chestAngle = chest.eulerAngles.y;
        diff1 = Mathf.Abs(headAngle - chestAngle);
        diff2 = Math.Max(headAngle , chestAngle) + 360 - Math.Min(headAngle , chestAngle);
        angleDiff = Math.Min(diff1 , diff2);
        isAngleing = angleDiff > 45;
    }
    #endregion
    #region 8、转圈
    private bool isTurning = false;//是否转圈的判断
    public bool startTurning = false;
    public bool endTurning = false;
    private float turningTimer = 0f;
    //private float rotateAngle = 0f;//转圈角度
    private float rotatemark1 , rotatemark2 , rotatemark3 , rotatemark4 = 0f;
    //mark1记录初始值，mark2记录每一帧的数据，当差值较大，mark3记录现在的数据。
    private void Turn()
    {
        startTurning = false;
        if (turningTimer > 0f)
        {
            if (turningTimer == 2f)
                endTurning = true;
            else
                endTurning =  false;
            isTurning = true;
            turningTimer -= 1f;
        }
        else
        {

            isTurning = false;

            if (rotatemark1 == 0f)
            {
                rotatemark1 = chest.rotation.eulerAngles.y;
            }
            rotatemark2 = chest.rotation.eulerAngles.y;
            if (Math.Abs(rotatemark1 - rotatemark2) > 90f)
                rotatemark3 = 1f;
            if (rotatemark3 != 0f)
            {
                if (Math.Abs(rotatemark1 - rotatemark2) < 20f)
                {
                    turningTimer = 1000f;
                    startTurning = true;
                    rotatemark1 = 0f;
                    rotatemark2 = 0f;
                    rotatemark3 = 0f;
                }
            }
        }
    }
    #endregion
    #region 9、拍地
    public bool isPatting = false;
    private void Pat()
    {
        isPatting = Mathf.Abs(leftHand.position.y - leftFoot.position.y) < 0.3f || Mathf.Abs(rightHand.position.y - rightFoot.position.y) < 0.3f;
    }
    #endregion
    #region 10、甩手
    private bool isShaking = false;
    public bool startShaking = false;
    private float shakeMark1 , shakeMark2 , shakeMark3 , shakeMark4 = 0f;
    private float shakeTimer = 0f;
    private void Shake()
    {
        startShaking = false;
        isShaking = false;
        if (shakeTimer > 0)
        {
            isShaking = true;
            shakeTimer -= 1;
            return;
        }
        shakeMark2 = shakeMark1;
        shakeMark1 = leftHand.position.y;
        shakeMark4 = shakeMark3;
        shakeMark3 = rightHand.position.y;
        if((shakeMark2 - shakeMark1 > 0.09f || shakeMark4 - shakeMark3 > 0.09f) && !isShaking)
        {
            shakeTimer = 100f;
            startShaking = true;
        }
    }
    #endregion
    #region 11、低头抱胸
    public bool isSurrounding = false;
    private bool surroundingCheck1 = false;
    private bool surroundingCheck2 = false;
    private void Surround()
    {
        isSurrounding = (Vector3.Distance(leftHand.position , rightShoulder.position)< 0.3f) && (Vector3.Distance(rightHand.position , leftShoulder.position)< 0.3f);
    }
    #endregion
    #region 12、双手画圈抬起

    #endregion
    #region 13、两侧抬起
    
    #endregion
    #region NetCode
    public NetworkVariable<Vector3> _positionS2C = new NetworkVariable<Vector3>(Vector3.zero);
    public NetworkVariable<Vector3> _positionC2S = new NetworkVariable<Vector3>(Vector3.zero);
    public NetworkVariable<bool> _isShakingS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isShakingC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isGrowingS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isGrowingC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isSurroundingS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isSurroundingC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<float> _handDistanceS2C = new NetworkVariable<float>(0f);
    public NetworkVariable<float> _handDistanceC2S = new NetworkVariable<float>(0f);
    public NetworkVariable<bool> _isPressingS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isPressingC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<float> _handHeightS2C = new NetworkVariable<float>(0f);
    public NetworkVariable<float> _handHeightC2S = new NetworkVariable<float>(0f);
    public NetworkVariable<bool> _isTouchingHeadS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isTouchingHeadC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _startTurningS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _startTurningC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _endTurningS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _endTurningC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isPattingS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isPattingC2S = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isRisingS2C = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> _isRisingC2S = new NetworkVariable<bool>(false);


    #endregion
    [ServerRpc(RequireOwnership = false)]
    private void S2CServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong Id = serverRpcParams.Receive.SenderClientId;
        if (Id == 0)
        {
            _positionS2C.Value = star.position;
            _isShakingS2C.Value = startShaking;
            _isGrowingS2C.Value = clapBegin;
            _isSurroundingS2C.Value = isSurrounding;
            _handDistanceS2C.Value = HandDistance0_1() * 2;
            _isPressingS2C.Value = isPressing;
            _handHeightS2C.Value = LeftHandHeight0_1();
            _isTouchingHeadS2C.Value = isTouchingHead;
            _startTurningS2C.Value = startTurning;
            _endTurningS2C.Value = endTurning;
            _isPattingS2C.Value = isPatting;
            _isRisingS2C.Value = handRise;
        }



        if (Id == 1)
        {
            _positionC2S.Value = star.position;
            _isShakingC2S.Value = startShaking;
            _isGrowingC2S.Value = clapBegin;
            _isSurroundingC2S.Value = isSurrounding;
            _handDistanceC2S.Value = HandDistance0_1() * 2;
            _isPressingC2S.Value = isPressing;
            _handHeightC2S.Value = LeftHandHeight0_1();
            _isTouchingHeadC2S.Value = isTouchingHead;
            _startTurningC2S.Value = startTurning;
            _endTurningC2S.Value = endTurning;
            _isPattingC2S.Value = isPatting;
            _isRisingC2S.Value = handRise;
        }




    }

    void Start()
    {
        
    }

    void Update()
    {
        Clap();
        Rise();
        Press();
        TouchHead();
        HeadAngle();
        Turn();
        Pat();
        Shake();
        Surround();

        S2CServerRpc();
        if (endTurning)
            Debug.Log("end");
        }
}
