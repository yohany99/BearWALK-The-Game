using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
            collision.transform.GetComponent<PlayerController>().getKey();
            Destroy(this.gameObject);
        }
    }

}
