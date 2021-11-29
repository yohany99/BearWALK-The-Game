using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    float x_input;
    float y_input;
    public Vector2 currDirection;
    private float animSpeed;
    #endregion

    #region Adrenaline_variables
    public float maxAdrenaline;
    private float currAdrenaline;
    public float adrenalineDuration;
    public float speedBoost;
    public float boostCooldown;
    public float adrenalineRechargeRate;
    private bool hasCooldown;
    private Coroutine adrenalineRegenerator;
    public Slider AdrenalineBar;
    #endregion

    #region Vent_variables
    public bool isHiding;
    private bool canVent;
    #endregion

    #region PepperSpray_variables
    public int maxPepperSpray;
    private int currPepperSpray;
    public int stunDuration;
    public float attackSpeed = 1;
    public float hitboxTiming;
    public float endAnimationTiming;
    private bool isAttacking;
    float attackTimer;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Animation_components
    Animator animator;
    private SpriteRenderer sr;
    #endregion

    #region Health_variables
    public float maxHealth;
    private float currHealth;
    public Slider HealthBar;
    #endregion

    #region Interactable_variables
    private bool hasKey;
    private bool hasEscort;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currHealth = maxHealth;
        HealthBar.value = currHealth / maxHealth;
        currAdrenaline = maxAdrenaline;
        AdrenalineBar.value = currAdrenaline / maxAdrenaline;
        currPepperSpray = maxPepperSpray;
        hasEscort = false;
        hasKey = false;
        attackTimer = 0;
    }

    public void HandleUpdate()
    {
        if (isAttacking)
        {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKeyDown(KeyCode.Space) && currAdrenaline > 0 && !hasCooldown)
        {
            UseAdrenaline();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.E) && attackTimer <= 0)
        {
            UsePepperSpray();
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F)) //&& PlayerRB.velocity == Vector2.zero)
        {
            if (!isHiding  && canVent)
            {
                UseVent();
            }
            else
            {
                isHiding = false;
            }
        }

        if (currAdrenaline == maxAdrenaline && adrenalineRegenerator != null)
        {
            StopCoroutine(adrenalineRegenerator);
            adrenalineRegenerator = null;
        }

        if (isHiding)
        {
            PlayerRB.velocity = Vector2.zero;
        }

        if (!isHiding)
        {
            sr.enabled = true;
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
            animSpeed = 1;
            currDirection = Vector2.right;
        } 
        else if (x_input < 0)
        {
            PlayerRB.velocity = Vector2.left * moveSpeed;
            animator.SetFloat("Speed", 1);
            animSpeed = 1;
            currDirection = Vector2.left;
        } 
        else if (y_input > 0)
        {
            PlayerRB.velocity = Vector2.up * moveSpeed;
            animator.SetFloat("Speed", 1);
            animSpeed = 1;
            currDirection = Vector2.up;
        }
        else if (y_input < 0)
        {
            PlayerRB.velocity = Vector2.down * moveSpeed;
            animator.SetFloat("Speed", 1);
            animSpeed = 1;
            currDirection = Vector2.down;
        } else
        {
            PlayerRB.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
            animSpeed = 0;
        }
        animator.SetFloat("dirX", currDirection.x);
        animator.SetFloat("dirY", currDirection.y);
    }

    public float GetAnimSpeed()
    {
        return animSpeed;
    }
    #endregion

    #region Vent_functions

    private void UseVent()
    {
        isHiding = true;
        StartCoroutine(VentAnimation());
    }

    IEnumerator VentAnimation()
    {
        animator.SetBool("isHiding", true);
        yield return new WaitForSeconds(0.65f);
        sr.enabled = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Vent"))
        {
            canVent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Vent"))
        {
            canVent = false;
        }
    }
    #endregion

    #region Adrenaline_functions

    private void UseAdrenaline()
    {
        moveSpeed *= speedBoost;
        currAdrenaline--;
        AdrenalineBar.value = currAdrenaline / maxAdrenaline;
        StartCoroutine(ActivateCooldown());
        StartCoroutine(ResetSpeed());
        if (adrenalineRegenerator == null)
        {
            adrenalineRegenerator = StartCoroutine(RefillAdrenaline());
        }
    }
    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(adrenalineDuration);
        moveSpeed /= speedBoost; 
    }

    IEnumerator ActivateCooldown()
    {
        hasCooldown = true;
        yield return new WaitForSeconds(boostCooldown);
        hasCooldown = false;
    }

    IEnumerator RefillAdrenaline()
    {
        yield return new WaitForSeconds(adrenalineRechargeRate);
        while (currAdrenaline < maxAdrenaline)
        {
            currAdrenaline += 1;
            AdrenalineBar.value = currAdrenaline / maxAdrenaline;
            yield return new WaitForSeconds(adrenalineRechargeRate);
        }
        adrenalineRegenerator = null;
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {
        currHealth -= value;

        HealthBar.value = currHealth / maxHealth;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float value)
    {
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);

        HealthBar.value = currHealth / maxHealth;
    }

    private void Die()
    {
        Destroy(this.gameObject, 1f);
        animator.Play("Base Layer.PlayerDeath", 0, 0);
        
    }
    #endregion

    #region PepperSpray_functions
    private void UsePepperSpray()
    {
        Debug.Log("Using pepperspray");
        attackTimer = attackSpeed;
        StartCoroutine(PepperSprayRoutine());
    }

    IEnumerator PepperSprayRoutine()
    {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        animator.SetTrigger("PepperSpray");

        yield return new WaitForSeconds(hitboxTiming);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Used stun on enemy");
                hit.transform.GetComponent<Enemy>().Stunned();
            }
        }
        yield return new WaitForSeconds(hitboxTiming);
        isAttacking = false;
        yield return null;
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
            collider.GetComponent<Interactable>()?.Interact();
            if (collider.GetComponent<Escort>())
            {
                hasEscort = true;
            }
        }
    }

    public bool HasEscort()
    {
        return hasEscort;
    }
    #endregion

    #region Key_functions
    public void getKey() {
        Debug.Log("player has key");
        hasKey = true;
    }

    public bool containsKey()
    {
        Debug.Log("check has key");
        return hasKey;
    }
    #endregion
}
