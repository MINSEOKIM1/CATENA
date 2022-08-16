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
                // item.gameObject.GetComponent<CharacterBehavior>().TakeHit(damage, airborne, stunTime);
                // item.gameObject.GetComponent<TestMob>().TakeHit(damage, airborne, stunTime);
            }
        }
    }
}
