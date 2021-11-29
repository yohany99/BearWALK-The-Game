using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float damage;
    //Coroutine SpikesActive;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            //if (SpikesActive == null)
            //{
            //    SpikesActive = StartCoroutine(DamageSpikes(collision));
            //}
            collision.transform.GetComponent<PlayerController>().TakeDamage(damage);
        }
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    IEnumerator DamageSpikes(Collider2D collision)
    {
        while (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().TakeDamage(damage);
            yield return new WaitForSeconds(1f);
        }
        
    }
}
