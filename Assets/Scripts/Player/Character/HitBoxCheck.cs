using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitBoxCheck : MonoBehaviour
{
    public Vector2 boxOffset, boxSize;
    public float damage;
    public Vector2 airborne;
    public float stunTime;

    public CinemachineVirtualCamera cine;
    [SerializeField] private Vector2 defaultBoxOffset;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.parent.position + defaultBoxOffset + boxOffset, boxSize);
    }

    private void Start()
    {
        cine = transform.parent.parent.GetComponent<ExpeditionManager>().cine;
    }

    public void AttackBoundaryCheck()
    {
        Vector2 offset = boxOffset;
        
        if (transform.parent.localScale.x < 0)
        {
            offset = new Vector2(-boxOffset.x, boxOffset.y);
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.parent.position + offset,
            boxSize, 0);

        
        foreach (Collider2D item in collider2Ds)
        {
            if (item.tag == "Mob")
            {
                Debug.Log("Hit!! Damage : " + damage);
                if (item.transform.position.x < transform.parent.position.x)
                {
                    item.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, new Vector2(-airborne.x, airborne.y), stunTime);
                }
                else
                {
                    item.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, airborne, stunTime);
                }
                // item.gameObject.GetComponent<TestMob>().TakeHit(damage, airborne, stunTime);
            }
        }
    }
}
