using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tin : Resource
{
    private Resource _resource;

    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        SetInitialValues();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<ResourceType, Resource> kvp in Resources)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    void Update()
    {
        UpdateResources();
    }
}
