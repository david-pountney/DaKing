using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class JSONManagerBehaviour : MonoBehaviour
{
    public JSONManagerLogic _jsonManagerLogic;
    
    public string jsonDataPath;

    [Header("Events")]
    public UnityEvent onLoaded;
    
    void Awake()
    {
        _jsonManagerLogic = new JSONManagerLogic();
    }

    void Start()
    {
        Setup();

        _jsonManagerLogic.StartLoadingCharacterTextFiles();
    }

    private void Setup()
    {
        _jsonManagerLogic.JsonManagerBehaviour = this;
        _jsonManagerLogic.JsonDataPath = jsonDataPath; 
    }

    void OnDestroy()
    {

    }

    public void CreateCoroutine(string filePath)
    {
        StartCoroutine(_jsonManagerLogic.LoadWWW(filePath));
    }

    

}
