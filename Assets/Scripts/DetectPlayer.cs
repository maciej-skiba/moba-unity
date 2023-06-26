using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private GameObject _playerObject;
    private float _rangeView = 5.0f;
    private RaycastHit _hit;
    private Vector3 _raycastDirection;
    private Ray _playerEnemyRay;
    private float rotationSpeed = 5.0f;

    [HideInInspector] public bool followingPlayer = false;

    private void Awake()
    {
        _playerObject = FindObjectOfType<Player>().gameObject;
    }
    private void Start()
    {
        StartCoroutine(CoLookForPlayer());
    }

    private void Update()
    {
        if(followingPlayer)
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = _playerObject.transform.position - transform.position;
            targetDirection.y = 0;

            // The step size is equal to speed times frame time.
            float singleStep = rotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    IEnumerator CoLookForPlayer()
    {
        while(true)
        {
            Vector3 y = new(0, 0.5f, 0);
            _raycastDirection = _playerObject.transform.position - transform.position + y;
            _playerEnemyRay = new Ray(transform.position, _raycastDirection);

            Debug.DrawRay(transform.position + y, _raycastDirection, Color.green);

            if (Physics.Raycast(_playerEnemyRay, out _hit, _rangeView))
            {
                string colliderName = _hit.collider.name;

                if (_hit.collider.tag == "Player")
                {
                    followingPlayer = true;
                }
                else
                {
                    followingPlayer = false;
                }
            }
            else
            {
                followingPlayer = false;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
