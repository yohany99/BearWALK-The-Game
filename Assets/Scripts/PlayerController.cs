using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    float x_input;
    float y_input;
    Vector2 currDirection;
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
    public Slider AdrenalineSlider;
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
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currHealth = maxHealth;
        HPSlider.value = currHealth / maxHealth;
        currAdrenaline = maxAdrenaline;
        AdrenalineSlider.value = currAdrenaline / maxAdrenaline;
    }

    private void Update()
    {
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKeyDown(KeyCode.Space) && currAdrenaline > 0 && !hasCooldown)
        {
            UseAdrenaline();
        }
    }
    #endregion

    #region Movement_functions
    private void Move()
    {
        if (x_input > 0)
        {
            PlayerRB.velocity = Vector2.right * movespeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.right;
        } 
        else if (x_input < 0)
        {
            PlayerRB.velocity = Vector2.left * movespeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.left;
        } 
        else if (y_input > 0)
        {
            PlayerRB.velocity = Vector2.up * movespeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.up;
        }
        else if (y_input < 0)
        {
            PlayerRB.velocity = Vector2.down * movespeed;
            animator.SetFloat("Speed", 1);
            currDirection = Vector2.down;
        } else
        {
            PlayerRB.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
        animator.SetFloat("dirX", currDirection.x);
        animator.SetFloat("dirY", currDirection.y);
    }
    #endregion

    #region Adrenaline_functions

   private void UseAdrenaline()
    {
        movespeed *= speedBoost;
        currAdrenaline--;
        AdrenalineSlider.value = currAdrenaline / maxAdrenaline;
        StartCoroutine(ActivateCooldown());
        StartCoroutine(ResetSpeed());
        if (adrenalineRegenerator != null)
        {
            StopCoroutine(adrenalineRegenerator);
        }
        adrenalineRegenerator = StartCoroutine(RefillAdrenaline());
    }
    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(adrenalineDuration);
        movespeed /= speedBoost; 
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
            AdrenalineSlider.value = currAdrenaline / maxAdrenaline;
            yield return new WaitForSeconds(adrenalineRechargeRate);
        }
        adrenalineRegenerator = null;
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {
        currHealth -= value;

        HPSlider.value = currHealth / maxHealth;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float value)
    {
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);

        HPSlider.value = currHealth / maxHealth;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
