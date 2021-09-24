using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
