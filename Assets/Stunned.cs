using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunned : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 200 * Time.deltaTime, 0);
    }
}
