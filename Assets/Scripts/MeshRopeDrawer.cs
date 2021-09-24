using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(VerletRope))]
[RequireComponent(typeof(CircularMeshExtruder))]

public class MeshRopeDrawer : MonoBehaviour
{
    VerletRope rope;
    CircularMeshExtruder meshExtruder;

    [SerializeField]int subdivisions = 6;

    void Start()
    {
        rope = GetComponent<VerletRope>();
        meshExtruder = GetComponent<CircularMeshExtruder>();
    }

    void Update()
    {
        DrawRope();
    }

    void DrawRope(){
        Vector3[] ropePositions = new Vector3[rope.segmentsCount];
        Vector3[] normals = new Vector3[rope.segmentsCount];
        rope.ropeSegments.Select(seg => seg.currentPos).ToArray();

        for(int i=0;i<rope.segmentsCount;i++){
            ropePositions[i] = rope.ropeSegments[i].currentPos;
            if(i==rope.segmentsCount-1){
                normals[i] = normals[i-1];
            }else{
                normals[i] = VerletRope.RopeSegment.GetNormal((rope.ropeSegments[i+1].currentPos - rope.ropeSegments[i].currentPos).normalized);
            }
        }

        meshExtruder.GenerateMesh(ropePositions,normals,rope.ropeRadius,subdivisions);
    }
}