using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Serializable]
    public enum EnemyStates
    {
        Idle,
        Waiting,
        Moving,
        PreparingAttack,
        Attacking,
        Searching,
        Reseting,
        Patrol,
        Guarding
    }

    [SerializeField] private float moveAmplitude;
    [SerializeField] private float moveCooldown;
    [SerializeField] private float waitTimeAmplitude;
    [SerializeField] private float averageWaitTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float resetSpeed;
    [SerializeField] private float chaseRange;
    [SerializeField] private float stunDuration;
    [SerializeField] private float stunCd;
    [SerializeField] private Transform[] eyeLightPositions; //0-right 1-down 2-left 3-up
    [SerializeField] private GameObject eyeLight;
    [SerializeField] private Transform[] patrolPositions;

    [SerializeField] private EnemyStates state = EnemyStates.Idle;
    [SerializeField] private Direction guardingDirection;

    public bool onLoS = false;
    public bool patrolLoop = true;

    private Animator animator;
    private Rigidbody2D rb2D;
    private GameObject player;

    private Vector3 defaultPosition;
    private EnemyStates lastState;
    private Vector3 moveTarget = Vector3.zero;
    private Vector3 move = Vector3.zero;
    private Vector3 attackTarget = Vector3.zero;
    private Vector3 initPosition = Vector3.zero;
    private int currentPatrolTarget = 0;
    private float defaultMoveSpeed;
    private float nextMoveTimer = 0.0f;
    private float waitTime = 0.0f;
    private bool moving = false;
    private bool attacking = false;
    private bool patroling = false;
    private bool reseting = false;
    private bool stunned = false;
    private bool stunOnCd = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        defaultMoveSpeed = moveSpeed;
        defaultPosition = transform.position;
    }

    private void Update()
    {
        if(!stunned)
            ProcessState();

        UpdateAnimator();
        UpdateEyeLight();
    }

    private void FixedUpdate()
    {
        if (!stunned)
        {
            if (moving)
                rb2D.MovePosition(transform.position + move * moveSpeed * Time.fixedDeltaTime);
            else if (attacking)
                rb2D.MovePosition(transform.position + move * attackSpeed * Time.fixedDeltaTime);

            if (moving && (transform.position - moveTarget).magnitude <= 0.1f)
            {
                ResetMovement();
                if (patroling)
                    state = EnemyStates.Patrol;
                else if (reseting)
                {
                    reseting = false;
                    state = lastState;
                }
                else
                    state = EnemyStates.Idle;
            }
            else if (attacking && (transform.position - attackTarget).magnitude <= 0.1f)
            {
                ResetMovement();
                if ((transform.position - player.transform.position).magnitude <= chaseRange || onLoS)
                {
                    AttackPlayer(player.transform);
                }
                else
                {
                    state = EnemyStates.Reseting;
                }
            } 
        }
    }

    private void ResetMovement()
    {
        moving = false;
        attacking = false;
        move = Vector2.zero;
        nextMoveTimer = 0.0f;
        moveSpeed = defaultMoveSpeed;
        initPosition = transform.position;
    }

    private void ProcessState()
    {
        switch (state)
        {
            case EnemyStates.Idle:
                ResetMovement();
                waitTime = UnityEngine.Random.Range(averageWaitTime - waitTimeAmplitude, averageWaitTime + waitTimeAmplitude);
                moveTarget = GetValidMoveTarget();
                move = (moveTarget - initPosition).normalized;
                state = EnemyStates.Waiting;
                break;
            case EnemyStates.Guarding:
                moveSpeed = 0.0f;
                ProcessGuardDirection();
                break;
            case EnemyStates.Waiting:
                nextMoveTimer += Time.deltaTime;
                if (nextMoveTimer >= waitTime)
                    state = EnemyStates.Moving;
                break;
            case EnemyStates.Moving:
                moving = true;
                break;
            case EnemyStates.PreparingAttack:
                initPosition = transform.position;
                move = (attackTarget - transform.position).normalized;
                state = EnemyStates.Attacking;
                break;
            case EnemyStates.Attacking:
                moving = false;
                attacking = true;
                break;
            case EnemyStates.Reseting:
                reseting = true;
                waitTime = UnityEngine.Random.Range(averageWaitTime - waitTimeAmplitude, averageWaitTime + waitTimeAmplitude);
                moveTarget = defaultPosition;
                move = (moveTarget - initPosition).normalized;
                state = EnemyStates.Waiting;
                break;
            case EnemyStates.Patrol:
                ResetMovement();
                patroling = true;
                waitTime = UnityEngine.Random.Range(averageWaitTime - waitTimeAmplitude, averageWaitTime + waitTimeAmplitude);
                moveTarget = patrolPositions[currentPatrolTarget].position;
                move = (moveTarget - initPosition).normalized;
                currentPatrolTarget++;
                if (currentPatrolTarget >= patrolPositions.Length)
                {
                    if (patrolLoop)
                        currentPatrolTarget = 0;
                    else
                        currentPatrolTarget = patrolPositions.Length - 1;
                }
                state = EnemyStates.Waiting;
                break;
        }
    }

    private void ProcessGuardDirection()
    {
        switch (guardingDirection)
        {
            case Direction.Down:
                move = new Vector3(0.0f, -1.0f, 0.0f);
                break;
            case Direction.Left:
                move = new Vector3(-1.0f, 0.0f, 0.0f);
                break;
            case Direction.Up:
                move = new Vector3(0.0f, 1.0f, 0.0f);
                break;
            case Direction.Right:
                move = new Vector3(1.0f, 0.0f, 0.0f);
                break;
        }
    }

    public void SetEnemyState(EnemyStates targetState, Transform[] patrolPos = null, bool loopPatrol = true, Direction guardDirection = Direction.Down)
    {
        state = targetState;
        patrolPositions = patrolPos;
        guardingDirection = guardDirection;
        this.patrolLoop = loopPatrol;
    }

    private void UpdateEyeLight()
    {
        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            if (move.x > 0.0f)
            {
                eyeLight.transform.position = eyeLightPositions[0].position;
                eyeLight.transform.up = eyeLightPositions[0].transform.up;
            }
            else
            {
                eyeLight.transform.position = eyeLightPositions[2].position;
                eyeLight.transform.up = eyeLightPositions[2].transform.up;
            }
        }
        else
        {
            if (move.y > 0.0f)
            {
                eyeLight.transform.position = eyeLightPositions[3].position;
                eyeLight.transform.up = eyeLightPositions[3].transform.up;
            }
            else
            {
                eyeLight.transform.position = eyeLightPositions[1].position;
                eyeLight.transform.up = eyeLightPositions[1].transform.up;
            }
        }
    }

    private Vector3 GetValidMoveTarget()
    {
        Vector3 target = Vector3.zero;
        int maxIterations = 1000;
        int iteCounter = 0;

        while (iteCounter < maxIterations)
        {
            target = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * moveAmplitude;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target, (target - transform.position).magnitude + 1.0f);
            if (hit.collider == null)
                return target;
            iteCounter++;
        }

        return Vector3.zero;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("move", moving);
        animator.SetBool("attack", attacking);
        animator.SetFloat("moveX", move.x);
        animator.SetFloat("moveY", move.y);
    }

    public void AttackPlayer(Transform playerTransform)
    {
        if (!attacking)
        {
            SoundManager.instance.PlaySFX("GameOver");
            lastState = state;
            attackTarget = playerTransform.position;
            state = EnemyStates.PreparingAttack;
        }
    }

    public void StunEnemy()
    {
        if (!stunned && !stunOnCd)
        {
            StartCoroutine(StunCdCoroutine());
        }
    }

    private IEnumerator StunCdCoroutine()
    {
        eyeLight.gameObject.SetActive(false);
        stunned = true;
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
        eyeLight.gameObject.SetActive(true);
        stunOnCd = true;
        yield return new WaitForSeconds(stunCd);
        stunOnCd = false;
    }
}
