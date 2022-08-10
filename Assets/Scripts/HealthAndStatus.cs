using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndStatus : MonoBehaviour
{
    [SerializeField] private float updateSpeedSeconds;
    private float maxHealth;

    [HideInInspector] public Image foregroundImage;
    public float Health;
    public float Mana;
    public float Energy;
    public bool isStunned = false;
    private void Awake()
    {
        maxHealth = Health;
    }
    private void OnEnable()
    {
        Player.AttackEvent += ChangeHealthBarToPct;
        MeleeMinion.AttackEvent += ChangeHealthBarToPct;
    }
    private void OnDisable()
    {
        Player.AttackEvent -= ChangeHealthBarToPct;
        MeleeMinion.AttackEvent -= ChangeHealthBarToPct;
    }
    private void Update()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeHealthBarToPct(GameObject gameObj = null)
    {
        StartCoroutine(CoChangeHealthBarToPct());
    }

    public IEnumerator CoChangeHealthBarToPct()
    {
        float preChangePct = foregroundImage.fillAmount;
        float timeElapsed = 0f;
        float newFillAmount = 0;

        while (timeElapsed < updateSpeedSeconds)
        {
            timeElapsed += Time.deltaTime;

            newFillAmount = Mathf.Lerp((float)preChangePct, (float)this.Health / (float)this.maxHealth, timeElapsed / updateSpeedSeconds);
            foregroundImage.fillAmount = newFillAmount;
                
            yield return null;
        }
    }
}
