using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : MonoBehaviour
{
    #region Healthpack_variables
    [SerializeField] 
    [Tooltip("The amount the player heals")]
    private float healAmount;
    #endregion

    #region Heal_functions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Heal(healAmount);
            Destroy(this.gameObject);
        }
    }
    #endregion

}
