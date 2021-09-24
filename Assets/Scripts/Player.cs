using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player obj;


    public Animator p_Animator;
    public bool fall;
    public Transform rayCast_Point;
    public float groundRange;

    public bool player_ChanceDone;
    public Material gray;
    public bool canFollow;
    public bool canShoot;
    public int health;

    public bool isDead;
    public float canFollowTime;


    public Vector3 lookDirection;

    //shoot delay
    public float give_GunTime;
    private float shoot_Time = 0;

    public float melee_ShootDelay;
    public float rifle_ShootDelay;
    public float crossbow_ShootDelay;
    public float rocket_ShootDelay;

    //GROUND CHECK
    public float isGroudedMinDistanceStanding;
    public float isGroudedMinDistanceFloating;
    public GameObject groundFall_Effect;
    public LayerMask groundLayer;


    public GameObject weapon_Point;
    //bomb and chest
    public GameObject bomb_Prefab;
    public Transform bomb_point;
    public bool haveBomb;
    //melee
    public GameObject axe;
    //public GameObject sword;
    //rifle
    public GameObject rifle;
    public GameObject rifle_Bullet;
    public GameObject rifle_MuzzleEffect;
    public GameObject rifle_ShootingPoint;
    //crossbow
    public GameObject crossbow;
    public GameObject arrow;
    public GameObject arrowEffect;
    public GameObject arrow_ShootingPoint;
    //rocket Launcher

    public GameObject rocket;
    public GameObject rocket_Bullet;
    public GameObject rocket_MuzzleEffect;
    public GameObject rocket_ShootingPoint;

    //weapons bools
    public bool have_AnyWeapon;
    public bool isAxe;
    //public bool isSword;
    public bool isRifle;
    public bool isCrossbow;
    public bool isRocket;
    public bool isGrounded;
    public bool isThrown;
    public bool tried;

    //scale
    public Vector3 initScale;

    //Effects
    public Transform evolve_EffectPoint;
    public GameObject evolve_Effect;

    public GameObject enemy_Body;

    private void Awake()
    {
        obj = this;
        GetComponent<Rigidbody>().useGravity = false;
        initScale = transform.localScale;
        DisableWeapons();
    }

    void Start()
    {

        health = 100;
        transform.localScale = Vector3.zero;
    }


    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -22f, 22f), transform.position.y, Mathf.Clamp(transform.position.z, -22f, -1.4f));
        if(transform.position.y < -2)
        {
            health = 0;
        }

        //if (EnemyList.obj.enemy_Loose)
        //{
        //    GetEnemy();
        //}
        //CHECK FOR GROUND
        GroundCheck();

        if (health <= 0)
        {
            if (!isDead)
            {
                FeverMode.fMode.score -= 0.5f;
                FeverMode.fMode.enemy_Score += 1;
                print("Player Died");
            }
            isDead = true;
            canFollow = false;
            canShoot = false;
            DeadAnim();
        }


        //------------------------------------------------------------------SHOOTING WEAPONS--START-------------------------------------------------------------------//


        shoot_Time += Time.deltaTime;

        if (canShoot)
        {
            if(axe != null)
            {
                if (axe.activeSelf)
                {
                    if (shoot_Time >= melee_ShootDelay)
                    {
                        shoot_Time = 0;
                        Shoot_Axe();
                    }
                }
            }           
            //if (sword.activeSelf)
            //{
            //    if (shoot_Time >= melee_ShootDelay)
            //    {
            //        shoot_Time = 0;
            //        Shoot_Sword();
            //    }
            //}
            if (rifle.activeSelf)
            {
                if (shoot_Time >= rifle_ShootDelay)
                {
                    shoot_Time = 0;
                    Shoot_Rifle();
                }
            }
            if (crossbow.activeSelf)
            {
                if (shoot_Time >= crossbow_ShootDelay)
                {
                    shoot_Time = 0;
                    Shoot_CrossBow();
                }
            }
            if (rocket.activeSelf)
            {
                if (shoot_Time >= rocket_ShootDelay)
                {
                    shoot_Time = 0;
                    Shoot_Rocket();
                }
            }
        }

        //-----------------------------------------------------------SHOOTING WEAPONS--END---------------------------------------------------------------//



        //-----------------------------------------------------------ATTACK ON ENEMY---------------------------------------------------------------//

        if (enemy_Body != null)
        {
            lookDirection = (enemy_Body.transform.position - transform.position).normalized;
        }
        else
        {
            GetEnemy();
        }


        if (canFollow && have_AnyWeapon && !isDead && enemy_Body != null)
        {
            Vector3 look = lookDirection;
            look.y = 0;
            transform.rotation = Quaternion.LookRotation(look);
            if (enemy_Body.tag == "Castle")
            {
                canShoot = true;
            }
            else
            {
                if (enemy_Body.GetComponent<Enemy>().canFollow && !enemy_Body.GetComponent<Enemy>().haveBomb)
                {
                    canShoot = true;
                }
                else
                {
                    canShoot = false;
                }
            }

        }
        else
        {
            canShoot = false;
        }
    }

    public void DisableWeapons()
    {
        isAxe = false;
        isRifle = false;
        isCrossbow = false;
        for (int p = 0; p < weapon_Point.transform.childCount; p++)
        {
            weapon_Point.transform.GetChild(p).transform.gameObject.SetActive(false);
        }
    }
    public void GetEnemy()
    {
        if (EnemyList.obj.enemyList != null)
        {
            float minDis = Mathf.Infinity;
            foreach (GameObject enemy in EnemyList.obj.enemyList)
            {
                if (enemy.GetComponent<Enemy>().isThrown)
                {
                    float dist = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dist < minDis)
                    {
                        minDis = dist;
                        enemy_Body = enemy;
                    }
                }
            }
        }
    }

    //-----------------------------------------------------------GROUND CHECK--START---------------------------------------------------------------//

    public void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(rayCast_Point.position, -Vector3.up * groundRange, Color.black);
        if (Physics.Raycast(rayCast_Point.position, -Vector3.up, out hit, groundRange, groundLayer))
        {
            //hitDebug.transform.position = hit.point;

            if (!isThrown)
            {
                if (Vector3.Distance(rayCast_Point.position, hit.point) < isGroudedMinDistanceStanding)
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
            else
            {
                if (Vector3.Distance(rayCast_Point.position, hit.point) < isGroudedMinDistanceFloating)
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
    }
    //-----------------------------------------------------------GROUND CHECK--END---------------------------------------------------------------//




    //-----------------------------------------------------------------WEAPONS ACTIVE--START--------------------------------------------------------------------------//


    void Give_Axe()
    {
        axe.SetActive(true);
        isAxe = true;
    }
    
    public void Give_Rifle()
    {
        rifle.SetActive(true);
        isRifle = true;
        Rifle_Anim();
    }
    void Give_Crossbow()
    {
        crossbow.SetActive(true);
        isCrossbow = true;
        Rifle_Anim();

    }
    public void Give_Rocket()
    {
        DisableWeapons();
        have_AnyWeapon = true;
        //EVOLVE EFFECT
        GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
        Destroy(effect, 2f);
        LeanTween.delayedCall(0.2f, () =>
        {
            rocket.SetActive(true);
            isRocket = true;
            Rocket_Anim();
        });       
    }



    //--------------------------------------------------------------------------WEAPONS ACTIVE--END-------------------------------------------------------------------------------//


    //--------------------------------------------------------------------------WEAPONS SHOOT--START-----------------------------------------------------------------------------//

    public void Shoot_Axe()
    {
        Melee_Anim();
    }
    public void Shoot_Sword()
    {
        Melee_Anim();
    }
    public void Shoot_Rifle()
    {
        Rifle_Anim();
        GameObject effect = Instantiate(rifle_MuzzleEffect, rifle_ShootingPoint.transform.position, rifle_ShootingPoint.transform.rotation);
        Destroy(effect, 1f);
        GameObject bullet = Instantiate(rifle_Bullet, rifle_ShootingPoint.transform.position, rifle_ShootingPoint.transform.rotation);
        bullet.GetComponent<RifleBullet>().SetTarget(enemy_Body);
        bullet.GetComponent<RifleBullet>().owner = this.gameObject;
    }
    public void Shoot_CrossBow()
    {
        Rifle_Anim();
        GameObject effect = Instantiate(arrowEffect, arrow_ShootingPoint.transform.position, arrow_ShootingPoint.transform.rotation);
        Destroy(effect, 1f);
        GameObject arr = Instantiate(arrow, arrow_ShootingPoint.transform.position, arrow_ShootingPoint.transform.rotation);
        arr.GetComponent<Arrow>().SetTarget(enemy_Body);
        arr.GetComponent<Arrow>().owner = this.gameObject;
    }
    public void Shoot_Rocket()
    {
        Rocket_Anim();
        GameObject effect = Instantiate(rocket_MuzzleEffect, rocket_ShootingPoint.transform.position, rocket_ShootingPoint.transform.rotation);
        Destroy(effect, 1f);
        GameObject rkt = Instantiate(rocket_Bullet, rocket_ShootingPoint.transform.position, rocket_ShootingPoint.transform.rotation);
        rkt.GetComponent<BazookaBullet>().SetTarget(enemy_Body);
        rkt.GetComponent<BazookaBullet>().owner = this.gameObject;
    }
    public void ThrowWeapon()
    {
        if (axe.activeSelf)
        {
            axe.GetComponent<Melee>().SetTarget(enemy_Body);
            axe.GetComponent<Melee>().owner = this.gameObject;
            canShoot = false;
            have_AnyWeapon = false;

        }
        //else
        //{
        //    sword.GetComponent<Melee>().SetTarget(enemy_Body);
        //    sword.GetComponent<Melee>().owner = this.gameObject;
        //    canShoot = false;
        //    have_AnyWeapon = false;
        //}
    }

    //--------------------------------------------------------------------------WEAPONS SHOOT--END-----------------------------------------------------------------------------//



    //--------------------------------------------------------------------------ANIMATIONS--START-------------------------------------------------------------------------------//

    public void DisableAnim()
    {
        foreach (AnimatorControllerParameter parameter in p_Animator.parameters)
        {
            p_Animator.SetBool(parameter.name, false);
        }
    }
    public void RunningAnim()
    {
        DisableAnim();
        p_Animator.SetBool("Run", true);
    }
    public void Idle()
    {
        DisableAnim();
        p_Animator.SetBool("Idle", true);
    }
    public void DragAnim()
    {
        DisableAnim();
        p_Animator.SetBool("Drag", true);
    }
    public void ThrowAnim()
    {
        DisableAnim();
        p_Animator.SetBool("Throw", true);
    }
    public void FallOnGroundAnim()
    {
        GetEnemy();
        ProjectileThrow.obj.canClick = true;
        DisableAnim();
        p_Animator.SetBool("Fall", true);
        LeanTween.value(gameObject, GetComponent<Rigidbody>().velocity, Vector3.zero, 0.1f).setEaseOutSine().setOnUpdate((Vector3 val) =>
        {
            GetComponent<Rigidbody>().velocity = val;
        });
        GameObject effect = Instantiate(groundFall_Effect, rayCast_Point.transform.position, rayCast_Point.rotation);
        Destroy(effect, 1f);
        StartCoroutine(StandUp());
    }
    public IEnumerator StandUp()
    {    
        yield return new WaitForSecondsRealtime(0.3f);
        tried = true;
        p_Animator.SetBool("Fall", false);
        p_Animator.SetBool("Stand", true);
        yield return new WaitForSeconds(0.7f);
        RunToMid();
    }
    public void BombAnim()
    {
        DisableAnim();
        p_Animator.SetBool("Have_Bomb", true);
    }
    public void DeadAnim()
    {
        //LeanTween.value(gameObject, GetComponent<Rigidbody>().velocity, Vector3.zero, 0.5f).setEaseOutSine().setOnUpdate((Vector3 val) =>
        //{
        //    GetComponent<Rigidbody>().velocity = val;
        //});
        DisableAnim();
        p_Animator.SetBool("Dead", true);
        GetComponentInChildren<SkinnedMeshRenderer>().material = gray;
        GetComponent<CapsuleCollider>().height = 0.01f;
        GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.01f, 0f);

        PlayerList.obj.playerList.Remove(this.gameObject);
        Destroy(gameObject, 2f);
        player_ChanceDone = true;
    }
    public void Melee_Anim()
    {
        DisableAnim();
        p_Animator.SetBool("Melee_Attack", true);
        canShoot = false;
    }
    public void Rifle_Anim()
    {
        DisableAnim();
        p_Animator.SetBool("Rifle_Shoot", true);
    }
    public void Rocket_Anim()
    {
        DisableAnim();
        p_Animator.SetBool("Rocket_Shoot", true);
    }

    //---------------------------------------------------------------------------ANIMATIONS--END---------------------------------------------------------------------------------//

    public void RunToMid()
    {
        if (!haveBomb)
        {
            if (transform.position.z < -4f)
            {
                RunningAnim();
                LeanTween.move(this.gameObject, new Vector3(transform.position.x, transform.position.y, -2), 1f).setOnComplete(() =>
                {
                    if (!have_AnyWeapon)
                    {
                        Idle();
                    }
                    else
                    {
                        if (isRifle || isCrossbow)
                            Rifle_Anim();
                    }
                    canFollow = true;
                });
            }
        }        
    }

    //-----------------------------------------------------------------CHECKING FOR COLLISION TO GIVE WEAPONS--START-------------------------------------------------------------//

    private void OnTriggerEnter(Collider other)
    {
        if (!have_AnyWeapon && !tried)
        {
            if (other.gameObject.tag == "Axe")
            {
                have_AnyWeapon = true;
                Destroy(other.transform.parent.gameObject, 0.1f);

                //EVOLVE EFFECT
                LeanTween.delayedCall(give_GunTime, () =>
                {
                    GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
                    Destroy(effect, 2f);
                });
                LeanTween.delayedCall(give_GunTime + 0.2f, () =>
                {
                    Give_Axe();
                });
            }
            if (other.gameObject.tag == "Bonus")
            {
                have_AnyWeapon = true;
                Destroy(other.transform.parent.gameObject, 0.1f);
                //EVOLVE EFFECT
                LeanTween.delayedCall(give_GunTime, () =>
                {
                    GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
                    Destroy(effect, 2f);
                    PlayerList.obj.RifleBonus(transform.position);
                });
                LeanTween.delayedCall(give_GunTime + 0.2f, () =>
                {
                    Give_Rifle();
                });
            }
            if (other.gameObject.tag == "Rifle")
            {
                have_AnyWeapon = true;
                Destroy(other.transform.parent.gameObject, 0.1f);

                //EVOLVE EFFECT
                LeanTween.delayedCall(give_GunTime, () =>
                {
                    GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
                    Destroy(effect, 2f);
                });
                LeanTween.delayedCall(give_GunTime + 0.2f, () =>
                {
                    Give_Rifle();
                });
            }
            if (other.gameObject.tag == "CrossBow")
            {
                have_AnyWeapon = true;
                Destroy(other.transform.parent.gameObject, 0.1f);

                //EVOLVE EFFECT
                LeanTween.delayedCall(give_GunTime, () =>
                {
                    GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
                    Destroy(effect, 2f);
                });
                LeanTween.delayedCall(give_GunTime + 0.2f, () =>
                {
                    Give_Crossbow();
                });
            }
            //if (other.gameObject.tag == "RocketLauncher")
            //{
            //    have_AnyWeapon = true;
            //    Destroy(other.transform.parent.gameObject, 0.1f);

            //    //EVOLVE EFFECT
            //    LeanTween.delayedCall(give_GunTime, () =>
            //    {
            //        GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
            //        Destroy(effect, 2f);
            //    });
            //    LeanTween.delayedCall(give_GunTime + 0.2f, () =>
            //    {
            //        Give_Rocket();
            //    });
            //}
            if (other.gameObject.tag == "Bomb")
            {
                haveBomb = true;
                canFollow = false;
                Destroy(other.transform.parent.gameObject, 0.1f);
                //Destroy Through Bomb
                LeanTween.delayedCall(give_GunTime, () =>
                {
                    
                    GameObject effect = Instantiate(evolve_Effect, evolve_EffectPoint.position, evolve_Effect.transform.rotation, evolve_EffectPoint.transform);
                    Destroy(effect, 2f);
                });
                LeanTween.delayedCall(give_GunTime + 0.2f, () =>
                {
                    BombAnim();
                    GameObject bomb = Instantiate(bomb_Prefab, bomb_point.position, Quaternion.identity, bomb_point);
                    bomb.GetComponent<Bomb>().SetTarget(this.gameObject);
                    int[] y = { 90, -90 };
                    transform.rotation = Quaternion.Euler(0, y[Random.Range(0, 2)], 0);
                });
            }

        }
    }

    public void BombThrow()
    {
        //GetComponent<Rigidbody>().AddForce(Vector3.back * 5f, ForceMode.Impulse);
        GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-3, 3), 5, -15);
        health = 0;
    }
    //-----------------------------------------------------------------CHECKING FOR COLLISION TO GIVE WEAPONS--END-------------------------------------------------------------//
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "E_Axe" )//|| other.gameObject.tag == "E_Sword"
        {
            health -= 50;
        }

    }

}
