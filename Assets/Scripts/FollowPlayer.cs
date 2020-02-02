using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    
    [Range(0,1)]
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        var myPos = transform.position;
        var targetPos = target.position;

        var newPos = Vector3.Lerp(myPos, targetPos, speed);

        transform.position = newPos;
    }
}
