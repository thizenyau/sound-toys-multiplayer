using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableEffect : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public LineRenderer lineRenderer;
    private bool isCableVisibleOnOverlap = false;
    private bool isCableVisibleOnDistance = true;
    public float maxDistanceToDisable = 5f;

    void Update()
    {
        UpdateCable();
    }

    void UpdateCable()
    {
        // 计算物体之间的距离
        float distance = Vector3.Distance(object1.position, object2.position);

        // 根据距离判断是否显示或隐藏拉丝
        if (distance < 0.01f && !isCableVisibleOnOverlap)
        {
            // 两个物体重合时显示拉丝
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, object1.position);
            lineRenderer.SetPosition(1, object2.position);
            isCableVisibleOnOverlap = true;
        }
        else if (distance >= maxDistanceToDisable && isCableVisibleOnDistance)
        {
            // 距离超过5米时隐藏拉丝
            lineRenderer.positionCount = 0;
            isCableVisibleOnDistance = false;
            isCableVisibleOnOverlap = false; // 重置重合状态
        }
        else if (distance < maxDistanceToDisable && !isCableVisibleOnDistance)
        {
            // 距离在5米以内时重新显示拉丝
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, object1.position);
            lineRenderer.SetPosition(1, object2.position);
            isCableVisibleOnDistance = true;
        }
    }
}
