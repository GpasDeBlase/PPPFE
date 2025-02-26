using UnityEngine;

public class CheckGround : MonoBehaviour
{
    // Variable privée
    private bool _isGrounded = true;

    // Propriété pour permettre a "Player" de connaitre _isGrounded
    public bool isGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }
}
