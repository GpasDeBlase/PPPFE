using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{

    // References 
    public GameObject canvaNiveaux;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        canvaNiveaux.SetActive(true);
    }

    public void LoadLevel(string path)
    {
        SceneManager.LoadScene(path);
    }
}
