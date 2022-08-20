using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LivingShuriken : MonoBehaviour
{
    public float speed;
    
    public float damage;
    public Vector2 airborne;
    public float stun;

    public Vector2 boxOffset;
    public Vector2 boxSize;

    public Vector2 detectSize;

    public GameObject currentTarget;
    public Dictionary<GameObject, int> targets;
    public int hit;

    private void FixedUpdate()
    {
        transform.position += (currentTarget.transform.position - transform.position).normalized *
                              (speed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + boxOffset, boxSize);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + boxOffset, detectSize);
        
        Gizmos.DrawLine(currentTarget.transform.position, transform.position);
    }

    public void SetInfo(float damage, float stun, Vector2 airborne, GameObject target)
    {
        targets = new Dictionary<GameObject, int>();
        this.damage = damage;
        this.airborne = airborne;
        this.stun = stun;
        currentTarget = target;
        targets.Add(target, 0);
        Invoke("DeleteThis", 10);
        Debug.Log("ASDG");
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
                if (item.gameObject != currentTarget) continue;
                if (item.transform.position.x < transform.position.x)
                {
                    item.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, new Vector2(-airborne.x, airborne.y), stun);
                }
                else
                {
                    item.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, airborne, stun);
                }

                hit++;
                // item.gameObject.GetComponent<TestMob>().TakeHit(damage, airborne, stunTime);
            }
        }

        if (hit == 3)
        {
            ChangeTarget();
        }
    }

    private void ChangeTarget()
    {
        hit = 0;
        targets[currentTarget]++;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.position, detectSize, 0);
        int k = 100;
        GameObject o = currentTarget;

        foreach (var i in collider2Ds)
        {
            if (i.tag != "Mob") continue;
            if (targets.Count == 0 || !targets.ContainsKey(i.gameObject)) targets.Add(i.gameObject, 0);
        }

        foreach (var i in targets)
        {
            if (i.Value < k)
            {
                k = i.Value;
                o = i.Key;
            }

            if (i.Value == k)
            {
                if (Vector3.Distance(i.Key.transform.position, transform.position) <=
                    Vector3.Distance(o.transform.position, transform.position))
                {
                    k = i.Value;
                    o = i.Key;
                }
            }
        }
        
        if (o == currentTarget) Destroy(gameObject);

        currentTarget = o;

    }

    private void DeleteThis()
    {
        Destroy(gameObject);
    }
}
