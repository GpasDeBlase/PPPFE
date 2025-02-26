using UnityEngine;

public class CheckGround : MonoBehaviour
{
    // Variable priv�e
    private bool _isGrounded = true;

    // Propri�t� pour permettre a "Player" de connaitre _isGrounded
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
