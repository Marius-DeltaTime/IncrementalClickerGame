using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Knowledge : Resource
{
    private Resource _resource;

    private void Awake()
    {
        _resource = GetComponent<Resource>();
        _resources.Add(Type, _resource);
        PlayerPrefs.SetInt(_isUnlockedString, IsUnlocked);
        SetInitialValues();
        // DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<ResourceType, Resource> kvp in _resources)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    private void Update()
    {
        UpdateResources();
    }
}