using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -0.06f, 0.06f), Mathf.Clamp(transform.localPosition.y, -0.2f, 0.2f),
            (Mathf.Clamp(transform.localPosition.z, -0.7f, 0.3f)));
    }
}
