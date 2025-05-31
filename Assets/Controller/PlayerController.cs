using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
	// Variables publiques
	public float movementSpeed = 10f;           // multiplicateur de vitesse du joueur
	public float jumpStrenght;                  // multiplicateur de force du saut
    public int nbrJump = 2;                     // nombre de saut que le joueur poss�de entre chaque reset

    // Variables type reference
    public GameObject checkSol;                 // reference au gameObject qui v�rifie si le joueur touche le sol
    public GameObject projectileSpawner;        // reference au gameObject qui indique ou va spawn le projectile
    public GameObject projectile;               // reference au prefab du projectile, a faire spawn
    public GameObject sprite;                   // reference au gameObject du Sprite pour le d�sactiver a la mort
    public ParticleSystem deathParticle;            // reference a la particule a jouer quand le perso meurt

    // Variables priv�es
    [SerializeField] private Vector3 _checkPos;     // checkpoint o� respawn
    private Rigidbody2D rb;                     // rigidBody du gameObject Player
    private float _horizontalInput;             // -1 ou 1 suivant si le joueur appuie sur Q ou D
    private int jumpsRemaining = 2;             // nombre de sauts restant
    private bool canThrowProjectile;            // est ce que le joueur peut lancer un projectile
    private GameObject _proj;                   // Instance du projectile
    private bool _isRespawning;                 // Pour �viter qu'i y ait plusieurs fois la coroutine de respawn en meme temps

    // Propri�t�s
    public bool canThrow                        // propri�t� pour get et set canThrowProjectile pour que les projectiles puissent modifier ce bool
    {
        get { return canThrowProjectile; }
        set { canThrowProjectile = value; }
    }

    
    /// /////////////
    // Instructions a faire au lancement du jeu
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = nbrJump;
        canThrow = true;
        _checkPos = this.gameObject.transform.position;
    }

    // FixedUpdate pour les mouvements horizontaux du joueur (�a fonctionne mieux ici)
    private void FixedUpdate()
    {
        // d�placement horizontal
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        float horizontalMovement = _horizontalInput * movementSpeed * Time.deltaTime;
        rb.linearVelocity = new Vector2(horizontalMovement, rb.linearVelocity.y);

    }

    // Update pour tout ce qui est des Input.KeyDown, lancement des m�thodes
    private void Update()
    {
        // Debugs
        //Debug.Log(canThrow);

        // Orientation personnage (pour que le tir parte dans la derniere direction vers laquelle le personnage a avance)
        if (Input.GetKeyDown(KeyCode.A)) projectileSpawner.transform.localPosition = new Vector3(-0.6f, 0f, 0f);
        if (Input.GetKeyDown(KeyCode.D)) projectileSpawner.transform.localPosition = new Vector3(0.6f, 0f, 0f);

        // JUMP
        if (Input.GetButtonDown("Jump")) Jump();

        // Respawn
        if ((Input.GetButtonDown("Respawn")) && (_isRespawning == false)) StartCoroutine(Respawn());

        // D�tection sol (seulement quand le joueur n'as plus de sauts)
        if (jumpsRemaining == 0) OnGround();

        // Lancer du projectile
        // Vis�e (si on peut tirer)
        if (Input.GetKey(KeyCode.Mouse0) && canThrow) Aiming();
        // Lancer (si on peut tirer)
        if (Input.GetKeyUp(KeyCode.Mouse0) && canThrow) Throwing();

        // T�l�portation sur le projectile si un projectile existe (_proj = true)
        if (Input.GetKeyDown(KeyCode.Mouse1) && (_proj == true)) Teleportation();

        // O� est la souris
        TrackMouse();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer == 8) && (_isRespawning == false)) StartCoroutine(Respawn());
    }

    private void Jump()
    {
        if (jumpsRemaining >=1)                                                 // on regarde si il reste des sauts a disposition
        {
            rb.linearVelocity = new Vector2(0, 0);                              // On meet la velocit� a 0 pour toujours faire un saut a la meme hauteur
            rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);        // On ajoute une impulsion
            jumpsRemaining -= 1;
        }
    }

    IEnumerator Respawn()
    {
        _isRespawning = true;                                                   // On �vite que la coroutine se lance plusieurs fois en meme temps

        rb.constraints = RigidbodyConstraints2D.FreezeAll;                      // on enleve les mouvements au joueur
        deathParticle.Play();                                                   // On joue la particule
        sprite.SetActive(false);                                                // On d�sactive le sprite en meme temps

        yield return new WaitForSeconds(deathParticle.main.duration);           // On attend que la particule finisse d'etre jouer pour effectuer la prochaine action

        rb.position = _checkPos;                                                // On tp le joueur
        rb.linearVelocity = new Vector2(0, 0);                                  // On met la v�locit� a 0
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;                 // On remet jsute le freeze sur la rotation
        sprite.SetActive(true);                                                 // On r�active le sprite

        _isRespawning = false;                                                  // On remet la possiblit� de respawn
    }

    private void OnGround()
    {
        // checksol possede son propre script, on veut juste le resultat de isGrounded
        if (checkSol.GetComponent<CheckGround>().isGrounded == true)
        {
            jumpsRemaining = nbrJump;       // reset des sauts
        }
    }

    void TrackMouse()
    {
        // Position de la souris sur l'�cran
        Vector3 mousePosition = Input.mousePosition;

        // Convertion en world space
        Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Update de la rotation de projectileSpawner
        Vector2 direction = _mousePosition - projectileSpawner.transform.position;
        projectileSpawner.transform.up = direction;
    }

    private void Aiming()
    {
        Time.timeScale = 0.1f;              // pendant la visee (tant que le joueur ne relache pas mouse0) on veut du controle donc on ralenti le temps
    }

    private void Throwing()
    {
        Time.timeScale = 1;                 // apr�s le tir on remet la vitesse du jeu a la normale
        // Instantiation d'un projectile, on le reference dans une variable pour pouvoir lui passer la ref du joueur
        _proj = Instantiate(projectile, projectileSpawner.transform.position, projectileSpawner.transform.rotation);
        _proj.GetComponent<ProjectileThrowed>().player = gameObject;
        canThrow = false;                   // on empeche le tir d'un projectile tant qu'il y en a un en jeu (le proejctile gere lui meme pour remet cette condition vraie)
    }

    private void Teleportation()
    {
        var _newpos = _proj.transform.position;     // on recupere la position du projectile
        Destroy(_proj);                             // puis on le detruit
        rb.position = _newpos;                      // on "tp" le joueur sur la derniere position du projectile
        rb.linearVelocity = new Vector2(0, 0);      // on remet la v�locit� a 0 pour �viter que le joueur tombe
        canThrow = true;                            // on redonne la possibilite de tirer
    }
}
