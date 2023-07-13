using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;

    [SerializeField] private LayerMask platformLayerMask;

    private void OnTriggerStay2D(Collider2D other) {
        isGrounded = other != null && (((1 << other.gameObject.layer) & platformLayerMask) != 0);
    }

    private void OnTriggerExit2D(Collider2D other) {
        isGrounded = false;
    }
}
