using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    public bool canMove;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;
    private bool isStunned;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private float characterVelocity = 2f;
    private int stunDuration;

    EnemyState state;
    float idleTimer = 0f;
    int currentPattern = 0;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform playerTransform;
    private bool targetLost;
    PlayerController player;
    #endregion

    #region Attack_variables
    public float enemyCooldown;
    public float attackDamage;

    private bool playerInRange = false;
    private bool canAttack = true;
    #endregion

    #region Animation_variables
    Animator animator;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;
        currHealth = maxHealth;
        latestDirectionChangeTime = 0f;
        CalculateNewMovementVector();
        targetLost = false;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        stunDuration = player.stunDuration;

    }

    public void HandleUpdate()
    {
        animator.SetBool("isStunned", false);
        if (isStunned)
        {
            EnemyRB.velocity = Vector2.zero;
            StartCoroutine(StunRoutine());
        }
        else if (!canMove)
        {
            EnemyRB.velocity = Vector2.zero;
            return;
        }
        else if (playerInRange && canAttack && !player.isHiding)
        {
            player.TakeDamage(attackDamage);
            StartCoroutine(AttackCooldown());
        }
        else if (playerTransform == null && !targetLost)
        {
            if (state == EnemyState.Idle)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer > timeBetweenPattern)
                {
                    idleTimer = 0f;
                    if (movementPattern.Count > 0)
                    {
                        StartCoroutine(Walk());
                    }
                }
            }
        }
        else if (playerTransform == null && targetLost)
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isMoving", true);
            MoveRandomly();
        }
        else if (playerTransform != null && !player.isHiding)
        {
            animator.SetBool("isChase", true);
            FollowPlayer();
        }
    }
    #endregion

    #region Movement_functions

    IEnumerator Walk()
    {
        state = EnemyState.Walking;
        yield return Move(movementPattern[currentPattern]);
        currentPattern = (currentPattern + 1) % movementPattern.Count;
        state = EnemyState.Idle;
    }

    public IEnumerator Move(Vector2 moveVec)
    {
        animator.SetFloat("dirX", Mathf.Clamp(moveVec.x, -1f, 1f));
        animator.SetFloat("dirY", Mathf.Clamp(moveVec.y, -1f, 1f));

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
        {
            Debug.Log("hit wall");
            yield break;
        }

        animator.SetBool("isMoving", true);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        animator.SetBool("isMoving", false);

    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;
        return !Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer);
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void FollowPlayer()
    {
        if (canMove)
        {
            Vector2 direction = playerTransform.position - transform.position;
            animator.SetFloat("dirX", direction.x);
            animator.SetFloat("dirY", direction.y);
            EnemyRB.velocity = direction.normalized * moveSpeed;
            StartCoroutine(InSight());
            if (Vector3.Distance(playerTransform.position, transform.position) > 6 || player.isHiding)
            {
                playerTransform = null;
                targetLost = true;
            }
            if (isStunned)
            {
                StartCoroutine(StunRoutine());
            }
        }
    }

    IEnumerator InSight()
    {
        yield return new WaitForSeconds(6);
    }

    private void MoveRandomly()
    {
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            CalculateNewMovementVector();
        }
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime));
        animator.SetFloat("dirX", movementPerSecond.x);
        animator.SetFloat("dirY", movementPerSecond.y);
    }

    private void CalculateNewMovementVector()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * characterVelocity;
        // float absX = Mathf.Abs(movementDirection.x);
        // float absY = Mathf.Abs(movementDirection.y);
        // if (movementDirection.x > 0 && (absX > absY)) {
        //     currDirection = Vector2.right;
        // }
        // else if (movementDirection.x < 0 && (absX > absY)) {
        //     currDirection = Vector2.left;
        // } 
        // else if (movementDirection.y < 0 && (absY > absX)) {
        //     currDirection = Vector2.down;
        // } 
        // else if (movementDirection.y > 0 && (absY > absX)) {
        //     currDirection = Vector2.up;
        // }
        // animator.SetFloat("dirX", currDirection.x);
        // animator.SetFloat("dirY", currDirection.y);

    }


    public enum EnemyState { Idle, Walking }

    public void Stunned()
    {
        isStunned = true;
    }

    IEnumerator StunRoutine()
    {
        animator.SetBool("isStunned", true);
        EnemyRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
    #endregion

    #region Attack_functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //animator.SetBool("isAttack", true);
            playerInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //animator.SetBool("isAttack", false);
            playerInRange = false;
        }
    }

    IEnumerator AttackCooldown()
    {
        canMove = false;
        canAttack = false;
        yield return new WaitForSeconds(enemyCooldown);
        canAttack = true;
        canMove = true;
    }
    #endregion

    #region Health_variables
    public float maxHealth;
    private float currHealth;
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {
        currHealth -= value;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
