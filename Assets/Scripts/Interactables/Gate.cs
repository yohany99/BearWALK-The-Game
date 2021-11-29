using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, Interactable
{
    #region Animation_variables
    Animator animator;
    #endregion

    [SerializeField] Dialogue dialogueNoKey;
    [SerializeField] Dialogue dialogueWrongKey;
    PlayerController player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    
    public void Interact()
    {
        Debug.Log("interact");
        if (player.containsKey())
        {
            Debug.Log("unlocked");
            StartCoroutine(UnlockGate());
        } 
        //else if (player has wrong key)
        //{
        //    StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogueWrongKey));
        //}
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogueNoKey));
        }
    }

    IEnumerator UnlockGate()
    {
        gameObject.transform.GetChild(2).GetComponent<Lock>().unlock();
        //gameObject.transform.GetChild(0).GetComponent<Gate>().animator.SetBool("unlocked", true);
        //Debug.Log("Gate 1 unlocked");
        //gameObject.transform.GetChild(1).GetComponent<Gate>().animator.SetBool("unlocked", true);
        //Debug.Log("gate 2 unlocked");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}