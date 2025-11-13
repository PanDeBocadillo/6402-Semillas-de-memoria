using UnityEngine;

public class LookOppositeOfTarget : MonoBehaviour
{
    public Transform target; // Assign the target in the inspector

    void Update()
    {
        if (target != null)
        {
            // Calculate direction from this object to the target
            Vector3 toTarget = target.position - transform.position;
            // Set forward to the opposite direction
            if (toTarget != Vector3.zero)
                transform.forward = -toTarget.normalized;
        }
    }
}
