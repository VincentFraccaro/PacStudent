using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("Direction", 2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetInteger("Direction", 0);
        }
    }
}
