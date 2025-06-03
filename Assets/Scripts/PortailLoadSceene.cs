using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using System.IO;
using UnityEngine.UIElements;

public class PortailLoadSceene : MonoBehaviour
{
    // references
    public string pathMenu;                         // menu
    public LevelManager levelManager;               // level manager


    // variables
    public DataClass datas = new DataClass();       // g�n�ration d'un DataClass pour y stocker nos donn�es
    private string fileUrl;                         // Chemin (URL) vers le fichier de sauvegarde
    private Scene scene;

    void Start()
    {
        // Pr�paration du chemin vers le fichier
        fileUrl = Application.persistentDataPath + "/save.json";

        // Nom de la scene dans laquelle se trouve l'objet
        scene = this.gameObject.scene;

        // Charge les donn�es dans le portail
        LoadGame();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            SaveGame();
            SceneManager.LoadScene(pathMenu);
        }
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
        if (scene.name == "NiveauTuto")
        {
            if (datas.tutoFait == false)
            {
                datas.tutoFait = true;                                 // si premi�re compl�tion indique qu'il est fait d�sormais
                datas.tutoTimer = levelManager.timer;                  // enregistrement du premier chrono
            }

            if ((datas.tutoFait == true) && (levelManager.timer < datas.tutoTimer)) datas.tutoTimer = levelManager.timer;     // si le temps est meilleur que celui enregistr�, le remplace

            datas.tutoMort += levelManager.nbrMorts;                                            // ajoute les morts au compteur total 

        }

        if (scene.name == "Niveau1")
        {
            if (datas.niv1Fait == false)
            {
                datas.niv1Fait = true;                                 // si premi�re compl�tion indique qu'il est fait d�sormais
                datas.niv1Timer = levelManager.timer;                  // enregistrement du premier chrono
            }

            if ((datas.tutoFait == true) && (levelManager.timer < datas.niv1Timer)) datas.niv1Timer =levelManager.timer;     // si le temps est meilleur que celui enregistr�, le remplace

            datas.niv1Mort += levelManager.nbrMorts;                                            // ajoute les morts au compteur total 

        }

        if (scene.name == "Niveau2")
        {
            if (datas.niv2Fait == false)
            {
                datas.niv2Fait = true;                                 // si premi�re compl�tion indique qu'il est fait d�sormais
                datas.niv2Timer = levelManager.timer;                 // enregistrement du premier chrono
            }

            if ((datas.tutoFait == true) && (levelManager.timer < datas.niv2Timer)) datas.niv2Timer = levelManager.timer;     // si le temps est meilleur que celui enregistr�, le remplace

            datas.niv2Mort += levelManager.nbrMorts;                                            // ajoute les morts au compteur total 

        }

        // Formatage (JSON)
        string dataJson = JsonUtility.ToJson(datas, true);

        // Ecriture dans le fichier
        File.WriteAllText(fileUrl, dataJson);
    }
}
