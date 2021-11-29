using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    PlayerController player;
    Escort escort;
    Animator animator;
    private bool thisVent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        escort = GameObject.Find("Escort").GetComponent<Escort>();
        thisVent = false;
    }
    private void Update()
    {
        if (player.isHiding && thisVent)
        {
            StartCoroutine(VentAnimation());
        }
        if (!player.isHiding)
        {
            animator.SetBool("hasPlayer", false);
        }
    }

    IEnumerator VentAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("hasPlayer", true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            thisVent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            thisVent = false;
        }
    }
}
