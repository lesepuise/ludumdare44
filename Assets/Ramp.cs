using CleverCode;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    public Transform velocityTarget;

    public float strength;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Layer.Ball)
        {
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            
            player.RigidBody.AddForce(velocityTarget.forward * strength, ForceMode.Acceleration);
        }
    }
}