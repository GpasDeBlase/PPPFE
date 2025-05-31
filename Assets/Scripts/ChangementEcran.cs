using UnityEngine;
using UnityEngine.InputSystem;

public class ChangementEcran : MonoBehaviour
{
    // variables publiques
    public GameObject _camera;
    public float tempsMouvement = 1;    

    // variables privées 
    private GameObject _ecran;        // pour avoir le transform de l'ecran
    private Transform _actualPos;     // position actuelle de la caméra
    private Vector3 velocity = Vector3.zero;
    private Vector3 target;
    private bool _go = false;



    private void Start()
    {
        _actualPos = _camera.gameObject.transform;
        _ecran = this.gameObject;
        target = new Vector3(_ecran.transform.position.x, _ecran.transform.position.y, -10f);
    }

    private void Update()
    {

        if(_go == true)
        {
            // smooth de la camera
            _camera.transform.position = Vector3.SmoothDamp(_actualPos.position, target, ref velocity, tempsMouvement);
        }

        if(_camera.transform.position == target)
        {
            _go = false;
            // nouvelle pos de la camera
            _actualPos = _camera.gameObject.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) 
        {
            // pour lancer le smooth quand on arrive dans le collider
            _go = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            // pour cancel le smooth si on rechange d'écran
            _go = false;
        }

    }
}
