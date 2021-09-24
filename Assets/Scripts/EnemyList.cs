using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyList : MonoBehaviour
{

    public static EnemyList obj;

    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject enemy_Prefab;
    public Transform cannon_MuzzlePoint;
    public bool enemy_Loose;
    GameObject e;
    Enemy en;

    private void Awake()
    {
        obj = this;
        SpawnEnemy();

    }
    private void Update()
    {
        if (en.isThrown)
        {
            if(!enemy_Loose && !PlayerList.obj.player_Loose)
            {
                SpawnEnemy();
            }
        }
    }
    public void SpawnEnemy()
    {
        e = Instantiate(enemy_Prefab, cannon_MuzzlePoint.position, cannon_MuzzlePoint.transform.rotation, cannon_MuzzlePoint.transform);
        en = e.GetComponent<Enemy>();
        en.Idle();
        enemyList.Add(e);
        EnemyThrow.obj.ThrowEnemy(e.gameObject);
    }
    public void KillEnemy()
    {
        enemy_Loose = true;
        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().canShoot = false;
        }
    }
    public void EnemyFever(GameObject castle)
    {
        foreach (GameObject enemy in enemyList)
        {
            Enemy enem = enemy.GetComponent<Enemy>();
            enem.GetPlayer();
            if (enem.player_Body == null)
            {
                enem.player_Body = castle;
            }
        }
    }
    public void GiveUpgradeEnemy()
    {
        foreach (GameObject enemy in enemyList)
        {
            Enemy enem = enemy.GetComponent<Enemy>();
            enem.Give_Rocket();
        }
    }
}
