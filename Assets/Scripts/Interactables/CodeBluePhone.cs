using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBluePhone : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;

    public void Interact()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().HasEscort()) {
            GameObject GM = GameObject.FindWithTag("GameController");
            GM.GetComponent<GameManager>().MissionComplete();
        } else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
        }
    }

    #region Animation_components
    Animator anim;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWorking", true);
    }
    #endregion
}
