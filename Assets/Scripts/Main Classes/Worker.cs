using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum WorkerType
{
    Farmers,
    Woodcutters,
    Miners
}

public class Worker : MonoBehaviour
{
    public static Dictionary<WorkerType, Worker> Workers = new Dictionary<WorkerType, Worker>();

    [System.NonSerialized] public GameObject objMainPanel;

    public GameObject objSpacerBelow;
    public static uint AvailableWorkerCount;
    public uint ChangeAmount = 1;
    public uint WorkerCount;
    public WorkerType Type;
    public TMP_Text TxtHeader, TxtAvailableWorkers;
    public ResourceType ResourceTypeToModify;
    public float ResourceMultiplier;
    public float AmountToIncreasePerSecondBy;
    public uint IsUnlocked;
    private string _workerString;
    public float tempAmount;


    protected void SetInitialValues()
    {
        InitializeObjects();        
    }
    protected void InitializeObjects()
    {
        objMainPanel = gameObject;

        _workerString = (Type.ToString() + "WorkerCount");

        WorkerCount = (uint)PlayerPrefs.GetInt(_workerString, (int)WorkerCount);
        AvailableWorkerCount = (uint)PlayerPrefs.GetInt("AvailableWorkerCount", (int)AvailableWorkerCount);

        TxtHeader.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
        TxtAvailableWorkers.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);

        if (IsUnlocked == 1)
        {
            objMainPanel.SetActive(true);
            objSpacerBelow.SetActive(true);
        }
        else
        {
            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
        }
    }
    
    public void OnPlusButton()
    {
        // AmountPerSecond = 0;
        // AmountPerSecond += workermultiplier 

        if (AvailableWorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                ChangeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (AvailableWorkerCount < 10)
                {
                    ChangeAmount = AvailableWorkerCount;
                }
                else
                {
                    ChangeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (AvailableWorkerCount < 100)
                {
                    ChangeAmount = AvailableWorkerCount;
                }
                else
                {
                    ChangeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                ChangeAmount = AvailableWorkerCount;
            }
            AvailableWorkerCount -= ChangeAmount;
            WorkerCount += ChangeAmount;
            TxtHeader.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
            TxtAvailableWorkers.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);

            AmountToIncreasePerSecondBy = (ChangeAmount * ResourceMultiplier);
            Resource._resources[ResourceTypeToModify].AmountPerSecond += AmountToIncreasePerSecondBy;
        }       
    }

    public void OnMinusButton()
    {
        if (WorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                ChangeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (WorkerCount < 10)
                {
                    ChangeAmount = WorkerCount;
                }
                else
                {
                    ChangeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (WorkerCount < 100)
                {
                    ChangeAmount = WorkerCount;
                }
                else
                {
                    ChangeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                ChangeAmount = WorkerCount;
            }
            AvailableWorkerCount += ChangeAmount;
            WorkerCount -= ChangeAmount;
            TxtHeader.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
            TxtAvailableWorkers.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);

            AmountToIncreasePerSecondBy = (ChangeAmount * ResourceMultiplier);
            Resource._resources[ResourceTypeToModify].AmountPerSecond -= AmountToIncreasePerSecondBy;
        }     
    }
    private void OnApplicationQuit()
    {             
        PlayerPrefs.SetInt("AvailableWorkerCount", (int)AvailableWorkerCount);
        PlayerPrefs.SetInt(_workerString, (int)WorkerCount);
    }
}