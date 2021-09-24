using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBullet : MonoBehaviour
{
    public static BazookaBullet obj;
    public float speed;
    public GameObject target;
    public GameObject explosion_Effect;
    public GameObject fire_Effect;
    public GameObject fire_Point;
    public GameObject owner;
    private void Awake()
    {
        obj = this;
    }
    private void Start()
    {
        StartCoroutine(Fire());
        Destroy(gameObject, 2f);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        if(target.tag != "Enemy" || target.tag != "Player")
        {
            StartCoroutine(GenerateSphere());
        }
    }

    void Update()
    {
        if (target.tag == "Enemy" && target.GetComponent<Enemy>().isDead)
        {
            Destroy(gameObject);
        }
        if (target.tag == "Player" && target.GetComponent<Player>().isDead)
        {
            Destroy(gameObject);
        }
        else
        {
            var dir2 = (target.transform.position - transform.position).normalized;
            dir2.y = 0.0f;
            transform.position += speed * Time.deltaTime * dir2;

            //Vector3 look = (target.transform.position);
            //look.y = dir2.y;
            //transform.LookAt(look);
        }
        
    }
    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag != owner.tag)
        {
            if (other.gameObject.tag == "Enemy")
            {
                GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity, target.transform);
                Destroy(eff, 2f);
                other.gameObject.GetComponent<Enemy>().health =0;
                Destroy(gameObject);
            }
            if (other.gameObject.tag == "Player")
            {
                GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity, target.transform);
                Destroy(eff, 2f);
                other.gameObject.GetComponent<Player>().health =0;
                Destroy(gameObject);
            }
            if (other.gameObject.tag == "Castle")
            {
                GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity);
                Destroy(eff, 1f);
                FeverMode.fMode.FeverOff();
                Destroy(gameObject, 0.2f);
            }

        }
        if (other.gameObject.tag == owner.tag)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Stopper")
        {
            GameObject eff = Instantiate(explosion_Effect, transform.position, Quaternion.identity);
            Destroy(eff, 1f);
            Destroy(gameObject);
        }
    }
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(fire_Effect, fire_Point.transform.position, fire_Point.transform.rotation, fire_Point.transform);
    }
    public IEnumerator GenerateSphere()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject sphere = new GameObject();
        sphere.transform.parent = this.transform;
        sphere.transform.localPosition = Vector3.zero;
        sphere.AddComponent<SphereCollider>().radius = 1.5f;
        sphere.AddComponent<Rigidbody>().mass = 2.5f;
        Destroy(sphere, 1f);
    }
}
