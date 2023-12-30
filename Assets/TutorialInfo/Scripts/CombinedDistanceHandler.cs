using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CombinedDistanceHandler : MonoBehaviour
{
    public enum CalculationMode
    {
        TwoObjects,
        ThreeObjects
    }

    public CalculationMode calculationMode = CalculationMode.TwoObjects;

    public Transform targetObject1;
    public Transform targetObject2;
    public Transform object3;

    public VisualEffect vfxGraph;
    public string exposedParameterName = "DistanceParameter";
    public float maxDistance = 0.3f;
    public float resetValue = 6f; // 新增的参数，表示当距离大于等于 0.3 米时要设置的值

    void Update()
    {
        // 检查引用是否存在
        if (vfxGraph == null)
        {
            Debug.LogError("Please assign the VFX Graph reference in the inspector.");
            return;
        }

        if (calculationMode == CalculationMode.TwoObjects)
        {
            CalculateTwoObjectsDistance();
        }
        else if (calculationMode == CalculationMode.ThreeObjects)
        {
            CalculateThreeObjectsDistance();
        }
    }

    void CalculateTwoObjectsDistance()
    {
        // 检查引用是否存在
        if (targetObject1 == null || targetObject2 == null)
        {
            Debug.LogError("Please assign both target objects in the inspector.");
            return;
        }

        // 计算两个物体之间的相对距离
        float distance = Vector3.Distance(targetObject1.position, targetObject2.position);

        // 检查距离是否都小于 0.3 米
        if (distance < maxDistance)
        {
            // 将距离信息传递给 VFX Graph 的参数
            vfxGraph.SetFloat(exposedParameterName, distance);
        }
        else
        {
            // 如果距离大于等于 0.3 米，将参数刷新到 resetValue
            vfxGraph.SetFloat(exposedParameterName, resetValue);
        }
    }

    void CalculateThreeObjectsDistance()
    {
        // 检查引用是否存在
        if (targetObject1 == null || targetObject2 == null || object3 == null)
        {
            Debug.LogError("Please assign all three target objects in the inspector.");
            return;
        }

        // 计算两个物体分别和第三个物体的距离
        float distance1 = Vector3.Distance(targetObject1.position, object3.position);
        float distance2 = Vector3.Distance(targetObject2.position, object3.position);

        // 检查距离是否都小于 0.3 米
        if (distance1 < maxDistance && distance2 < maxDistance)
        {
            // 将距离之和传递给 VFX Graph 的参数
            vfxGraph.SetFloat(exposedParameterName, distance1 + distance2);
        }
        else
        {
            // 如果任何一对物体的距离大于等于 0.3 米，将参数刷新到 resetValue
            vfxGraph.SetFloat(exposedParameterName, resetValue);
        }
    }
}
