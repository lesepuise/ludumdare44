﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCode;

public class IcePatch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In");
        if (other.gameObject.layer == Layer.Ball)
        {
            Debug.Log("Payler found");
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.OnIce = true;
        }
        else
        {
            Debug.Log(other.GetType());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out");
        if (other.gameObject.layer == Layer.Ball)
        {
            Debug.Log("Payler found");
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.OnIce = false;
        }
        else
        {
            Debug.Log(other.GetType());
        }
    }
}
