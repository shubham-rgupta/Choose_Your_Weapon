using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosion_Effect;
    public GameObject owner;
    public void SetTarget(GameObject newTarget)
    {
        owner = newTarget;
    }
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(explosion_Effect, transform.position, Quaternion.identity, this.transform);
        Destroy(gameObject,0.25f);
        if (owner.tag == "Player")
        {
            owner.GetComponent<Player>().BombThrow();
        }
        if (owner.tag == "Enemy")
        {
            owner.GetComponent<Enemy>().BombThrow();
        }
    }

}
