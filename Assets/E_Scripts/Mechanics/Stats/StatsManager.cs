using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private static StatsManager instance;
    public static StatsManager Instance { get { return instance; } }

    public StatsScriptableObject statsScriptableObject;

    public bool haveJumpMask = false;

    #region Player stats

    //Health

    public float GetMinJumpForce(bool haveMask) => MinFirstJumpHeight;

    public int Hp
    {
        get => statsScriptableObject.hp;
        set
        {
            statsScriptableObject.hp = value;
            OnHp?.Invoke(statsScriptableObject.hp);
        }
    }

    public event Action<int> OnHp;

    //Damage
    public int Dmg
    {
        get => statsScriptableObject.dmg;
        set
        {
            statsScriptableObject.dmg = value;
            OnDmg?.Invoke(statsScriptableObject.dmg);
        }
    }

    public event Action<int> OnDmg;

    //Jump
    public int Jumps
    {
        get => statsScriptableObject.jumps;
        set
        {
            statsScriptableObject.jumps= value;
            OnJumps?.Invoke(statsScriptableObject.jumps);
        }
    }
    public float MinFirstJumpHeight
    {
        get => statsScriptableObject.minFirstJumpHeight;
        set
        {
            statsScriptableObject.minFirstJumpHeight = value;
            OnMinFirstJumpHeight?.Invoke(statsScriptableObject.minFirstJumpHeight);
        }
    }
    public float MaxFirstJumpHeight
    {
        get => statsScriptableObject.maxFirstJumpHeight;
        set
        {
            statsScriptableObject.maxFirstJumpHeight = value;
            OnMaxFirstJumpHeight?.Invoke(statsScriptableObject.maxFirstJumpHeight);
        }
    }


    public float MinSecJumpHeight
    {
        get => statsScriptableObject.minSecJumpHeight;
        set
        {
            statsScriptableObject.minSecJumpHeight = value;
            OnMinSecJumpHeight?.Invoke(statsScriptableObject.minSecJumpHeight);
        }
    }
    public float MaxSecJumpHeight
    {
        get => statsScriptableObject.maxSecJumpHeight;
        set
        {
            statsScriptableObject.maxSecJumpHeight = value;
            OnMaxSecJumpHeight?.Invoke(statsScriptableObject.maxSecJumpHeight);
        }
    }

    public event Action<int> OnJumps;
    public event Action<float> OnMinFirstJumpHeight;
    public event Action<float> OnMaxFirstJumpHeight;
    public event Action<float> OnMinSecJumpHeight;
    public event Action<float> OnMaxSecJumpHeight;
    #endregion

}
