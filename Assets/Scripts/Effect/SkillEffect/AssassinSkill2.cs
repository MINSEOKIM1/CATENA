using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssassinSkill2 : MonoBehaviour
{
    public int n;
    public GameObject shuriken;
    public GameObject effect;

    public float damage;
    public float stun;
    public Vector2 airborne;
    public float speed;

    public void SetAttack(float damage, float speed, float stun, Vector2 airborne, int n)
    {
        this.damage = damage;
        this.speed = speed;
        this.stun = stun;
        this.airborne = airborne;
        this.n = n;

        StartCoroutine(Attack());

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 10);
        bool contain = false;
        if (hits.Length > 0)
        {
            foreach (var i in hits)
            {
                if (i.collider.tag == "Ground")
                {
                    contain = true;
                    transform.position = i.point + Vector2.up;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        Instantiate(effect, transform.position, Quaternion.identity);

        if (n > 0)
        {
            yield return new WaitForSeconds(0.05f);
            var i = Instantiate(gameObject,
                transform.position + new Vector3(Random.Range(-5.0f, 5.0f), 0, 0),
                Quaternion.identity);
            i.GetComponent<AssassinSkill2>().SetAttack(damage, speed, stun, airborne, n-1);
        }
        
        yield return new WaitForSeconds(0.5f);
        var s = Instantiate(shuriken, transform.position, Quaternion.identity);
        Explosion t = s.GetComponentInChildren<Explosion>();
        t.SetInfo(damage, stun, airborne);
        Destroy(gameObject);
    }
}
