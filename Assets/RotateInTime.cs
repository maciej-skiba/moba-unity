using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInTime : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10.0f;
    private float _fullRotation = 360.0f;
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, (Time.time * _rotationSpeed) % _fullRotation, 0);
    }
}
