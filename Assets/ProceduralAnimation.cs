using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimation : MonoBehaviour
{
    public Transform[] legPositions; // Where the legs are detached to
    public Transform[] legNewTargets; // Point where the raycast starts from
    public Vector3[] newTargetsFromRay; // Point where the raycast ends, if distance to a leg is greater than 1 it moves it here
    public Vector3[] legTargetVectors;
    
    float bobbingAmplitude = 0.05f; // Amplitude of bobbing
    float bobbingFrequency = 7.5f; // Frequency of bobbing 
    float moveSpeed = 0.04f; // Movement speed

    void Start()
    {
        // Getting the children transforms
        Transform spider = transform.Find("Spider");
        Transform rigContainer = spider.Find("Rig 1");
        Transform newLegTargetContainer = spider.Find("New_Leg_Targets");

        // Initializing arrays for legs
        legPositions = new Transform[8];
        legTargetVectors = new Vector3[8];
        newTargetsFromRay = new Vector3[8];
        legNewTargets = new Transform[8];

        // Getting legPositions transforms
        for(int i = 0; i < 2; i++){
            for(int j = 0; j < 4; j++){
                string legName;
                string legTargetName;
                Transform legRig;
                Transform legRigTarget;
                switch(i){
                    case 0: 
                        legName = "L_Leg_"+(j+1);
                        legTargetName = "L_Leg_"+(j+1)+"_target";
                        legRig = rigContainer.Find(legName);
                        legRigTarget = legRig.Find(legTargetName);
                        legPositions[j] = legRigTarget;
                        break;
                    case 1: 
                        legName = "R_Leg_"+(j+1);
                        legTargetName = "R_Leg_"+(j+1)+"_target";
                        legRig = rigContainer.Find(legName);
                        legRigTarget = legRig.Find(legTargetName);
                        legPositions[j+4] = legRigTarget;
                        break;
                    default: break;
                }
            }
        }

        // Getting legNewTargets transforms
        for(int i = 0; i < 2; i++){
            for(int j = 0; j < 4; j++){
                string legName;
                switch(i){
                    case 0: 
                        legName = "Left_Leg_"+(j+1); 
                        legNewTargets[j] = newLegTargetContainer.Find(legName);
                        break;
                    case 1: 
                        legName = "Right_Leg_"+(j+1);
                        legNewTargets[j+4] = newLegTargetContainer.Find(legName); 
                        break;
                    default: break;
                }
            }
        }

        // Turning the newTarget raycast points off
        foreach(Transform newTarget in legNewTargets){
            newTarget.gameObject.SetActive(false);
        }

        for(int i = 0; i < legTargetVectors.Length; i++){
            legTargetVectors[i] = legPositions[i].position;
        }
        
    }

    void Update()
    {
        // Making the spider bobbing
        float desiredHeight = 0.0f;

        for(int i = 0; i < legPositions.Length; i++){
            desiredHeight += legPositions[i].position.y;
        }
        desiredHeight /= 8;

        Vector3 bodyPosition = transform.position;
        bodyPosition.y = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude + desiredHeight; // Bobbing motion
        transform.position = bodyPosition;

        // Setting basic speed to see the legs move automatically
        transform.position += transform.forward * moveSpeed;

        for(int i = 0; i < newTargetsFromRay.Length; i++){
            newTargetsFromRay[i] = GetGroundPosition(legNewTargets[i]);
        }

        // Move legs if needed
        for(int i = 0; i < newTargetsFromRay.Length; i++){
            MoveLeg(legPositions[i], newTargetsFromRay[i], ref legTargetVectors[i]);
        }
    }

    // Raycasting a point from legNewTargets to get the height of the ground
    Vector3 GetGroundPosition(Transform startPoint){
        Vector3 rayDirection = Vector3.down;
        RaycastHit hit;
        Physics.Raycast(startPoint.position, rayDirection, out hit);
        return hit.point;
    }

    // Move leg if needed
    void MoveLeg(Transform currentPosition, Vector3 newTarget, ref Vector3 targetVector)
    {
        if (Vector3.Distance(currentPosition.position, newTarget) > 1.0f)
        {
            targetVector = newTarget;
        }
        currentPosition.position = targetVector;
    }
}
