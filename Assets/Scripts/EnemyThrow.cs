using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrow : MonoBehaviour
{
    public static EnemyThrow obj;

    public Rigidbody enemyRB;
    public GameObject enemy_Body;
    public LayerMask Ground;

    public GameObject cannon;
    public GameObject cannon_Base;
    public Transform cannon_MuzzlePoint;
    public GameObject cannon_MuzzleEffect;

    public GameObject enemy_WeaponSpawner;
    public Vector3 Voo;
    public float speed;
    public bool hasThrown;

    private Camera MainCamera;
    public Transform[] point = new Transform[2];

    private void Awake()
    {
        obj = this;
    }
    void Start()
    {
    }

    public void ThrowEnemy(GameObject enemy)
    {
        StartCoroutine(ThrowEnemyCoroutine(enemy));
    }

    IEnumerator ThrowEnemyCoroutine(GameObject enemy)
    {
        while (enemy_WeaponSpawner.GetComponent<WeaponPoints>().point == null)
            yield return null;

        enemy_Body = enemy;
        enemyRB = enemy_Body.GetComponent<Rigidbody>();
        while (!PointToThrow())
        {
            yield return null;
        }
        CannonLook();
    }

    public void ThrowEnemy()
    {
        Enemy en = enemy_Body.GetComponent<Enemy>();

        enemy_Body.transform.parent = null;
        enemyRB.useGravity = true;
        enemyRB.velocity = Voo;
        GameObject eff = Instantiate(cannon_MuzzleEffect, cannon_MuzzlePoint.position, cannon_MuzzleEffect.transform.rotation, cannon_MuzzlePoint.transform);
        Destroy(eff, 3f);
        Vector3 scale = en.initScale;
        LeanTween.scale(enemy_Body, scale, 0.2f);
        LeanTween.rotate(enemy_Body, new Vector3(0, 180, 0), 0.5f);
        en.ThrowAnim();
        en.isThrown = true;
    }

    public void CannonLook()
    {
        //cannon_Base.transform.rotation = Quaternion.Euler(0f, -(transform.position.x + 180), 0f);
        Vector3 look = transform.position;
        //look.y = 0;
        //cannon.transform.LookAt(look);
        //LeanTween.rotate(cannon, -look, 0.25f);
        LeanTween.rotateY(cannon_Base, -look.x + 180, 0.25f);
        LeanTween.rotateX(cannon, -look.z, 0.25f);
        LeanTween.delayedCall(0.3f, () =>
        {
            LaunchProjectile();
        });

    }
    public void LaunchProjectile()
    {
        Vector3 Vo = CalVelocity(transform.position, cannon_MuzzlePoint.position, 0.4f);
        Voo = Vo;
        ThrowEnemy();
    }
    public bool PointToThrow()
    {
        point = enemy_WeaponSpawner.GetComponent<WeaponPoints>().point;
        //if (point == null) print("EMPTY POINT");
        int n = Random.Range(0, 2);
        if (point[n] == null)
        {
            return false;
        }
        if (transform.position != point[n].position)
        {
            transform.position = point[n].position;

            point[0] = null;
            point[1] = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 CalVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        //create a float that represent distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }
}
