using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMinion : Minion
{   
    override protected void Awake()
    {
        base.Awake();
        _minionDamage = 10.0f;
        _minionRange = 1.5f;
    }
}
