using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponPoints : MonoBehaviour
{
    public static WeaponPoints obj;
    public Transform[] weapon_SpawnPoints;
    public GameObject[] weapon_Prefabs;
    public int points_Length, prefabs_Length;
    public Transform[] point = new Transform[2];

    float time = 0;
    bool give_Bonus;


    private void Awake()
    {
        obj = this;
    }
    void Start()
    {
        points_Length = weapon_SpawnPoints.Length;
        prefabs_Length = weapon_Prefabs.Length;
        StartCoroutine(SpawnWeaponPoints());
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 6)
        {
            give_Bonus = true;
            //time = 0;
        }
        //else
        //{
        //    give_Bonus = false;
        //}
        if (EnemyList.obj.enemy_Loose || PlayerList.obj.player_Loose)
        {
            StopCoroutine(SpawnWeaponPoints());
        }
    }

    public IEnumerator SpawnWeaponPoints()
    {
        yield return new WaitForSeconds(2.6f);
        if (!EnemyList.obj.enemy_Loose && !PlayerList.obj.player_Loose)
        {

            int a = Random.Range(0, prefabs_Length);
            int b = Random.Range(0, points_Length);
            int aa = Random.Range(0, prefabs_Length);
            int bb = Random.Range(0, points_Length);
            if (b == bb)
            {
                if (b >= prefabs_Length - 1)
                    b--;
                else
                    b++;
            }
            if (a == aa)
            {
                if (a >= prefabs_Length - 1)
                    a--;
                else
                    a++;
            }
            if (a == prefabs_Length - 1)
            {
                if (!give_Bonus)
                {
                    --a;
                }
            }
            if(aa == prefabs_Length - 1)
            {
                if (!give_Bonus)
                {
                    --aa;
                }
            }
            

            point[0] = weapon_SpawnPoints[b].transform;
            point[1] = weapon_SpawnPoints[bb].transform;

            GameObject obj1 = Instantiate(weapon_Prefabs[a], weapon_SpawnPoints[b].position, weapon_Prefabs[a].transform.rotation);
            GameObject obj2 = Instantiate(weapon_Prefabs[aa], weapon_SpawnPoints[bb].position, weapon_Prefabs[aa].transform.rotation);

            GameObject parent = new GameObject();
            obj1.transform.SetParent(parent.transform);
            obj2.transform.SetParent(parent.transform);

            Destroy(parent, 2.5f);
            StartCoroutine(SpawnWeaponPoints());
        }
    }
    void bonus()
    {
        //if(weapon_Prefabs.FirstOrDefault(x => x.tag == "Bonus"))
        //{

        //}
        //else
        //{

        //}
    }
}
