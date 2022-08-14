using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector2 _origin;
    private Vector2 _targetOrigin;
    
    public float offset;
    public float damping;

    public GameObject target;
    private void Start()
    {
        _origin = transform.position;
        _targetOrigin = target.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = (Vector3) _origin - offset * ((Vector3) _targetOrigin - target.transform.position);
    }
}
