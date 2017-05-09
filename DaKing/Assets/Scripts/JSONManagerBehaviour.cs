using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class JSONManagerBehaviour : MonoBehaviour
{
    public JSONManagerLogic _resourceManagerLogic;
    
    public string jsonDataPath;

    [Header("Events")]
    public UnityEvent onLoaded;
    
    void Awake()
    {
        _resourceManagerLogic = new JSONManagerLogic();
    }

    void Start()
    {
        Setup();

        _resourceManagerLogic.StartLoadingCharacterTextFiles();

    }

    private void Setup()
    {
        _resourceManagerLogic.ResourceManager = this;
        _resourceManagerLogic.JsonDataPath = jsonDataPath; 
    }

    void OnDestroy()
    {

    }

    public void CreateCoroutine(string filePath)
    {
        StartCoroutine(_resourceManagerLogic.LoadWWW(filePath));
    }

    

}
