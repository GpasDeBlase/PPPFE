using System.Collections;
using UnityEngine;

public class lifeTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Dies());
        
    }

    IEnumerator Dies()
    {
        Destroy(gameObject, 2);
        yield return null;
    }
}
