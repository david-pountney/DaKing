using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class JSONManagerBehaviour : MonoBehaviour
{
    public JSONManagerLogic _jsonManagerLogic;
    
    public string jsonCharacterDataPath;
    public string jsonCharacterOptionDataPath;

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
        _jsonManagerLogic.StartLoadingCharacterOptionFiles();
    }

    private void Setup()
    {
        _jsonManagerLogic.JsonManagerBehaviour = this;
        _jsonManagerLogic.JsonCharacterDataPath = jsonCharacterDataPath;
        _jsonManagerLogic.JsonCharacterOptionDataPath = jsonCharacterOptionDataPath;
    }

    void OnDestroy()
    {

    }

    public void LoadCharacterTextCoroutine(string filePath)
    {
        StartCoroutine(_jsonManagerLogic.LoadCharacterWWW(filePath));
    }

    public void LoadCharacterOptionsTextCoroutine(string filePath)
    {
        StartCoroutine(_jsonManagerLogic.LoadCharacterOptionsWWW(filePath));
    }

}
