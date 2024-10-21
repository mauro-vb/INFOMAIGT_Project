using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDataManager : MonoBehaviour
{
    public static QDataManager Instance;

    public string SceneName;    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
