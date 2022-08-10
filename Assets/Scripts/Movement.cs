using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    static public float rotationSpeed = 5.0f;
    //Face towards attackTarget object
    static public void LookAtTarget(GameObject attacker, GameObject attackTarget)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = attackTarget.transform.position - attacker.transform.position;
        targetDirection.y = 0;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(attacker.transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        attacker.transform.rotation = Quaternion.LookRotation(newDirection);

    }
}
