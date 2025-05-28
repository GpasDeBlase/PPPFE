using UnityEngine;

public class CheckGround : MonoBehaviour
{
    // Variable privée
    private bool _isGrounded = true;
    private Rigidbody2D rb;
    private bool _isTriggered = false;

    // Propriété pour permettre a "Player" de connaitre _isGrounded
    public bool isGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(isGrounded);
        if ((rb.linearVelocity.y == 0) && (_isTriggered = true)) _isGrounded = true;
        else _isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) _isTriggered = true;       // _isTriggered va nous dire si le gameeobject est dans un sol
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isTriggered = false;
    }
}
