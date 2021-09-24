using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletRope : MonoBehaviour
{
    public Transform ropeStartTranform;
    public Transform ropeEndTransform;

    [SerializeField]Vector3 gravity;
    [SerializeField]float ropeSegmentLength = 0.25f;
    public int segmentsCount = 35;
    public float ropeRadius = 0.1f;
    [SerializeField]int constraintIterations = 35;
    [SerializeField]float friction = 0.1f;

    internal List<RopeSegment> ropeSegments = new List<RopeSegment>();

    void Start()
    {
        Vector3 ropeStartPoint = ropeStartTranform.position;

        for(int i=0;i<segmentsCount;++i){
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y-=ropeSegmentLength;
        }
    }

    void FixedUpdate()
    {
        Simulate();
    }

    void Simulate(){

        for(int i=0; i<segmentsCount;++i){
            RopeSegment currentSeg = ropeSegments[i];
            Vector3 velolcity = currentSeg.currentPos - currentSeg.oldPos;
            currentSeg.oldPos = currentSeg.currentPos;
            currentSeg.currentPos += velolcity;
            currentSeg.currentPos += gravity*Time.deltaTime;
            ropeSegments[i] = currentSeg;
        }

        //Constraints
        RopeStartEndPointConstraint();
        for(int i=0;i<constraintIterations;++i){
            StrechConstraint();
        }
        GroundandFrictionConstraint();
    }

    void RopeStartEndPointConstraint(){
        //1st constraint, start position is fixed at ropestartTransform;
        RopeSegment currentSegment = ropeSegments[0];
        currentSegment.currentPos = ropeStartTranform.position;
        ropeSegments[0] = currentSegment;
        currentSegment = ropeSegments[segmentsCount-1];
        currentSegment.currentPos = ropeEndTransform.position;
        ropeSegments[segmentsCount-1] = currentSegment;
    }

    void StrechConstraint(){
        //2nd constraint, distance between the segments should be constant;
        for(int i=0;i<segmentsCount-1;++i){
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i+1];

            float error = Vector3.Distance(firstSeg.currentPos,secondSeg.currentPos) - ropeSegmentLength;
            Vector3 changeDirection = (firstSeg.currentPos - secondSeg.currentPos).normalized;
            Vector3 change = error*changeDirection;

            if(i!=0){
                firstSeg.currentPos -=change*0.5f;
                ropeSegments[i] = firstSeg;
            }
            if((i+1)!=segmentsCount-1){
                secondSeg.currentPos += change*0.5f;
                ropeSegments[i+1] = secondSeg;
            }
        }
    }

    void GroundandFrictionConstraint(){
        //3rd constraint, should stay above the ground
        for(int i=0;i<segmentsCount;++i){
            RopeSegment currentSeg = ropeSegments[i];
            currentSeg.currentPos.y = Mathf.Clamp(currentSeg.currentPos.y,ropeRadius,Mathf.Infinity);
            if(currentSeg.currentPos.y == ropeRadius){
                //apply friction 
                Vector3 velolcity = currentSeg.currentPos - currentSeg.oldPos;
                currentSeg.currentPos -= velolcity*friction;
            }
            ropeSegments[i] = currentSeg;
        }
    }


    public struct RopeSegment{
        public Vector3 currentPos;
        public Vector3 oldPos;

        public static Vector3 GetNormal(Vector3 currentConnectionDirection){
            //get the rotation from initial connection direction to current connection direction in quaterion
            return Quaternion.FromToRotation(Vector3.down,currentConnectionDirection) * Vector3.right;
        }

        public RopeSegment(Vector3 pos){
            this.currentPos = pos;
            this.oldPos = pos;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(ropeStartTranform.position,0.05f);
        Gizmos.DrawSphere(ropeEndTransform.position,0.05f);
    }
}