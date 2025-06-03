using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.SearchService;
using TMPro;

public class TitleScene : MonoBehaviour
{

    // References 
    public GameObject canvaNiveaux;
    public GameObject[] boutons;
    public GameObject[] cadenas;

    public GameObject[] stats;
    public TextMeshProUGUI[] timers;
    public TextMeshProUGUI[] morts;

    // variables
    public DataClass datas = new DataClass();       // génération d'un DataClass pour y stocker nos données
    private string fileUrl;                         // Chemin (URL) vers le fichier de sauvegarde

    void Start()
    {
        // Préparation du chemin vers le fichier
        fileUrl = Application.persistentDataPath + "/save.json";

        // Charge les données dans le portail
        LoadGame();
    }

    void LoadGame()
    {
        // Lecture dans le fichier
        string dataJson = File.ReadAllText(fileUrl);
        // Formatage
        datas = JsonUtility.FromJson<DataClass>(dataJson);

        if (datas.tutoFait == true)
        {
            // active les stats et rempli les stats
            stats[0].SetActive(true);
            DisplayTime(datas.tutoTimer, timers[0]);
            morts[0].text = datas.tutoMort.ToString();

            // active le niveau suivant
            boutons[0].SetActive(true);
            cadenas[0].SetActive(false);
        }

        if (datas.niv1Fait == true)
        {
            // active les stats et rempli les stats
            stats[1].SetActive(true);
            DisplayTime(datas.niv1Timer, timers[1]);
            morts[1].text = datas.niv1Mort.ToString();

            // active le niveau suivant
            boutons[1].SetActive(true);
            cadenas[1].SetActive(false);
        }

        if (datas.niv2Fait == true)
        {
            // active les stats et rempli les stats
            stats[2].SetActive(true);
            DisplayTime(datas.niv2Timer, timers[2]);
            morts[2].text = datas.niv2Mort.ToString();

            // active le niveau suivant

        }

    }
    void DisplayTime(float timeToDisplay, TextMeshProUGUI chrono)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        chrono.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

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
