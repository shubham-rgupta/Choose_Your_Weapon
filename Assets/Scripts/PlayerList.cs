using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    public static PlayerList obj;

    public List<GameObject> playerList = new List<GameObject>();
    GameObject p;
    public GameObject player_Prefab;
    public Transform cannon_MuzzlePoint;
    public bool player_Loose;
    public GameObject player_SpawnEffect;
    private void Awake()
    {
        obj = this;
    }
    public void Add_Player(GameObject player)
    {
        playerList.Add(player);
    }
    private void Start()
    {
        SpawnPlayer();
    }
    private void Update()
    {
        if (p.GetComponent<Player>().player_ChanceDone)
        {
            if (!player_Loose && !EnemyList.obj.enemy_Loose)
            {
                SpawnPlayer();
            }
        }

    }
    public void SpawnPlayer()
    {
        p = Instantiate(player_Prefab, cannon_MuzzlePoint.position, player_Prefab.transform.rotation, cannon_MuzzlePoint.transform);
        Add_Player(p);
        p.GetComponent<Player>().Idle();
    }
    public void KillPlayer()
    {
        player_Loose = true;
        foreach (GameObject player in playerList)
        {
            player.GetComponent<Player>().canShoot = false;
        }
    }
    public void PlayerFever(GameObject castle)
    {
        //int x = 15;
        foreach (GameObject player in playerList)
        {
            //LeanTween.move(player, new Vector3(x, player.transform.position.y, player.transform.position.z), 1f);
            //x -= 3;
            Player p = player.GetComponent<Player>();
            p.GetEnemy();
            if (p.enemy_Body == null)
            {
                p.enemy_Body = castle;
            }
        }
    }
    public void GiveUpgrade()
    {
        foreach (GameObject player in playerList)
        {
            Player p = player.GetComponent<Player>();
            p.Give_Rocket();
        }
    }
    public void RifleBonus(Vector3 pos)
    {
        int x = -3;
        for (int i = 0; i < 4; ++i)
        {  
            GameObject bonus_Player = Instantiate(player_Prefab, pos + new Vector3(x,0,0) , player_Prefab.transform.rotation);
            playerList.Add(bonus_Player);
            var pos2 = pos + new Vector3(x, 0, 0);

            var eff = Instantiate(player_SpawnEffect, pos2, Quaternion.identity, bonus_Player.transform);
            Destroy(eff, 1f);
            LeanTween.scale(bonus_Player, new Vector3(3, 3, 3), 0.1f);

            bonus_Player.GetComponent<Rigidbody>().useGravity = true;
            Player pl = bonus_Player.GetComponent<Player>();

            pl.isThrown = true;
            pl.player_ChanceDone = true;

            pl.GetEnemy();
            pl.have_AnyWeapon = true;
            pl.tried = true;
            pl.Give_Rifle();
            pl.RunningAnim();
            LeanTween.move(bonus_Player, new Vector3(bonus_Player.transform.position.x + Random.Range(-5,6), bonus_Player.transform.position.y, -2), 1f).setOnComplete(() =>
            {
                pl.canFollow = true;
                pl.Rifle_Anim();
            });
            x += 5;  
        }
        if (!player_Loose && !EnemyList.obj.enemy_Loose)
        {
            SpawnPlayer();
        }
        ProjectileThrow.obj.canClick = true;
        
    }
}
