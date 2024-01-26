using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private static StatsManager instance;
    public static StatsManager Instance { get { return instance; } }

    public StatsScriptableObject statsScriptableObject;

    #region Player stats

    //Health

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
    public int MinFirstJumpHeight
    {
        get => statsScriptableObject.minFirstJumpHeight;
        set
        {
            statsScriptableObject.minFirstJumpHeight = value;
            OnMinFirstJumpHeight?.Invoke(statsScriptableObject.minFirstJumpHeight);
        }
    }
    public int MaxFirstJumpHeight
    {
        get => statsScriptableObject.maxFirstJumpHeight;
        set
        {
            statsScriptableObject.maxFirstJumpHeight = value;
            OnMaxFirstJumpHeight?.Invoke(statsScriptableObject.maxFirstJumpHeight);
        }
    }
    public int MinSecJumpHeight
    {
        get => statsScriptableObject.minSecJumpHeight;
        set
        {
            statsScriptableObject.minSecJumpHeight = value;
            OnMinSecJumpHeight?.Invoke(statsScriptableObject.minSecJumpHeight);
        }
    }
    public int MaxSecJumpHeight
    {
        get => statsScriptableObject.maxSecJumpHeight;
        set
        {
            statsScriptableObject.maxSecJumpHeight = value;
            OnMaxSecJumpHeight?.Invoke(statsScriptableObject.maxSecJumpHeight);
        }
    }

    public event Action<int> OnJumps;
    public event Action<int> OnMinFirstJumpHeight;
    public event Action<int> OnMaxFirstJumpHeight;
    public event Action<int> OnMinSecJumpHeight;
    public event Action<int> OnMaxSecJumpHeight;
    #endregion

}
