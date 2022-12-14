using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBoxCheck : MonoBehaviour
{
    public Vector2 boxOffset, boxSize;
    public float damage;
    public Vector2 airborne;

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
            if (item.tag == "Player")
            {
                Debug.Log("Player Hit!! Damage : " + damage);
                if (item.transform.position.x < transform.parent.position.x)
                {
                    StartCoroutine(item.gameObject.GetComponent<CharacterBehavior>().TakeHit(damage, new Vector2(-airborne.x, airborne.y)));
                }
                else
                {
                    StartCoroutine(item.gameObject.GetComponent<CharacterBehavior>().TakeHit(damage, airborne));
                }
            }
        }
    }

    public bool AttackBoundaryCheck(bool attacked)
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
            if (item.tag == "Player")
            {
                Debug.Log("Player Hit!! Damage : " + damage);
                float monsterDamage = attacked ? damage : 3;
                if (item.transform.position.x < transform.parent.position.x)
                {
                    StartCoroutine(item.gameObject.GetComponent<CharacterBehavior>().TakeHit(monsterDamage, new Vector2(-airborne.x, airborne.y)));
                }
                else
                {
                    StartCoroutine(item.gameObject.GetComponent<CharacterBehavior>().TakeHit(monsterDamage, airborne));
                }
                return true;
            }
        }

        return false;
    }
}
