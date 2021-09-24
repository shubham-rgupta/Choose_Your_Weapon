using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public static Melee obj;
    public float speed;
    public GameObject target;
    public GameObject explosion_Effect;
    public GameObject owner;

    private void Awake()
    {
        obj = this;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        
        if (target != null)
        {
            var dir2 = (target.transform.position - transform.position).normalized;
            dir2.y = 0;
            transform.parent = null;
            Vector3 look = dir2;
            look.y = 0.2f;
            transform.rotation = Quaternion.LookRotation(look);
            transform.position += speed * Time.deltaTime * dir2;
            
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.tag == "Castle")
        //{
        //    GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity);
        //    Destroy(eff, 2f);
        //    FeverMode.fMode.FeverOff();
        //    GameObject sphere = new GameObject();
        //    sphere.transform.parent = this.transform;
        //    sphere.transform.localPosition = Vector3.zero;
        //    sphere.AddComponent<SphereCollider>().radius = 1f;
        //    sphere.AddComponent<Rigidbody>().mass = 2.5f;
        //    Destroy(gameObject, 0.1f);
        //    print("Melee");
        //}
        if (other.gameObject.tag != owner.tag)
        {
            if (other.gameObject.tag == "Enemy")
            {
                GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity, target.transform);
                Destroy(eff, 2f);
                other.gameObject.GetComponent<Enemy>().health -= 50;
                Destroy(gameObject);
            }
            if (other.gameObject.tag == "Player")
            {
                GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity, target.transform);
                Destroy(eff, 2f);
                other.gameObject.GetComponent<Player>().health -= 50;
                Destroy(gameObject);
            }
            //if (other.gameObject.tag == "Stopper")
            //{
            //    GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity);
            //    Destroy(eff, 2f);
            //    Destroy(gameObject);
            //}
        }
        //if (other.gameObject.tag == owner.tag)
        //{
        //    if(other.gameObject != owner)
        //    {
        //        Destroy(gameObject);
        //    }
        //}

    }
}
