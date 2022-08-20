using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Throwing : MonoBehaviour
{
    public float damage;
    public Vector2 airborne;
    public float stun;

    public GameObject effect;

    public float speed;
    public Vector3 dir;

    public void SetInfo(float damage, float stun, float speed, Vector2 airborne, Vector3 dir)
    {
        this.dir = dir;
        this.damage = damage;
        this.airborne = airborne;
        this.speed = speed;
        this.stun = stun;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));

        GetComponent<Rigidbody2D>().velocity = dir * speed;
        
        Invoke("DeleteThis", 5f);
    }
    
    public void SetInfo(float damage, float stun, float speed, Vector2 airborne, Vector3 dir, GameObject effect)
    {
        this.dir = dir.normalized;
        this.damage = damage;
        this.airborne = airborne;
        this.speed = speed;
        this.stun = stun;
        this.effect = effect;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));

        GetComponent<Rigidbody2D>().velocity = dir * speed;
        
        Invoke("DeleteThis", 5f);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Mob")
        {
            if (effect != null)
            {
                var eff = Instantiate(effect, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 90)));
                eff.GetComponentInChildren<Explosion>().SetInfo(damage, stun, airborne);
                Destroy(gameObject);
                return;
            }
            if (col.transform.position.x < transform.position.x)
            {
                col.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, new Vector2(-airborne.x, airborne.y), stun);
            }
            else
            {
                col.gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, airborne, stun);
            }

            
            Destroy(gameObject);
            return;
        }

        if (col.collider.tag == "Ground")
        {
            if (effect != null)
            {
                var eff = Instantiate(effect, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 90)));

                eff.GetComponentInChildren<Explosion>().SetInfo(damage, stun, airborne);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void DeleteThis()
    {
        Destroy(gameObject);
    }
}
