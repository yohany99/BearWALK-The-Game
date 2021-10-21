using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().player = coll.transform;
            Debug.Log("See player, run at player");
        }
    }

    //private void OnTriggerExit2D(Collider2D coll)
    //{
    //    if (!coll.CompareTag("Player"))
    //    {
    //        GetComponentInParent<Enemy>().player = null;
    //    }
    //}
}
