using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escort : MonoBehaviour, Interactable
{
    #region Dialogue_variables
    [SerializeField] Dialogue dialogue;
    #endregion

    #region Movement_variables
    private Transform target;
    private PlayerController player;
    public float moveSpeed;
    private bool madeContact;
    private bool bodyChanged;
    #endregion

    #region Animation_variables
    Animator animator;
    #endregion

    #region Vent_variables
    public bool isHiding;
    #endregion

    #region Dialogue_functions
    public void Interact()
    {
        if (!madeContact)
        {
            if (target.position.x > transform.position.x)
            {
                if ((Mathf.Sqrt(Mathf.Pow(transform.InverseTransformPoint(target.position).y, 2)) <= Mathf.Sqrt(Mathf.Pow(transform.InverseTransformPoint(target.position).x, 2))))
                {
                    animator.SetFloat("dirX", 1);
                }
                else if (transform.InverseTransformPoint(target.position).x <= transform.InverseTransformPoint(target.position).y)
                {
                    animator.SetFloat("dirY", 1);
                }
                else
                {
                    animator.SetFloat("dirY", -1);
                }
            }
            else if (target.position.x < transform.position.x)
            {
                if ((Mathf.Sqrt(Mathf.Pow(transform.InverseTransformPoint(target.position).y, 2)) <= Mathf.Sqrt(Mathf.Pow(transform.InverseTransformPoint(target.position).x, 2))))
                {
                    animator.SetFloat("dirX", -1);
                }
                else if (transform.InverseTransformPoint(target.position).x >= transform.InverseTransformPoint(target.position).y)
                {
                    animator.SetFloat("dirY", -1);
                }
                else
                {
                    animator.SetFloat("dirY", 1);
                }
            }
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
            madeContact = true;
            gameObject.layer = 7;
        }
    }
    #endregion

    #region Unity_functions
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        madeContact = false;
        bodyChanged = false;
    }

    public void HandleUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) > 0.75 && madeContact)
        {
            moveSpeed = player.moveSpeed;
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if (!bodyChanged)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                bodyChanged = true;
            }
            animator.SetFloat("speed", player.GetAnimSpeed());
            animator.SetFloat("dirX", player.currDirection.x);
            animator.SetFloat("dirY", player.currDirection.y);
        }
    }
    #endregion
}
