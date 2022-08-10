using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //[SerializeField] private GameObject healthPrefab;

    private Image healthForeground; //Image childrena

    private float verticalOffset = 2.0f;
    private GameObject healthCarrier;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
        if(healthCarrier != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(healthCarrier.transform.position + Vector3.up * verticalOffset);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void AssignCarrier(GameObject carrier)
    {
        this.healthCarrier = carrier;
        healthForeground = GetComponentInChildren<Foreground>().gameObject.GetComponent<Image>();
        carrier.GetComponent<HealthAndStatus>().foregroundImage = healthForeground;
    }
}
