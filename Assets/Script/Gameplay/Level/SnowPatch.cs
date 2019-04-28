using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCode;

public class SnowPatch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.Ball)
        {
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.IsGainingSnow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layer.Ball)
        {
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.IsGainingSnow = false;
        }
    }
}
