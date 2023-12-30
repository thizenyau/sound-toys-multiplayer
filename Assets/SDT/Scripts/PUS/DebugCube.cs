using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.VFX;

public class DebugCube : NetworkBehaviour
{
    public BodyData bodyData;


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
        //RequestDestroyObject();
        bodyData = GameObject.Find("BodyData").GetComponent<BodyData>();
        if (bodyData == null)
            return;
    }

    // Update is called once per frame
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
    }
}
