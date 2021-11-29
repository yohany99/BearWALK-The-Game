using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Interactable
{
    #region GameObject_variables
    [SerializeField]
    [Tooltip("Healthpack")]
    private GameObject healthpack;
    Animator animator;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #region Chest_functions
    IEnumerator DestroyChest()
    {
        animator = GetComponent<Animator>();
        //yield return new WaitForSeconds(0.5f);
        animator.SetBool("Open", true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(healthpack, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Interact()
    {
        StartCoroutine(DestroyChest());
    }
    #endregion
}
