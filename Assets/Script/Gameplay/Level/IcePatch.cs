using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCode;

public class IcePatch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.Ball)
        {
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.OnIce = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layer.Ball)
        {
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.OnIce = false;
        }
    }
}
