using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthAndStatus : MonoBehaviour
{
    [SerializeField] private float updateSpeedSeconds;
    [SerializeField] private TextMeshProUGUI _healthText;

    private float maxHealth;

    [HideInInspector] public Image foregroundImage;
    public float health;
    public float mana;
    public float energy;
    public bool isStunned = false;

    public static HealthAndStatus Instance { get; set; }
    private void Awake()
    {
        maxHealth = health;
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
        if (_healthText != null) _healthText.text = "HP: " + health.ToString();

        print(health);

        if (health <= 0)
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

            newFillAmount = Mathf.Lerp((float)preChangePct, (float)this.health / (float)this.maxHealth, timeElapsed / updateSpeedSeconds);
            foregroundImage.fillAmount = newFillAmount;
                
            yield return null;
        }
    }
}
