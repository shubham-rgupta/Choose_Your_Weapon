using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDebug : MonoBehaviour
{
    public Transform mid;

    void Update()
    {
        Debug.DrawLine(transform.position, mid.position, Color.red);
    }
}
