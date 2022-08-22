using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovingObject : DamageObject
{
    public float speed;
    private void FixedUpdate(){
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    //이거 제대로 작동 안 할 수도 있음. ex) 카메라가 여러개
    //애초에 사라지는 구역을 정하고 하는게 나을수도
    void OnBecameInvisible()
    {
        GameObject.Destroy(this.gameObject);
    }
}
