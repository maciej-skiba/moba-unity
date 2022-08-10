using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Giyo : Champion
{
    [SerializeField] private GameObject W_Fire;
    [SerializeField] private Animator Q_Animator;
    [SerializeField] private GameObject E_Dragon;
    [SerializeField] private Animator E_Animator;

    private NavMeshAgent myAgent;
    private float W_Expiry;
    private AbilityController abilityCtrl;
    private bool W_Ended = true;
    private float E_Expiry;

    static public int[] Cooldowns = { 4, 5, 6 };

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        abilityCtrl = GetComponent<AbilityController>();
    }

    private void Update()
    {
        //Control behavior of the abilities
        if (abilityCtrl.Q_onCD)
        {

        }
        if (abilityCtrl.W_onCD && !W_Ended)
        {
            Instantiate(W_Fire, gameObject.transform.position, Quaternion.identity);
            if (Time.time > W_Expiry)
            {
                W_Ended = true;
                myAgent.speed /= 2.0f;
            }
        }
        if (abilityCtrl.E_onCD)
        {
            if (Time.time > E_Expiry)
            {
                E_Dragon.SetActive(false);
            }
        }
    }
    /*
     * Q - Mythic Strength | Cooldown: 4s
     * ----------------------------------
     * Increase your Attack Damage by 200% for 3s
     */
    public void Cast_Q()
    {
        Q_Animator.SetTrigger("Q_Casted");
        Debug.Log("Q casted");
        StartCoroutine(CoCastQ());
    }
    /*
     * W - Fire Charge | Cooldown: 5s
     * ------------------------------
     * Increase speed by 100% for 2s 
     * and leave a fire path behind the champion.
     */
    public void Cast_W()
    {
        myAgent.speed *= 2;
        W_Expiry = Time.time + 2;
        Debug.Log("W casted");
        W_Ended = false;
    }
    /*
     * E - Dragon's Breath | Cooldown: 6s
     * ----------------------------------
     * Stun an enemy in front of you for 1.5s
     * and deal 100 damage.
     */
    public void Cast_E()
    {
        E_Dragon.SetActive(true);
        E_Animator.SetTrigger("E_Casted");
        E_Expiry = Time.time + 1.5f;
        Debug.Log("E casted");
    }

    IEnumerator CoCastQ()
    {
        this.attackDamage *= 2;
        yield return new WaitForSeconds(3.0f);
        this.attackDamage /= 2;
    }
}
