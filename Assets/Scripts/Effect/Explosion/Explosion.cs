using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage;
    public Vector2 airborne;
    public float stun;

    public Vector2 boxOffset;
    public Vector2 boxSize;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + boxOffset, boxSize);
    }

    public void SetInfo(float damage, float stun, Vector2 airborne)
    {
        this.damage = damage;
        this.airborne = airborne;
        this.stun = stun;
        Invoke("DeleteThis", 5f);
    }

    public void AttackBoundaryCheck()
    {
        Vector2 offset = boxOffset;
        
        if (transform.localScale.x < 0)
        {
            offset = new Vector2(-boxOffset.x, boxOffset.y);
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + offset,
            boxSize, 0);

        
        foreach (Collider2D item in collider2Ds)
        {
            if (item.tag == "Mob")
            {
                Debug.Log("Hit!! Damage : " + damage);
                if (item.transform.position.x < transform.parent.position.x)
                {
                    item.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, new Vector2(-airborne.x, airborne.y), stun);
                }
                else
                {
                    item.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, airborne, stun);
                }
                // item.gameObject.GetComponent<TestMob>().TakeHit(damage, airborne, stunTime);
            }
        }
    }

    private void DeleteThis()
    {
        Destroy(gameObject);
    }
}
