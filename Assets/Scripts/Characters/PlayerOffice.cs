using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffice : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    float x_input;
    float y_input;
    public Vector2 currDirection;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Animation_components
    Animator animator;
    private SpriteRenderer sr;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Interact();
        }
    }
    #endregion

    #region Movement_functions
    private void Move()
    {
        animator.SetBool("isHiding", false);
        if (x_input > 0)
        {
            PlayerRB.velocity = Vector2.right * moveSpeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.right;
        }
        else if (x_input < 0)
        {
            PlayerRB.velocity = Vector2.left * moveSpeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.left;
        }
        else if (y_input > 0)
        {
            PlayerRB.velocity = Vector2.up * moveSpeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.up;
        }
        else if (y_input < 0)
        {
            PlayerRB.velocity = Vector2.down * moveSpeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.down;
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
        animator.SetFloat("dirX", currDirection.x);
        animator.SetFloat("dirY", currDirection.y);
    }
    #endregion

    #region Interact_functions
    private void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("dirX"), animator.GetFloat("dirY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            Debug.Log("going into interact");
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
    #endregion
}
