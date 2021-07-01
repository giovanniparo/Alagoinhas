using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventsMonster : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] eyeLightPositions; //0-right 1-down 2-left 3-up
    [SerializeField] private GameObject eyeLight;

    private Rigidbody2D rb2D;
    private Animator animator;

    private float defaultMoveSpeed;
    private Vector3 moveTarget = Vector3.zero;
    private Vector2 move = Vector2.zero;
    private bool moving = false;
    private bool attacking = false;
    public bool onTarget = false;
    private bool lookingFor = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        defaultMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        UpdateAnimator();
        UpdateEyeLight();
        if(lookingFor)
        {
            lookingFor = false;
            StopMovement();
        }
    }

    private void FixedUpdate()
    {
        if (move.magnitude != 0.0f)
        {
            rb2D.MovePosition((Vector2)transform.position + move * moveSpeed * Time.fixedDeltaTime);

            if ((transform.position - moveTarget).magnitude <= 0.1f)
            {
                onTarget = true;
                moveSpeed = defaultMoveSpeed;
                StopMovement();
            }
        }
    }

    public void OrderToMove(Vector3 moveTarget, float moveSpeed = -1.0f)
    {
        onTarget = false;
        this.moveTarget = moveTarget;
        if(moveSpeed > 0.0f)
            this.moveSpeed = moveSpeed;
        move = (moveTarget - transform.position).normalized;
    }

    private void StopMovement()
    {
        moving = false;
        move = Vector2.zero;
    }

    public void LookTo(Direction lookToDirection)
    {
        lookingFor = true;

        switch (lookToDirection)
        {
            case Direction.Right:
                move = new Vector2(1.0f, 0.0f);
                break;
            case Direction.Down:
                move = new Vector2(0.0f, -1.0f);
                break;
            case Direction.Left:
                move = new Vector2(-1.0f, 0.0f);
                break;
            case Direction.Up:
                move = new Vector2(0.0f, 1.0f);
                break;
        }
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

    private void UpdateAnimator()
    {
        if (move.magnitude > 0.0f)
            moving = true;
        else
            moving = false;

        animator.SetBool("move", moving);
        animator.SetFloat("moveX", move.x);
        animator.SetFloat("moveY", move.y);
    }
}
