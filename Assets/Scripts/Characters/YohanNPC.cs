using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YohanNPC : MonoBehaviour, Interactable
{
    #region Dialogue_variables
    [SerializeField] Dialogue dialogue;
    #endregion

    #region Dialogue_functions
    public void Interact()
    {
        Debug.Log("Yohan going into interact");
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }
    #endregion
}
