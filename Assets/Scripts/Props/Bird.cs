using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private enum BirdState
    {
        Idle,
        Waiting,
        Moving,
        Eating,
        Flying
    }
    
    [SerializeField] private float interactRadius;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float flySpeed;
    [SerializeField] private float walkAmplitude;
    [SerializeField] private float averageWaitTime;
    [SerializeField] private float waitTimeAmplitude;

    private GameObject player;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Collider2D coll2D;

    private BirdState state;
    private float waitTime;
    private Vector3 targetPos;
    private Vector3 flyDirection;
    private float timer = 0.0f;
    private int maxIterations = 100;

    private bool eat = false;
    private bool move = false;
    private bool fly = false;

    private bool facingRight = false;
    private int currentIteration = 0;
    private bool playedSound = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll2D = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CheckPlayerPos();
        ProcessState();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (move)
        {
            rb2D.MovePosition(transform.position + targetPos * moveSpeed * Time.deltaTime);
            if((transform.position - targetPos).magnitude <= 0.01f || currentIteration <= 0)
            {
                state = BirdState.Eating;
                move = false;
            }

            currentIteration--;
        }
        if (fly)
        {
            move = false;
            coll2D.enabled = false;
            rb2D.MovePosition(transform.position + flyDirection * flySpeed * Time.deltaTime);
        }
    }

    private void CheckPlayerPos()
    {
        if((transform.position - player.transform.position).magnitude <= interactRadius)
        {
            state = BirdState.Flying;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void ProcessState()
    {
        switch (state)
        {
            case BirdState.Idle:
                waitTime = Random.Range(averageWaitTime - waitTimeAmplitude, averageWaitTime + waitTimeAmplitude);
                state = BirdState.Waiting;
                break;
            case BirdState.Waiting:
                timer += Time.deltaTime;
                if(timer >= waitTime)
                {
                    targetPos = Random.insideUnitCircle * walkAmplitude;

                    if ((targetPos.x < 0f && facingRight) ||
                        (targetPos.x > 0f && !facingRight))
                    {
                        Flip();
                    }

                    eat = false;
                    timer = 0.0f;
                    currentIteration = maxIterations;
                    state = BirdState.Moving;
                }
                break;
            case BirdState.Moving:
                move = true;
                break;
            case BirdState.Eating:
                eat = true;
                waitTime = Random.Range(averageWaitTime - waitTimeAmplitude, averageWaitTime + waitTimeAmplitude);
                state = BirdState.Waiting;
                break;
            case BirdState.Flying:
                fly = true;
                if (!playedSound)
                {
                    playedSound = true;
                    GetComponent<AudioSource>().Play();
                }
                flyDirection = (-1.0f) * (player.transform.position - transform.position).normalized;

                if ((flyDirection.x > 0f && !facingRight) ||
                    (flyDirection.x < 0f && facingRight))
                {
                    Flip();
                }

                Destroy(this.gameObject, 5.0f);
                break;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("move", move);
        animator.SetBool("fly", fly);
        animator.SetBool("eat", eat);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
