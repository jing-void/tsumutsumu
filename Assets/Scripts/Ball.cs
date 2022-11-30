using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public int id;
    public GameObject burstPrefab;
    public void Explosion()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(burstPrefab,transform.position,transform.rotation);
        Destroy(explosion,0.3f);
    }

    public bool IsBomb()
    {
        return id == -1;
    }
}
