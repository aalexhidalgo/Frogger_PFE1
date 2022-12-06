using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_DestroyAfterTime : MonoBehaviour
{
    //Objects with this script will be destroyed after the lifeTime passes
    public float lifeTime = 1.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
