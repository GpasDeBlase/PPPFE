using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileThrowed : MonoBehaviour
{
    // Variables publiques
    public float speed =100f;       // Vitesse du projectile si non renseignée dans le prefab
    public float lifeTime = 3f;     // Temps de vie du projectile
    public GameObject player;       // Stocker la reference du Player
    public GameObject deathParticle;    // Pour quand le projectile explose

    // Variable privée
    private LayerMask mask;         // LayerMask sur lequel le projectile collisione

    void Start()
    {
        StartCoroutine(Autodestruction());
        mask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);   // le projectile avance droit devant lui
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            player.GetComponent<PlayerController>().canThrow = true;    // change canThrow dans Player avant de se détruire
            Instantiate(deathParticle,gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator Autodestruction()
    {
        yield return new WaitForSeconds(lifeTime);                      // attends  lifeTime avant d'executer la suite
        player.GetComponent<PlayerController>().canThrow = true;        // change canThrow dans Player avant de se détruire
        Instantiate(deathParticle, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
