using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int _healthIncrease = 20;
    private HealthAndStatus _playerHealthAndStatus;

    private void Start()
    {
        _playerHealthAndStatus = Giyo.Instance.GetComponent<HealthAndStatus>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            _playerHealthAndStatus.health += _healthIncrease;
            Destroy(this.gameObject);
        }
    }
}
