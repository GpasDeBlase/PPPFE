using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // refs
    public TextMeshProUGUI chrono;
    public TextMeshProUGUI morts;
    public float nbrMorts;

    // variables
    float timer; 

    void Start()
    {
        timer = 0;
        nbrMorts = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        // CHRONO
        timer += Time.deltaTime;
        DisplayTime(timer);

        // COMPTEUR DE MORT
        morts.text = nbrMorts.ToString(); 
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        chrono.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
