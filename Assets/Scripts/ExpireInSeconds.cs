using UnityEngine;

public class ExpireInSeconds : MonoBehaviour
{
    [SerializeField] private float liveTime;
    private float expiryTime;
    private void Start()
    {
        expiryTime = Time.time + liveTime;
    }

    private void Update()
    {
        Debug.Log("Expiry time = " + expiryTime);
        if(Time.time > expiryTime)
        {
            Destroy(gameObject);
        }
    }
}
