using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private bool standardFlashlight = true;
    private bool rareFlashlight = false;
    private bool legendaryFlashlight = false;


    private void Start()
    {
        if (standardFlashlight)
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
        else if (rareFlashlight)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (legendaryFlashlight)
        {
            transform.localScale = new Vector3(5, 5, 1);
        }

    }
}
