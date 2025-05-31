using UnityEngine;

public class ActivateParticle : MonoBehaviour
{
    public GameObject particle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 6)
        {
            particle.SetActive(true);
        }
    }
}
