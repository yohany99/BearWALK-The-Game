using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;
    private float characterVelocity = 2f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float enemyCooldown;
    public float attackDamage;

    private bool playerInRange = false;
    private bool canAttack = true;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
        latestDirectionChangeTime = 0f;
        calculateNewMovementVector();
    }

    private void Update()
    {
        if (playerInRange && canAttack)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().TakeDamage(attackDamage);
            StartCoroutine(AttackCooldown());
        }
        if (player == null)
        {
            MoveRandomly();
        } else
        {
            FollowPlayer();
        }
    }
    #endregion

    #region Movement_functions
    private void FollowPlayer()
    {
        Vector2 direction = player.position - transform.position;
        EnemyRB.velocity = direction.normalized * movespeed;
    }

    private void MoveRandomly()
    {
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            calculateNewMovementVector();
        }
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime));
    }

    private void calculateNewMovementVector()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * characterVelocity;
    }
    #endregion

    #region Attack_functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(enemyCooldown);
        canAttack = true;
    }
    #endregion
}
