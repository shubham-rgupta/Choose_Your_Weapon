using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverMode : MonoBehaviour
{
    public static FeverMode fMode;

    private Camera cam;
    public Transform camPoint;
    public Image player_FeverBar;
    public float score = 0f;
    public GameObject offFire, onFire;
    public Image enemy_FeverBar;
    public float enemy_Score = 0f;
    public GameObject e_offFire, e_onFire;
    public bool player_Fever, enemy_Fever;
    public GameObject p_Flag, e_Flag;


    private GameObject p_Castle, e_Castle;
    public bool p_CastleBroke, e_Castle_Broke, broke;

    private bool d1, d3;
    private void Awake()
    {
        //LeanTween.init = 2000f;
        fMode = this;
        onFire.SetActive(false);
        e_onFire.SetActive(false);
        offFire.SetActive(true);
        e_offFire.SetActive(true);
    }
    void Start()
    {
        p_Castle = GameManager.obj.player_Castle;
        e_Castle = GameManager.obj.enemy_Castle;
        cam = Camera.main;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    player_Fever = true;
        //}
        Mathf.Clamp(score, 0, 1);
        player_FeverBar.fillAmount = score * 0.20f;

        Mathf.Clamp(enemy_Score, 0, 1);
        enemy_FeverBar.fillAmount = enemy_Score * 0.20f;

        if (player_FeverBar.fillAmount >= 1f)
        {
            player_Fever = true;
        }
        if (enemy_FeverBar.fillAmount >= 1)
        {
            enemy_Fever = true;
        }

        if (player_Fever)
        {
            ProjectileThrow.obj.canClick = false;
            onFire.SetActive(true);
            offFire.SetActive(false);
            EnemyList.obj.KillEnemy();
            PlayerList.obj.PlayerFever(e_Castle);
            if (!d1)
            { 
                PlayerList.obj.GiveUpgrade();  
                d1 = true;
            }
            if (e_Castle_Broke)
            {
                e_Flag.GetComponent<Rigidbody>().useGravity = true;
                e_Flag.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                e_Flag.GetComponent<Animation>().enabled = false;
                PlayerList.obj.KillPlayer();
            }
        }
        //if (onFire.activeSelf)
        //{
        //    LeanTween.scale(onFire, Vector3.one * 1.1f, 0.2f);
        //    LeanTween.delayedCall(0.2f, () => LeanTween.scale(onFire, Vector3.one, 0.1f));
        //}
        if (enemy_Fever)
        {
            ProjectileThrow.obj.canClick = false;
            e_onFire.SetActive(true);
            e_offFire.SetActive(false);

            PlayerList.obj.KillPlayer();

            EnemyList.obj.EnemyFever(p_Castle);
            if (!d3)
            {
                LeanTween.move(cam.transform.gameObject, camPoint.position, 1.5f).setIgnoreTimeScale(true);
                LeanTween.rotate(cam.transform.gameObject, camPoint.rotation.eulerAngles, 1.5f).setIgnoreTimeScale(true);
                EnemyList.obj.GiveUpgradeEnemy();
                d3 = true;
            }
            if (p_CastleBroke)
            {
                p_Flag.GetComponent<Rigidbody>().useGravity = true;
                p_Flag.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                p_Flag.GetComponent<Animation>().enabled = false;
                EnemyList.obj.KillEnemy();
            }
        }
        //if (e_onFire.activeSelf)
        //{
        //    LeanTween.scale(e_onFire, Vector3.one * 1.1f, 0.2f);
        //    LeanTween.delayedCall(0.2f, () => LeanTween.scale(e_onFire, Vector3.one, 0.1f));
        //}
        if (broke)
        {
            if (player_Fever)
                e_Castle_Broke = true;
            if (enemy_Fever)
                p_CastleBroke = true;
        }

    }
    public IEnumerator FOff()
    {
        if (!broke)
        {
            yield return new WaitForSeconds(1f);
            broke = true;
        }
        else
        {
            yield return null;
        }
    }
    public void FeverOff()
    {
        StartCoroutine(FOff());
    }
}
