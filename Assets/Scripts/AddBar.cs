using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBar : MonoBehaviour
{
    public Material material;
    void Start()
    {

    }

    void Update()
    {

    }
    public void AmmoBar(GameObject player,int ammo)
    {
        GameObject bar = new GameObject();
        bar.AddComponent<SpriteRenderer>();
    }
}
