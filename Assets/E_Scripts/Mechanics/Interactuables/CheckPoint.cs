using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CheckPoint : Interactable
{
    Interactable instance;
    CheckPoint[] checkPoints;
    object data;

    public int? CurCheckPoint { get; private set; } = null;
    public Interactable Instance { get => instance; }

    private void Awake()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        instance ??= this;
    }

    public override void Action()
    {
        base.Action();

        //var ( fMask, jMask, hpMask, pos, ls, 
        //     curDir, hp, shield) = FindObjectOfType<Player>().Deconstruct();


    }
}
