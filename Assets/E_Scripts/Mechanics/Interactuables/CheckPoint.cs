using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : Interactable
{
    public List<Character> enemies;
    List<Vector3> startPositions = new();
    object data;
    Player player;
    public static CheckPoint activePoint = null;
    public bool beenEnabled = false;
    public bool deadEnemies = false;

    public int? CurCheckPoint { get; private set; } = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.OnDead += () => deadEnemies = true;
            startPositions.Add(enemy.transform.position);
        }
    }

    public override void Action()
    {
        base.Action();
        if (activePoint == this) return;

        activePoint = this;
        beenEnabled = true;

        var (fMask, jMask, hpMask, pos, ls,
             curDir, hp, shield) = player;

        (bool, bool, bool, Vector3, Vector3, Vector3, int, int) data =
            (fMask, jMask, hpMask, pos, ls,
             curDir, hp, shield);

        this.data = data;
    }

    public void RestartPoint()
    {
        if (activePoint != null)
            player.LoadData(data);
    }

    public void ReviveEnemies()
    {
        deadEnemies = false;

        int i = 0;
        foreach (var enemy in enemies)
        {
            enemy.Revive();
            enemy.transform.position = startPositions[i++];
            enemy.gameObject.SetActive(true);
        }
    }
}
