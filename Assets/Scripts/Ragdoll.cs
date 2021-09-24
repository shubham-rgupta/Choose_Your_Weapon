using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public static Ragdoll obj;
    
    public List<Collider> colliderParts = new List<Collider>();
    public List<Rigidbody> rigidBodies = new List<Rigidbody>();
    Animator anim;

    private List<Vector3> tempColliderVel = new List<Vector3>();
    private List<RigidbodyConstraints> tempRigid = new List<RigidbodyConstraints>();
    private void Awake()
    {
        obj = this;
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        foreach(Collider c in colliderParts)
        {
            
            tempColliderVel.Add (c.attachedRigidbody.velocity);
        }
        foreach(Rigidbody rb in rigidBodies)
        {
            tempRigid.Add(rb.constraints);
        }

        //GetComponent<CapsuleCollider>().enabled = false;
        //TurnOnRagdoll();


    }
    public void TurnOnRagdoll()
    {
        anim.enabled = false;
        foreach (Collider c in colliderParts)
        {
            c.enabled = true;
            c.isTrigger = false;
            c.attachedRigidbody.velocity = Vector3.zero;
        }

        foreach (Rigidbody rd in rigidBodies)
        {
            rd.isKinematic = false;
            rd.useGravity = true;
            rd.constraints = RigidbodyConstraints.None;
        }
    }

    //public void TurnOffRagdoll()
    //{
    //    anim.enabled = true;
    //    foreach (Collider c in colliderParts)
    //    {
    //        c.enabled = false;
    //        c.isTrigger = true;
            
    //    }
    //    foreach(Vector3 v in tempColliderVel)
    //    {
    //        c.attachedRigidbody.velocity = ;
    //    }

    //    foreach (Rigidbody rd in rigidBodies)
    //    {
    //        rd.isKinematic = true;
    //        rd.useGravity = false;
    //        //rd.constraints = RigidbodyConstraints.None;
    //    }
    //}
}
