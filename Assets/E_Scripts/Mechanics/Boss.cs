using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : Character
{
    #region Variables
    [Header("References"), Space()]
    [SerializeField] GameObject edgeL;
    [SerializeField] GameObject edgeR;
    [SerializeField] GameObject picos;

    [Header("Settings"), Space()]
    [SerializeField] float slashRate = 1;
    [SerializeField] float slashRange = 3;
    [SerializeField] float closeDis = 4;
    [SerializeField] float midDis = 6;
    [SerializeField] float explotionRadious = 2;
    [SerializeField] float blowCD = 5;
    [SerializeField] float pushSpeed = 6;
    [SerializeField] float pushCD = 4;
    [SerializeField] float spikesTime = 1f;
    [SerializeField] float spikesCD = 8;

    [Space(10), Header("Status")]
    [SerializeField] int fase = 1;
    [SerializeField] PlayerDis curDis = PlayerDis.None;
    [SerializeField] PlayerPos playerPosition = PlayerPos.None;

    [Space(10), Header("Behaviour")]
    [SerializeField, Range(0, 4)] float tgrSlash = 0;
    [SerializeField, Range(0, 12)] float tgrPush = 0;
    [SerializeField, Range(0, 16)] float tgrBlow = 0;
    [SerializeField, Range(0, 20)] float tgrSpike = 0;
    bool canSlash;
    bool canBlow;
    bool canPush;
    bool canAttack = true;

    public event Action OnDie;

    private GameObject myPlayer;
    private Movement myMovement;
    private Attack myAttack;
    bool isNear = false;

    public bool isClose = false;
    [SerializeField] ConditialAction caSlash;
    [SerializeField] ConditialAction caPush;
    [SerializeField] ConditialAction caBlow;
    [SerializeField] ConditialAction caSpikes;

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

    [Serializable]
    class ConditialAction
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
                curTime = curTime + value >= tgrTime ? tgrTime : value;
            }
        }

        public ConditialAction(float tick, float tgrTime, Action action, float? cdTime = null)
        {
            this.tgrTime = tgrTime;
            this.tick = tick;
            curTime = 0;
            this.action = action;
            this.cdTime = cdTime;
            canMove = true;
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

        //            yield return new WaitForSeconds(tick);

        //            if (cdTime.HasValue)
        //                yield return new WaitForSeconds(cdTime.Value);
        //        }
        //    }
        //    finally
        //    {
        //        isRunning = false;
        //    }
        //}

        public void Restart()
        {
            //isRunning = false;
            curTime = 0;
            canMove = true;
        }
    } 
    #endregion

    #region Unity methods
    void Start()
    {
        myMovement = GetComponent<Movement>();
        myAttack = GetComponent<Attack>();

        caSlash = new ConditialAction(slashRate, 4, myAttack.Slash);
        caPush = new ConditialAction(slashRate, 12, () => { });
        caBlow = new ConditialAction(slashRate, 16, myAttack.Blow);
        caSpikes = new ConditialAction(slashRate, 20, myAttack.Trhow_Spikes);

        Invoke("FindPlayer", 2);
    }

    void Update()
    {
        SetTimers();

        if (DetectPlayer())
            Attack();

        Move();
    }

    private void SetTimers()
    {
        if (playerPosition != PlayerPos.UnReachable)
        {
            if (curDis == PlayerDis.Far)
            {
                caPush.CurTime += Time.deltaTime;
                caSpikes.CurTime += Time.deltaTime;
            }
            else if (curDis == PlayerDis.Mid)
            {
                if (playerPosition != PlayerPos.UnReachable)
                {
                    caPush.CurTime += Time.deltaTime;
                }
                else
                    caBlow.CurTime += Time.deltaTime * 2;
            }
            else if (curDis == PlayerDis.Close && playerPosition != PlayerPos.UnReachable)
            {
                if (playerPosition == PlayerPos.Front)
                {
                    caSpikes.CurTime += Time.deltaTime;
                    caBlow.CurTime += Time.deltaTime;
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
                    caBlow.CurTime += Time.deltaTime * 2;
                }
            } 
        }

        tgrBlow = caBlow.CurTime;
        tgrSlash = caSlash.CurTime;
        tgrPush = caPush.CurTime;
        tgrSpike = caSpikes.CurTime;
    }
    #endregion

    private void Attack()
    {
        if (!canAttack) return;

        if (caSpikes.CanMove)
        {
            EnableSpears();
        }
        else if (caBlow.CanMove)
        {
            print("Blow");
        }
        else if (caPush.CanMove)
        {
            Push();
        }
        else if (caSlash.CanMove)
        {
            canAttack = false;
            myAttack.Slash();
            Invoke("EnableAttack", 1);
        }
    }

    private void EnableSpears()
    {
        canAttack = false;
        picos.SetActive(true);

        Invoke("EnableAttack", spikesTime);
    }

    private void SisableSpears()
    {
        picos.SetActive(false);
        canAttack = true;
    }

    private void EnableAttack() => canAttack = true;

    private void Push()
    {
        canAttack = false;
        caPush.canMove = false;

        Vector3 oppositeEdge = default;
        var edgePos = GetNearestEdge(out oppositeEdge);
        Vector3 dir1 = edgePos - transform.position;

        myAttack.EnableDagameOnHit();

        myMovement.MoveTo(edgePos, () =>
            myMovement.MoveTo(oppositeEdge, StopPush));

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
        myAttack.EnableDagameOnHit(false);
        caPush.canMove = true;

        caPush.Restart();
        canAttack = true;
    }

    private void Move()
    {
        //if (curDis == PlayerDis.Far)
        //    myMovement.MyMove(myPlayer.transform.position - transform.position);
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

        return true;
    }

    void FindPlayer() =>
        myPlayer = FindObjectOfType<Player>().gameObject;
}
