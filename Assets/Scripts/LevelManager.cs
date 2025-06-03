using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    // refs
    public TextMeshProUGUI chrono;
    public TextMeshProUGUI morts;
    public int nbrMorts;
    public float timer;
    public GameObject popUpAbandon;
    public GameObject UILevel;
    public string pathMenu;

    // variables
    public DataClass datas = new DataClass();       // génération d'un DataClass pour y stocker nos données
    private string fileUrl;                         // Chemin (URL) vers le fichier de sauvegarde
    private Scene scene;

    void Start()
    {
        // Nom de la scene dans laquelle se trouve l'objet
        scene = this.gameObject.scene;

        // initialisation
        timer = 0;
        nbrMorts = 0;

        // Préparation du chemin vers le fichier
        fileUrl = Application.persistentDataPath + "/save.json";
        // Charge les données dans le manager
        LoadGame();

    }

    // Update is called once per frame
    void Update()
    {
        // CHRONO
        timer += Time.deltaTime;
        DisplayTime(timer);

        // COMPTEUR DE MORT
        morts.text = nbrMorts.ToString();

        // Abandon et retour au menu principal
        if (Input.GetButtonDown("Abandon")) UIAbandon();
    }

    void UIAbandon()
    {
        popUpAbandon.SetActive(true);
        Time.timeScale = 0f;
        UILevel.SetActive(false);
    }

    public void Abandon()
    {
        SaveGame();
        SceneManager.LoadScene(pathMenu);
    }

    public void UIQuit()
    {
        popUpAbandon.SetActive(false);
        Time.timeScale = 1f;
        UILevel.SetActive(true);
    }


    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        chrono.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void LoadGame()
    {
        // Lecture dans le fichier
        string dataJson = File.ReadAllText(fileUrl);
        // Formatage
        datas = JsonUtility.FromJson<DataClass>(dataJson);
    }

    void SaveGame()
    {
        if (scene.name == "NiveauTuto") datas.tutoMort += nbrMorts;

        if (scene.name == "Niveau1") datas.niv1Mort += nbrMorts;

        if (scene.name == "Niveau2") datas.niv2Mort += nbrMorts;

        // Formatage (JSON)
        string dataJson = JsonUtility.ToJson(datas, true);

        // Ecriture dans le fichier
        File.WriteAllText(fileUrl, dataJson);
    }
}
