using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public CheckPoint[] checkPoints;
    Player player;

    private void Awake()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        player.OnDead += RestartEnemies;
    }

    public void RestartEnemies()
    {
        foreach (var checkPoint in checkPoints)
        {
            if (!checkPoint.beenEnabled && checkPoint.deadEnemies)
            {
                checkPoint.ReviveEnemies();
            }
        }
    }
}
