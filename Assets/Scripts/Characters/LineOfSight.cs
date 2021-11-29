using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (!coll.GetComponent<PlayerController>().isHiding)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.parent.position, coll.transform.position, GameLayers.i.SolidObjectsLayer);
                if (hit.collider != null && hit.collider.CompareTag("Solid Object"))
                {
                    return;
                }
                else
                {
                    GetComponentInParent<Enemy>().playerTransform = coll.transform;
                }
            }
        }
    }
}
