using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MyDistanceHandler : MonoBehaviour
{
    public Transform targetObject1;
    public Transform targetObject2;
    public Transform referenceObject;
    public VisualEffect visualEffect;
    public string parameterName = "CombinedDistance";
    public float maximumDistance = 0.3f;

    void Update()
    {
        // 检查引用是否存在
        if (targetObject1 == null || targetObject2 == null || referenceObject == null || visualEffect == null)
        {
            Debug.LogError("Please assign all references in the inspector.");
            return;
        }

        // 计算两个物体分别和第三个物体的距离
        float distance1 = Vector3.Distance(targetObject1.position, referenceObject.position);
        float distance2 = Vector3.Distance(targetObject2.position, referenceObject.position);

        // 检查距离是否都小于 0.3 米
        if (distance1 < maximumDistance && distance2 < maximumDistance)
        {
            // 将距离之和传递给 VFX Graph 的参数
            visualEffect.SetFloat(parameterName, distance1 + distance2);
        }
        else
        {
            // 如果任何一对物体的距离大于等于 0.3 米，可以选择在这里执行其他逻辑
            // 例如重置参数值或执行其他操作
        }
    }
}
