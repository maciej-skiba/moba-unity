using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHealthBar : MonoBehaviour
{
    [SerializeField] private HealthBar healthBarPrefab;
    private GameObject mainCanvas;
    private void Start()
    {
        mainCanvas = FindObjectOfType<MainCanvas>().gameObject;
        HealthBar healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
        healthBarInstance.AssignCarrier(this.gameObject);
    }
}
