using UnityEngine;
using UnityEngine.SceneManagement;

public class PortailLoadSceene : MonoBehaviour
{
    // reference au menu
    public string pathMenu;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6) SceneManager.LoadScene(pathMenu);
    }
}
