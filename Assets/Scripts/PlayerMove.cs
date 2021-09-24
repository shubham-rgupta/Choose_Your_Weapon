//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMove : MonoBehaviour
//{

//    //movement
//    //public float h;
//    //public float v;
//    //public float multiplier = 0.1f;

//    public static PlayerMove obj;
//    public GameObject playerBody;
//    public GameObject parentObj;
//    public float moveSpeed;
//    public float camRotSpeed;
//    Rigidbody rb;
//    protected Joystick joystick;
//    private Vector3 moveDirection;
//    public GameObject baseCube;
//    public GameObject cubePrefab;
//    public List<GameObject> cubeList = new List<GameObject>();
//    public int listLength = 0;


//    public Animator PAnimator;
//    //Vector3 size;

//    //Spherecast
//    private Vector3 origin;
//    public float radius;
//    private Vector3 direction;
//    public float max_Distance;
//    public LayerMask botmask;

//    public GameObject effect_point;
//    public GameObject evolve_Effect;

//    //Weapons
//    float shoot_Time = 0;
//    //public float pistol_Shoot_Delay;
//    public float bazooka_Shoot_Delay;
//    public float rifle_Shoot_Delay;
//    public float tank_Shoot_Delay;
//    public GameObject enemy_Body;
//    public bool isShooting = false;
//    public GameObject weapon_Point;

//    //public GameObject pistol_ShootingPoint;
//    //public GameObject pistol;
//    public GameObject rifle;
//    public GameObject rifle_Bullet;
//    public GameObject rifle_Muzzle;
//    public GameObject rifle_ShootingPoint;

//    //same bullet of pistol

//    //public GameObject bazooka_ShootingPoint;
//    //same shooting point of pistol
//    public GameObject bazooka;
//    public GameObject bazooka_Bullet;
//    public GameObject bazooka_Muzzle;
//    public GameObject bazooka_ShootingPoint;


//    public GameObject tank;
//    public GameObject tank_Bullet;
//    public GameObject tank_Muzzle;
//    public GameObject tank_ShootingPoint;

//    //Particle effects for weapons




//    //bools for weapons
//    //private bool isPistol = false;
//    private bool isRifle = false;
//    private bool isBazooka = false;
//    private bool isTank = false;
//    public GameObject water_Effect;
//    public Camera cam;
//    public GameObject stackUp_Puff;
//    public Transform camTarget;
//    public float sp;
//    public Vector3 dir;
//    private Vector3 sizeOf_Tile;
//    int inc = 0, inc2 = 0;
//    public bool canInc = false;
//    public float rayRange;
//    public GameObject flag;
//    private void Awake()
//    {
//        //to disable all weapons at start
//        for (int p = 0; p < weapon_Point.transform.childCount; p++)
//        {
//            weapon_Point.transform.GetChild(p).transform.gameObject.SetActive(false);
//        }
//        //to enable body and disable tank at start
//        playerBody.SetActive(true);
//        tank.SetActive(false);
//        obj = this;
//    }

//    void Start()
//    {
//        sizeOf_Tile = new Vector3(1, 1.5f, 1);
//        parentObj.transform.localScale = sizeOf_Tile;
//        //cubePrefab.transform.localScale = new Vector3(1,1.5f,1);
//        sp = moveSpeed;
//        joystick = FindObjectOfType<DynamicJoystick>();
//        rb = GetComponent<Rigidbody>();
//        cubeList.Add(baseCube);
//        PAnimator.SetBool("Idle", true);
//        rifle.SetActive(false);
//        bazooka.SetActive(false);
//    }


//    void Update()
//    {
//        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -60, 60), Mathf.Clamp(transform.position.y, -50, 50),
//            (Mathf.Clamp(transform.position.z, -60, 60)));
//        //sizeOf_Tile = parentObj.transform.localScale;


//        if (water_Effect != null)
//        {
//            Vector3 p = water_Effect.transform.position;
//            water_Effect.transform.position = new Vector3(p.x, 0, p.z);
//        }


//        //Giving Weapons only
//        //if (listLength >= 4 && listLength <= 7 && !isPistol)
//        //{
//        //    Give_Pistol();
//        //}

//        if (listLength > 5 && listLength <= 15 && !isRifle)
//        {
//            //sizeOf_Tile = Vector3.one;

//            Give_Rifle();
//        }

//        if (listLength > 15 && listLength <= 25 && !isBazooka)
//        {
//            //sizeOf_Tile = Vector3.one * 1.5f;
//            IncreaseSizeOfParent();
//            Give_Bazooka();
//        }
//        if (listLength > 25 && !isTank)
//        {
//            //sizeOf_Tile = Vector3.one * 2f;
//            IncreaseSizeOfParent2();
//            Give_Tank();
//        }


//        //spherecast
//        origin = transform.position;
//        origin.y = 2f;
//        direction = transform.forward;
//        shoot_Time += Time.deltaTime;
//        RaycastHit hit;

//        //if (Physics.SphereCast(origin, radius, direction, out hit, max_Distance, botmask))
//        //{
//        //    enemy_Body = hit.collider.gameObject;
//        //    if (listLength >= hit.collider.gameObject.GetComponent<Bot1>().index && hit.collider.gameObject.GetComponent<Bot1>().index >= 0)
//        //    {
//        //        isShooting = true;
//        //        StartShooting(); //shooting will start
//        //    }
//        //}
//        if (enemy_Body != null)
//        {
//            float d = Vector3.Distance(enemy_Body.transform.position, transform.position);
//            if (d < rayRange)
//            {
//                Vector3 look = enemy_Body.transform.position;
//                look.y = transform.position.y;
//                transform.LookAt(look);
//            }
//            else
//            {
//                enemy_Body = null;
//            }

//        }
//        dir = transform.position;
//        dir.y = 0f;
//        if (Physics.Raycast(dir, transform.forward, out hit, rayRange, botmask) && listLength > 5)
//        {


//            if (listLength > hit.collider.gameObject.GetComponent<Bot1>().index && hit.collider.gameObject.GetComponent<Bot1>().index >= 0)
//            {
//                isShooting = true;
//                enemy_Body = hit.collider.gameObject;
//                StartShooting(); //shooting will start
//            }
//            else
//            {
//                enemy_Body = null;
//            }
//        }
//        else
//        {
//            isShooting = false;
//        }





//        if (TapStart.obj.startGame)
//        {

//            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
//            {
//                if (!isShooting)
//                {
//                    //AnimBool();
//                    //PAnimator.SetBool("PistolShoot", false);
//                    PAnimator.SetBool("RifleShoot", false);
//                    PAnimator.SetBool("BazookaShoot", false);
//                    PAnimator.SetBool("Idle", false);
//                    PAnimator.SetBool("Surfing", true);
//                }
//                moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

//                Vector3 camMoveDirection = transform.forward * joystick.Vertical;
//                rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * moveDirection);
//                //rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, joystick.Horizontal * Time.deltaTime *camRotSpeed,0f));
//                if (enemy_Body == null)
//                {
//                    Quaternion targetRotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * camRotSpeed);
//                    rb.MoveRotation(targetRotation);
//                }

//            }
//            if (joystick.Horizontal == 0 && joystick.Vertical == 0)
//            {
//                if (!isShooting)
//                {
//                    AnimBool();
//                    PAnimator.SetBool("Idle", true);
//                }
//            }
//        }
//    }
//    void IncreaseSizeOfParent()
//    {

//        if (inc == 0)
//        {
//            sizeOf_Tile += new Vector3(0.5f, 0.5f, 0.5f);
//            parentObj.transform.localScale = sizeOf_Tile;
//            transform.position += transform.up * (float)(listLength / 8f);
//            parentObj.transform.localPosition -= new Vector3(0, 0.17f, 0);
//            //cubePrefab.transform.localScale = sizeOf_Tile;
//            ++inc;
//        }
//    }
//    void IncreaseSizeOfParent2()
//    {

//        if (inc2 == 0)
//        {
//            sizeOf_Tile += new Vector3(0.5f, 0.5f, 0.5f);
//            parentObj.transform.localScale = sizeOf_Tile;
//            //for (int i = 0; i < parentObj.transform.childCount; ++i)
//            //{
//            //    parentObj.transform.GetChild(i).transform.localScale = sizeOf_Tile;
//            //}
//            transform.position += transform.up * (float)(listLength / 8.5f);
//            parentObj.transform.localPosition -= new Vector3(0, 0.35f, 0);
//            //cubePrefab.transform.localScale = sizeOf_Tile;
//            ++inc2;
//        }
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Colorless")
//        {
//            other.gameObject.tag = "Untagged";
//            //PAnimator.SetBool("PistolShoot", false);
//            PAnimator.SetBool("RifleShoot", false);
//            PAnimator.SetBool("BazookaShoot", false);
//            PAnimator.SetBool("Surfing", false);
//            PAnimator.SetBool("Jump", true);
//            StartCoroutine(jump());
//            Destroy(other.gameObject);
//            IncreaseTile();
//        }
//    }
//    public void IncreaseTile()
//    {
//        transform.position += transform.up * ((float)sizeOf_Tile.y / 4.285f);
//        camTarget.transform.position -= transform.up * ((float)sizeOf_Tile.y / 4.285f);

//        LeanTween.moveLocal(camTarget.gameObject, Vector3.zero, 1.5f);
//        GameObject n = Instantiate(stackUp_Puff, cubeList[listLength].transform.position, baseCube.transform.rotation, cubeList[listLength].transform);
//        Destroy(n, 1f);
//        GameObject newCube = Instantiate(cubePrefab, cubeList[listLength].transform.position + new Vector3(0, -((float)sizeOf_Tile.y / 4.285f), 0), baseCube.transform.rotation);
//        //water_Effect.transform.position -= transform.up * 0.35f;
//        cubeList.Add(newCube);
//        listLength++;
//        cubeList[listLength].transform.SetParent(parentObj.transform);
//        cubeList[listLength].transform.localScale = Vector3.one;
//        parentObj.transform.localScale = sizeOf_Tile;

//        LeanTween.scale(parentObj, sizeOf_Tile * 1.2f, 0.05f).setLoopCount(1);
//        LeanTween.delayedCall(0.05f, () => LeanTween.scale(parentObj, sizeOf_Tile, 0.1f).setLoopCount(1));
//        canInc = false;
//    }
//    private IEnumerator jump()
//    {
//        yield return new WaitForSeconds(0.2f);
//        PAnimator.SetBool("Jump", false);
//    }
//    public void MakeSlow(float time)
//    {
//        moveSpeed = sp / 2;
//        StartCoroutine(SlowSpeed(time));
//    }
//    public IEnumerator SlowSpeed(float t)
//    {
//        yield return new WaitForSecondsRealtime(t);
//        moveSpeed = sp;
//    }
//    public void playerDestroy()
//    {
//        if (listLength == 0)
//        {
//            GameManager.obj.GameOver();
//            Destroy(water_Effect.gameObject);
//            isShooting = true;
//            cubeList[listLength].transform.SetParent(null);
//            GameObject obj = cubeList[listLength].gameObject;
//            cubeList.RemoveAt(listLength);
//            Destroy(obj);
//            listLength--;
//            Destroy(gameObject, 3f);
//            AnimBool();
//            PAnimator.SetBool("Fall", true);
//            flag.SetActive(false);
//            moveSpeed = 0f;
//            rb.constraints = RigidbodyConstraints.None;
//            rb.useGravity = true;
//            //gameObject.GetComponent<CapsuleCollider>().enabled = false;

//        }
//        if (listLength > 0)
//        {
//            cubeList[listLength].transform.SetParent(null);
//            GameObject obj = cubeList[listLength].gameObject;
//            cubeList.RemoveAt(listLength);
//            Destroy(obj);
//            listLength--;
//            //foreach (GameObject go in cubeList)
//            //{
//            //    go.transform.position -= new Vector3(0, 0.35f, 0);
//            //}
//            transform.position -= Vector3.up * 0.35f;
//            //water_Effect.transform.position += transform.up * 0.35f;
//            //Camera.main.GetComponentInChildren<CameraFollow>().DecreseCamDistance();
//            //playerBody.transform.position -= Vector3.up * 0.35f;
//            //tank.transform.position -= Vector3.up * 0.35f;
//        }
//    }

//    //void PlayerFall()
//    //{
//    //    AnimBool();
//    //    PAnimator.SetBool("Fall", true);
//    //}

//    void StartShooting()
//    {
//        //check which weapon is active and shoot from that weapon

//        //if (pistol.activeInHierarchy)
//        //{
//        //    if (shoot_Time >= pistol_Shoot_Delay)
//        //    {
//        //        shoot_Time = 0;

//        //        Shoot_Pistol();
//        //    }

//        //}

//        if (rifle.activeInHierarchy)
//        {
//            if (shoot_Time >= rifle_Shoot_Delay)
//            {
//                shoot_Time = 0;
//                Shoot_Rifle();
//            }
//        }

//        if (bazooka.activeInHierarchy)
//        {
//            if (shoot_Time >= bazooka_Shoot_Delay)
//            {
//                shoot_Time = 0;
//                Shoot_Bazooka();
//            }
//        }
//        if (tank.activeInHierarchy)
//        {
//            if (shoot_Time >= tank_Shoot_Delay)
//            {
//                shoot_Time = 0;
//                Shoot_Tank();
//            }

//        }


//        //tank
//    }

//    //just giving different weapons
//    //void Give_Pistol()
//    //{
//    //    GameObject effect = Instantiate(evolve_Effect, effect_point.transform.position, Quaternion.identity, effect_point.transform);
//    //    pistol.SetActive(true);
//    //    StartCoroutine(EffectDestroyer(effect));
//    //    isPistol = true;
//    //}
//    void Give_Rifle()
//    {
//        GameObject effect = Instantiate(evolve_Effect, effect_point.transform.position, Quaternion.identity, effect_point.transform);
//        //pistol.SetActive(false);
//        rifle.SetActive(true);
//        StartCoroutine(EffectDestroyer(effect));
//        isRifle = true;
//    }
//    void Give_Bazooka()
//    {
//        rayRange = 14f;
//        GameObject effect = Instantiate(evolve_Effect, effect_point.transform.position, Quaternion.identity, effect_point.transform);
//        rifle.SetActive(false);
//        bazooka.SetActive(true);
//        StartCoroutine(EffectDestroyer(effect));
//        isBazooka = true;
//    }
//    void Give_Tank()
//    {
//        rayRange = 16f;
//        //GameObject eff = evolve_Effect;
//        //eff.transform.localScale += Vector3.one * 2;
//        GameObject effect = Instantiate(evolve_Effect, effect_point.transform.position, Quaternion.identity, effect_point.transform);
//        bazooka.SetActive(false);
//        playerBody.SetActive(false);
//        tank.SetActive(true);
//        StartCoroutine(EffectDestroyer(effect));
//        isTank = true;
//    }


//    //public void Shoot_Pistol()
//    //{

//    //    PAnimator.SetBool("PistolShoot", true);
//    //    GameObject effect = Instantiate(pistol_Muzzle, pistol_ShootingPoint.transform.position, pistol_ShootingPoint.transform.rotation);
//    //    StartCoroutine(EffectDestroyer(effect));
//    //    Instantiate(pistol_Bullet, pistol_ShootingPoint.transform.position, pistol_ShootingPoint.transform.rotation).GetComponent<PistolBullet>().SetTarget(enemy_Body);
//    //}
//    public void Shoot_Rifle()
//    {
//        AnimBool();
//        PAnimator.SetBool("RifleShoot", true);
//        GameObject effect = Instantiate(rifle_Muzzle, rifle_ShootingPoint.transform.position, rifle_ShootingPoint.transform.rotation);
//        StartCoroutine(EffectDestroyer(effect));
//        Instantiate(rifle_Bullet, rifle_ShootingPoint.transform.position, rifle_ShootingPoint.transform.rotation).GetComponent<PistolBullet>().SetTarget(enemy_Body);
//        canInc = true;
//    }
//    public void Shoot_Bazooka()
//    {
//        AnimBool();
//        PAnimator.SetBool("BazookaShoot", true);
//        GameObject effect = Instantiate(bazooka_Muzzle, bazooka_ShootingPoint.transform.position, bazooka_ShootingPoint.transform.rotation);
//        StartCoroutine(EffectDestroyer(effect));
//        Instantiate(bazooka_Bullet, bazooka_ShootingPoint.transform.position, bazooka_ShootingPoint.transform.rotation).GetComponent<BazookaBullet>().SetTarget(enemy_Body);
//        canInc = true;
//    }
//    public void Shoot_Tank()
//    {
//        GameObject effect = Instantiate(tank_Muzzle, tank_ShootingPoint.transform.position, tank_ShootingPoint.transform.rotation);
//        StartCoroutine(EffectDestroyer(effect));
//        Instantiate(tank_Bullet, tank_ShootingPoint.transform.position, tank_ShootingPoint.transform.rotation).GetComponent<TankBullet>().SetTarget(enemy_Body);
//        canInc = true;
//    }

//    IEnumerator EffectDestroyer(GameObject obj)
//    {
//        yield return new WaitForSeconds(0.5f);
//        Destroy(obj);
//    }

//    void AnimBool()
//    {
//        foreach (AnimatorControllerParameter parameter in PAnimator.parameters)
//        {
//            PAnimator.SetBool(parameter.name, false);
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.blue;
//        Gizmos.DrawLine(dir, dir + transform.forward * rayRange);
//        //Gizmos.DrawWireSphere(origin + direction * max_Distance, radius);
//    }
//}
