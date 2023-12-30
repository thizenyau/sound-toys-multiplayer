using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateAroundTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float speed2 = 5f;
    public float distance;
    public float smooth = 1;

    private void Start()
    {
        distance = Vector3.Distance(transform.position, target.position);
    }

    void Update()
    {
        FollowOrRotate();
        //transform.position = Vector3.Lerp(transform.position, DesiredTargetPosition(), speed * Time.deltaTime);

    }

    public Vector3 DesiredTargetPosition()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        Vector3 vertical = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 desiredPosition = target.position + vertical * distance;
        return desiredPosition;
    }

    public Vector3 ToDesiredTargetSpeed()
    {
        Vector3 direction = (DesiredTargetPosition() - transform.position).normalized;
        Vector3 speedtodirection = direction * speed2 * Time.deltaTime;
        return speedtodirection;
    }

    public void FollowOrRotate()
    {
        float _distance = Vector3.Distance(transform.position, target.position);
        if (_distance > distance - smooth)
        {
            transform.position += ToDesiredTargetSpeed();
        }
      

        if (_distance < distance + smooth)
        {
            transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        }
       

    }
    
        
    
}
