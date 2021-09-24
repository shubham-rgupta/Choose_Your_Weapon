using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMeter : MonoBehaviour
{
    public RectTransform ammo_Meter;
    public Camera mainCam;
    Transform playerTransform;
    void Start()
    {
        mainCam = Camera.main;
        playerTransform = Player.obj.transform;
    }
    void Update()
    {

        Vector3 worldDir = (transform.position - playerTransform.position).normalized;
        ammo_Meter.gameObject.SetActive(OutOfBounds(Camera.main.WorldToScreenPoint(transform.position)));
        Vector3 screenDir = GetDirectionVector(worldDir);
        Vector3 screenPos = GetScreenPos(screenDir);

        ammo_Meter.transform.position = screenPos;
        ammo_Meter.transform.up = screenDir;
    }

    Vector3 ClampPos(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        return pos;
    }

    bool OutOfBounds(Vector3 pos)
    {
        return !(pos.x > 0 && pos.x < Screen.width && pos.y > 0 && pos.y < Screen.height);
    }

    Vector3 GetDirectionVector(Vector3 worldDir)
    {
        return Vector3.ProjectOnPlane(worldDir, Camera.main.transform.forward);
    }

    Vector3 GetScreenPos(Vector3 screenDir)
    {
        //find angle
        Vector3 screenpos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        //marching towards the edge of the screen, until edge point is found

        while (!OutOfBounds(screenpos))
        {
            screenpos += screenDir;
        }

        return ClampPos(screenpos);
    }
}
