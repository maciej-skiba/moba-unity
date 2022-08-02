using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    private int nofAbilities = 3;
    private float Q_AvailableTime = 0;
    private float W_AvailableTime = 0;
    private float E_AvailableTime = 0;
    private bool Q_onCD = false;
    private bool W_onCD = false;
    private bool E_onCD = false;

    public string champ = "Giyo";
    [SerializeField] private Image[] abilityIcons;
    public TMPro.TextMeshProUGUI[] abilityTimers;

    private void Cooldown(int abilityIndex)
    {
        int cd = Giyo.Cooldowns[abilityIndex];
        abilityIcons[abilityIndex].color = new Color32(30, 30, 30, 255);
        abilityTimers[abilityIndex].text = cd.ToString();
    }

    private void CooldownGone(int abilityIndex)
    {
        var cd = Giyo.Cooldowns[abilityIndex];
        abilityIcons[abilityIndex].color = new Color(255, 255, 255, 255);
        abilityTimers[abilityIndex].text = "";
    }

    private void Update()
    {     
        if(Input.GetKeyDown(KeyCode.Q) && Time.time > Q_AvailableTime)
        {
            Q_onCD = true;
            Q_AvailableTime = Time.time + Giyo.Cooldowns[0];
            Cooldown(0);
            Giyo.Cast_Q();
        }
        else if(Input.GetKeyDown(KeyCode.W) && Time.time > W_AvailableTime)
        {
            W_onCD = true;
            W_AvailableTime = Time.time + Giyo.Cooldowns[1];
            Cooldown(1);
            Giyo.Cast_W();
        }
        else if(Input.GetKeyDown(KeyCode.E) && Time.time > E_AvailableTime)
        {
            E_onCD = true;
            E_AvailableTime = Time.time + Giyo.Cooldowns[2];
            Cooldown(2);
            Giyo.Cast_E();
        }

        //Set proper 'time left' on abilities and grey out if it is ready
        if(Q_onCD)
        {
            if(Time.time > Q_AvailableTime)
            {
                CooldownGone(0);
            }
            else
            {
                abilityTimers[0].text = ((int)(Q_AvailableTime - Time.time)).ToString();
            }
        }
        if(W_onCD)
        {
            if (Time.time > W_AvailableTime)
            {
                CooldownGone(1);
            }
            else
            {
                abilityTimers[1].text = ((int)(W_AvailableTime - Time.time)).ToString();
            }
        }
        if(E_onCD)
        {
            if (Time.time > E_AvailableTime)
            {
                CooldownGone(2);
            }
            else
            {
                abilityTimers[2].text = ((int)(E_AvailableTime - Time.time)).ToString();
            }
        }
    }
}
