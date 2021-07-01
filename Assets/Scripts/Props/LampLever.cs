using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLever : MonoBehaviour
{
    private Animator animator;

    public bool pull = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("pull", pull);
    }
}
