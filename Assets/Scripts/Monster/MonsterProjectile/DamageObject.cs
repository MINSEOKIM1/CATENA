using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public bool isCollider;
    public Vector2 boxOffset, boxSize;
    public float damage;
    public Vector2 airborne;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + boxOffset, boxSize);
    }

    //파이어볼은 항상 데미지를 입힘
    private void OnCollisionEnter2D(Collision2D col){
        if(isCollider && col.gameObject.tag == "Player"){
            if (col.transform.position.x < transform.position.x)
                StartCoroutine(col.gameObject.GetComponent<CharacterBehavior>().TakeHit(damage, new Vector2(-airborne.x, airborne.y)));
            else
                StartCoroutine(col.gameObject.GetComponent<CharacterBehavior>().TakeHit(damage, new Vector2(airborne.x, airborne.y)));
        }
    }

    //번개랑 폭발은 animationEvent로
    public void DoDamge(){
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
            if (item.tag == "Player")
            {
                Debug.Log("Player Hit!! Damage : " + damage);
                if (item.transform.position.x < transform.position.x)
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

    public void Delete(){
        GameObject.Destroy(this.gameObject);
    }

}
