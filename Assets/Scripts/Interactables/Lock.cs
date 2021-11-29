using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    #region Animation_variables
    Animator animator;
    #endregion

    #region interaction_variables
    #endregion

    public void unlock()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("unlocked", true);
        //Destroy(this.gameObject);
    }
}
