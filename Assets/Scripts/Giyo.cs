using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giyo : Champion
{
    static public int[] Cooldowns = { 4, 5, 6 };

    static public void Cast_Q()
    {
        Debug.Log("Q casted");
    }
    static public void Cast_W()
    {
        Debug.Log("W casted");
    }
    static public void Cast_E()
    {
        Debug.Log("E casted");
    }
}
