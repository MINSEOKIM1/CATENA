using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerSkill1 : MonoBehaviour
{
    void Start()
    {
        Invoke("Delete", 2f);
    }

    void Delete()
    {
        Destroy(gameObject);
    }
}
