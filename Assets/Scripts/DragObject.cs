using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragObject : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;




    public Vector3 mouse_DownPos;
    public Vector3 mouse_UpPos;
    public bool is_Shoot;
    public float force;
    public float y_Power;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }
    private void OnMouseDown()
    {
        mouse_DownPos = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        mouse_UpPos = Input.mousePosition;
        ThrowBody(mouse_DownPos-mouse_UpPos);
    }
    void ThrowBody(Vector3 dir)
    {
        if (is_Shoot) return;
        rb.AddForce(new Vector3(dir.x,y_Power, dir.y) * force);
        //is_Shoot = true;
    }

    //private void Update()
    //{
    //    //if (Input.GetKey(KeyCode.S))
    //    //    rb.AddForce(new Vector3(0f, 0f, Input.GetAxis("Vertical") * force * Time.deltaTime), ForceMode.VelocityChange);
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit hit;
    //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            Transform objectHit = hit.transform;
    //        }
    //    }






    //}



    //void OnMouseDown()

    //{

    //    mZCoord = Camera.main.WorldToScreenPoint(

    //        gameObject.transform.position).z;



    //    // Store offset = gameobject world pos - mouse world pos

    //    mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    //}



    //private Vector3 GetMouseAsWorldPoint()

    //{

    //    // Pixel coordinates of mouse (x,y)

    //    Vector3 mousePoint = Input.mousePosition;



    //    // z coordinate of game object on screen

    //    mousePoint.z = mZCoord;



    //    // Convert it to world points

    //    return Camera.main.ScreenToWorldPoint(mousePoint);

    //}



    //void OnMouseDrag()
    //{
    //    //transform.position = GetMouseAsWorldPoint() + mOffset;
    //}
}
