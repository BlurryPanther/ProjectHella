using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : Character
{
    #region Variables
    [Header("References"), Space()]
    [SerializeField] protected GameObject edgeL;
    [SerializeField] protected GameObject edgeR;
    [SerializeField] GameObject picos;

    [Header("Settings"), Space()]
    [SerializeField, Range(0, 1)] float secondPhace;
    [SerializeField] float slashRate = 1;
    [SerializeField] float slashRange = 3;
    [SerializeField] float closeDis = 4;
    [SerializeField] float midDis = 6;
    [SerializeField] float explotionRadious = 2;
    [SerializeField] float blowCD = 5;
    [SerializeField] float pushCD = 4;
    [SerializeField] float thornsTime = 1f;
    [SerializeField] float thornsCD = 8;

    [Space(10), Header("Status")]
    [SerializeField] int fase = 1;
    [SerializeField] PlayerDis curDis = PlayerDis.None;
    [SerializeField] PlayerPos playerPosition = PlayerPos.None;

    [Space(10), Header("Behaviour")]
    [SerializeField, Range(0, 4)] float tgrSlash = 0;
    [SerializeField, Range(0, 12)] float tgrPush = 0;
    [SerializeField, Range(0, 16)] float tgrBlow = 0;
    [SerializeField, Range(0, 20)] float tgrthorn = 0;
    bool canSlash;
    bool canBlow;
    bool canPush;
    bool canAttack = true;
    bool drawExplotion = false;

    public event Action OnDie;

    private GameObject myPlayer;
    private Character target;
    bool isNear = false;

    public bool isClose = false;
    [SerializeField] myAction caSlash;
    [SerializeField] myAction caPush;
    [SerializeField] myAction caBlow;
    [SerializeField] myAction caThorns;

    enum Hability
    {
        None,
        Slash,
        Hit,
        Push,
        Spikes,
        Move,
        Retreat
    }

    enum PlayerDis
    {
        None,
        Close,
        Mid,
        Far
    }

    [Flags]
    enum PlayerPos
    {
        None,
        Front,
        Back,
        Up,
        UnReachable,
    }

    class HabilityData
    {
        public float progress = 0;
        public bool enable = false;
    }
    #endregion

    #region Unity methods
    void Start()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();

        caSlash = new myAction(slashRate, 4);
        caPush = new myAction(slashRate, 12);
        caBlow = new myAction(slashRate, 16);
        caThorns = new myAction(slashRate, 20);

        Invoke("FindPlayer", 2);
    }

    protected override void Update()
    {
        base.Update();

        SetTimers();

        if (DetectPlayer())
            Attack();

        Move();

        CheckPhase();
    }

    private void CheckPhase()
    {
        if (hp < hp * secondPhace)
            fase = 2;
        else
            fase = 1;
    }

    private void OnDrawGizmos()
    {
        if (drawExplotion)
            Gizmos.DrawWireSphere(transform.position, explotionRadious);
    }

    private void SetTimers()
    {
        if (fase == 2) return;

        if (curDis == PlayerDis.Far)
        {
            if (playerPosition != PlayerPos.UnReachable)
            {
                caPush.CurTime += Time.deltaTime;
                caThorns.CurTime += Time.deltaTime; 
            }
            else if (playerPosition == PlayerPos.UnReachable)
            {
                caBlow.CurTime += Time.deltaTime;
            }
        }
        else if (curDis == PlayerDis.Mid)
        {
            if (playerPosition != PlayerPos.UnReachable)
            {
                caPush.CurTime += Time.deltaTime;
                caSlash.CurTime += Time.deltaTime;
            }
            else
                caBlow.CurTime += Time.deltaTime * 2;
        }
        else if (curDis == PlayerDis.Close && playerPosition != PlayerPos.UnReachable)
        {
            if (playerPosition == PlayerPos.Front)
            {
                caThorns.CurTime += Time.deltaTime;
                caSlash.CurTime += Time.deltaTime;
                caPush.CurTime += Time.deltaTime;
            }
            else if (playerPosition == PlayerPos.Back)
            {
                caBlow.CurTime += Time.deltaTime * 2;
                caSlash.CurTime += Time.deltaTime * 2;
            }
            else if (playerPosition == PlayerPos.Up)
            {
                caBlow.CurTime += Time.deltaTime * 2;
                caThorns.CurTime += Time.deltaTime * 2;
            }
        }

        tgrBlow = caBlow.CurTime;
        tgrSlash = caSlash.CurTime;
        tgrPush = caPush.CurTime;
        tgrthorn = caThorns.CurTime;
    }
    #endregion

    private void Attack()
    {
        if (!canAttack) return;

        if (fase == 1)
        {
            if (caThorns.CanMove)
            {
                EnableThorns();
            }
            else if (caBlow.CanMove)
            {
                if (!canJump) return;

                canAttack = false;
                caBlow.Restart();

                movement.MoveTo(myPlayer.transform.position, () => Invoke("Blow", .5f));
            }
            else if (caPush.CanMove)
            {
                Push();
            }
            else if (caSlash.CanMove)
            {
                if (!canJump) return;
                canAttack = false;

                movement.MoveTo(myPlayer.transform.position, Slash);
                //if (curDis == PlayerDis.Mid || curDis == PlayerDis.Far)
                //{
                //    movement.MoveTo(myPlayer.transform.position, Slash);
                //}
                //else
                //    Slash();
            } 
        }
        else
        {
            var rand = UnityEngine.Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    EnableThorns();
                    break;

                case 1:
                    if (!canJump) return;
                    canAttack = false;
                    caBlow.Restart();

                    movement.MoveTo(myPlayer.transform.position, () => Invoke("Blow", .5f));
                    break;

                case 2:
                    if (!canJump) return;
                    Push();
                    break;

                case 3:
                    if (!canJump) return;
                    canAttack = false;

                    movement.MoveTo(myPlayer.transform.position, Slash);
                    break;

                default:
                    break;
            }
        }
    }

    private void Blow()
    {
        attack.Blow(explotionRadious, target);

        DrawExplotionGizmo();
        Invoke("StopExplotion", 2);
        Invoke("EnableAttack", 2);
    }

    private void DrawExplotionGizmo() => drawExplotion = true;

    private void StopExplotion() => drawExplotion = false;

    private void Slash()
    {
        attack.Slash();
        caSlash.Restart();

        Invoke("EnableAttack", slashRate);
    }

    private void EnableThorns()
    {
        canAttack = false;
        picos.SetActive(true);

        Invoke("DisableThorns", thornsTime);
    }

    private void DisableThorns()
    {
        caThorns.Restart();
        picos.SetActive(false);
        EnableAttack();
    }

    private void EnableAttack() => canAttack = true;

    private void Push()
    {
        canAttack = false;
        caPush.canMove = false;

        Vector3 oppositeEdge = default;
        var edgePos = GetNearestEdge(out oppositeEdge);
        Vector3 dir1 = edgePos - transform.position;

        attack.EnableDagameOnHit();

        movement.MoveTo(edgePos, () =>
            movement.MoveTo(oppositeEdge, StopPush, 2.6f));

        Vector3 GetNearestEdge(out Vector3 opposite)
        {
            if (Vector3.Distance(transform.position, edgeL.transform.position) <
                Vector3.Distance(transform.position, edgeR.transform.position))
            {
                opposite = edgeR.transform.position;
                return edgeL.transform.position;
            }
            else
            {
                opposite = edgeL.transform.position;
                return edgeR.transform.position;
            }
        }
    }

    private void StopPush()
    {
        attack.EnableDagameOnHit(false);
        caPush.canMove = true;

        caPush.Restart();
        canAttack = true;
    }

    private void Move()
    {
        //if (curDis == PlayerDis.Far)
        //    movement.MyMove(myPlayer.transform.position - transform.position);
    }

    private bool DetectPlayer()
    {
        if (myPlayer == null) return false;

        var dis = Vector3.Distance(myPlayer.transform.position, transform.position);

        if (dis <= 0)
            curDis = PlayerDis.None;
        else if (dis > 0 && dis <= closeDis)
            curDis = PlayerDis.Close;
        else if (dis < midDis)
            curDis = PlayerDis.Mid;
        else 
            curDis = PlayerDis.Far;

        if (Physics.BoxCast(transform.position, new Vector3(.5f, .5f, .5f), movement.CurDirection, Quaternion.identity, 1, 1 << 9))
        {
            playerPosition = PlayerPos.Front;
            print("Front");
        }
        else if (Physics.BoxCast(transform.position, new Vector3(.5f, .5f, .5f), Vector3.up, Quaternion.identity, 1, 1 << 9))
        {
            playerPosition = PlayerPos.Up;
            print("Up");
        }
        else if (Physics.BoxCast(transform.position, new Vector3(.5f, .5f, .5f), movement.CurDirection * -1, Quaternion.identity, 1, 1 << 9))
        {
            playerPosition = PlayerPos.Back;
            print("Back");
        }
        else if (Vector3.Distance(myPlayer.transform.position, transform.position) > closeDis)
        {
            if (myPlayer.transform.position.x > transform.position.x)
            {
                if (Vector3.Angle(myPlayer.transform.position - transform.position, Vector3.right) > 20)
                    playerPosition = PlayerPos.UnReachable;
            }
            else if (Vector3.Angle(myPlayer.transform.position - transform.position, Vector3.left) > 20)
                playerPosition = PlayerPos.UnReachable;
        }
        else
            playerPosition = PlayerPos.None;

        return true;
    }

    void FindPlayer() =>
        myPlayer = FindObjectOfType<Player>().gameObject;
}

[Serializable]
public class myAction
{
    public float tgrTime;
    public float tick;
    //public bool isRunning;
    public float? cdTime;
    public float progress = 0;
    float curTime;
    Action action;
    public bool canMove;

    public bool CanMove
    {
        get
        {
            if (canMove && curTime >= tgrTime)
                return true;

            return false;
        }
    }

    public float CurTime
    {
        get => curTime;
        set
        {
            curTime = value >= tgrTime ? tgrTime : value;
        }
    }

    public myAction(float tick, float tgrTime, float? cdTime = null)
    {
        this.tgrTime = tgrTime;
        this.tick = tick;
        curTime = 0;
        this.action = null;
        this.cdTime = cdTime;
        canMove = true;
    }

    public myAction(float? cdTime)
    {
        this.cdTime = cdTime;
        canMove= true;
    }

    //public IEnumerator StartLoop()
    //{
    //    isRunning = true;
    //    curTime = 0;

    //    try
    //    {
    //        while (true)
    //        {
    //            action?.Invoke();

    //            yield return new WaitForSeconds(rate);

    //            if (cdTime.HasValue)
    //                yield return new WaitForSeconds(cdTime.Value);
    //        }
    //    }
    //    finally
    //    {
    //        isRunning = false;
    //    }
    //}

    public IEnumerator CoolDown()
    {
        canMove = false;

        yield return new WaitForSeconds(cdTime.Value);

        canMove = true;
    }

    public void Restart()
    {
        //isRunning = false;
        curTime = 0;
        canMove = true;
    }
}
