using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrow : MonoBehaviour
{
    public static ProjectileThrow obj;      //INSTANCE

   
    public GameObject player;
    public Rigidbody playerRB;

    public GameObject cannon;
    public GameObject cannonBase;
    public Transform cannonMuzzlePoint;
    public GameObject cannon_MuzzleEffect;
    public Transform LR_Point;


    public LayerMask Ground;
    public LineRenderer LineVisual;
    private int LineSegment = 10;
    private SpriteRenderer sr;

    private Vector3 prevMouseDownPos;
    private Vector3 mouseDeltaPos;
    public bool gotMouseDown;
    public Vector3 Voo;
    public float speed;
    public bool canClick;

    private Camera MainCamera;

    private void Awake()
    {
        LineVisual.positionCount = LineSegment;
        sr = GetComponent<SpriteRenderer>();
        obj = this;
    }
    void Start()
    {

        LineVisual.enabled = false;
        sr.enabled = false;
        canClick = true;
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -25, 25f), transform.position.y, Mathf.Clamp(transform.position.z, -15f, -1f));


        if (PlayerList.obj.playerList != null)                    //ONLY GETTING PLAYER AND STORING IN VARIABLE FROM PLAYER LIST
        {
            int len = PlayerList.obj.playerList.Count;
            for (int i = 0; i < len; ++i)
            {
                GameObject p = PlayerList.obj.playerList[i];
                if (p != null && !p.GetComponent<Player>().player_ChanceDone && !p.GetComponent<Player>().isThrown)
                {
                    player = p;
                    playerRB = player.GetComponent<Rigidbody>();
                }
                else
                {
                    player = null;
                    playerRB = null;
                }
            }
        }

        if (player != null && playerRB != null && canClick)         //CLICK AND DRAG TO AIM
        {
            if (Input.GetMouseButtonDown(0))
            {
                gotMouseDown = true;
                prevMouseDownPos = Input.mousePosition;
            }

            if (gotMouseDown && Input.GetMouseButton(0))
            {
                mouseDeltaPos = (Input.mousePosition - prevMouseDownPos) / Screen.width;
                prevMouseDownPos = Input.mousePosition;
                LaunchProjectile();
            }

            if (gotMouseDown && Input.GetMouseButtonUp(0))
            {
                LineVisual.enabled = false;
                sr.enabled = false;

                gotMouseDown = false;

                mouseDeltaPos = Vector3.zero;
                ThrowPlayer();
            }
        }
    }

    public void ThrowPlayer()
    {
        canClick = false;
        player.transform.parent = null;
        playerRB.useGravity = true;
        playerRB.velocity = Voo;            //ADDING VELOCITY IN RIGIDBODY
        Player pl = player.GetComponent<Player>();
        pl.ThrowAnim();                        //THROW ANIM
        
        GameObject eff = Instantiate(cannon_MuzzleEffect, cannonMuzzlePoint.position, cannon_MuzzleEffect.transform.rotation, cannonMuzzlePoint.transform);
        Destroy(eff, 3f);
        Vector3 scale = pl.initScale;
        LeanTween.scale(player, scale, 0.25f);
        pl.isThrown = true;            //PLAYER IS THROWN
        pl.player_ChanceDone = true;      
    }


    public void LaunchProjectile()
    {
        //ENABLE LINE RENDERER AND SPRITE RENDERER
        LineVisual.enabled = true;
        sr.enabled = true;

        //CHANGING POSITION OF FALLING POINT
        transform.position = new Vector3(transform.position.x + mouseDeltaPos.x * speed, transform.position.y, transform.position.z + mouseDeltaPos.y * speed);

        //GETTING THE VELOCITY
        Vector3 Vo = CalVelocity(transform.position, cannonMuzzlePoint.position, 0.4f);

        //VISUALISE LINE RENDERER
        Visualize(Vo);

        cannon.transform.rotation = Quaternion.Euler(transform.position.z * 2, transform.position.x * 2, 0f);
        cannonBase.transform.rotation = Quaternion.Euler(0f, transform.position.x * 2, 0f); 
        Voo = Vo;
    }

    Vector3 CalPosInTime(Vector3 Vo, float time)
    {
        Vector3 Vxz = Vo;
        Vxz.y = 0f;
        Vector3 result = cannonMuzzlePoint.position + Vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (Vo.y * time) + cannonMuzzlePoint.position.y;
        result.y = sY;
        return result;
    }
    void Visualize(Vector3 Vo)
    {
        for (int i = 1; i < LineSegment - 1; i++)
        {
            LineVisual.SetPosition(0, LR_Point.position);
            Vector3 pos = CalPosInTime(Vo, i / (float)LineSegment);
            LineVisual.SetPosition(i, pos);
            LineVisual.SetPosition(9, transform.position);
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

