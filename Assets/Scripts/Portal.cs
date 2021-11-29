using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] Transform spawnPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Entered portal");
    }

    public Transform SpawnPoint => spawnPoint;
}
