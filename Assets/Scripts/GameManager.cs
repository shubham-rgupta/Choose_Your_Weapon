using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager obj;


    public GameObject enemy_Castle;
    public GameObject player_Castle;
    public GameObject player_Cannon;
    public GameObject player_Cannon_Break;
    public GameObject enemy_Cannon;
    public GameObject enemy_Cannon_Break;
    public GameObject cannon_Explosion_Effect;

    bool done = false;

    private void Awake()
    {
        obj = this;
        player_Cannon.SetActive(true);
        enemy_Cannon.SetActive(true);
        player_Cannon_Break.SetActive(false);
        enemy_Cannon_Break.SetActive(false);
    }

    private void Update()
    {
        if (EnemyList.obj.enemy_Loose)
        {
            if (!done)
            {
                enemy_Cannon.SetActive(false);
                enemy_Cannon_Break.SetActive(true);

                //var eff = Instantiate(cannon_Explosion_Effect, enemy_Cannon_Break.transform.position, Quaternion.identity);
                //Destroy(eff, 2f);

                //GameObject sphere = new GameObject();
                //sphere.transform.parent = enemy_Cannon_Break.transform;
                //sphere.transform.localPosition = Vector3.zero;
                //sphere.AddComponent<SphereCollider>().radius = 1.5f;
                //sphere.AddComponent<Rigidbody>().mass = 2.5f;
                //Destroy(sphere, 0.1f);
                done = true;
            }

        }
        if (PlayerList.obj.player_Loose)
        {
            if (!done)
            {
                player_Cannon.SetActive(false);
                player_Cannon_Break.SetActive(true);

                //var eff = Instantiate(cannon_Explosion_Effect, player_Cannon_Break.transform.position, Quaternion.identity);
                //Destroy(eff, 1f);

                //GameObject sphere = new GameObject();
                //sphere.transform.parent = player_Cannon_Break.transform;
                //sphere.transform.localPosition = Vector3.zero;
                //sphere.AddComponent<SphereCollider>().radius = 1.5f;
                //sphere.AddComponent<Rigidbody>().mass = 2.5f;
                //Destroy(sphere, 0.1f);
                done = true;
            }
        }

    }
}
