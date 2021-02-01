﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public bool xOne, xTen, xHundred, xMax;
    public int increaseAmount, increaseAmountCache, woodcutterWorkerAmount, minerWorkerAmount, farmerWorkerAmount, workerAmountToMinus;
    public GameObject woodcutterText, minerText, farmerText;
    public Animator toggleAnim;

    public void Start()
    {
        increaseAmount = 1;
    }

    public void Button1()
    {
        xOne = true;
        xTen = false;
        xHundred = false;
        xMax = false;

        increaseAmount = 1;
        toggleAnim.SetTrigger("1Idle");
    }

    public void Button10()
    {
        toggleAnim.SetTrigger("2Anim");
        xOne = false;
        xTen = true;
        xHundred = false;
        xMax = false;

        increaseAmount = 10;

    }

    public void Button100()
    {
        if (xOne == true)
        {
            toggleAnim.SetTrigger("2to3Anim");
        }
        else if (xTen == true)
        {
            toggleAnim.SetTrigger("2Idle3Anim");
        }
        else
        {
            toggleAnim.SetTrigger("2Idle3Anim");
        }

        xOne = false;
        xTen = false;
        xHundred = true;
        xMax = false;

        increaseAmount = 100;

    }

    public void ButtonMax()
    {
        if (xOne == true)
        {
            toggleAnim.SetTrigger("2to4Anim");
        }
        else if (xTen == true)
        {
            toggleAnim.SetTrigger("2Idle3to4Anim");
        }
        else if (xHundred == true)
        {
            toggleAnim.SetTrigger("23Idle4Anim");
        }

        xOne = false;
        xTen = false;
        xHundred = false;
        xMax = true;

        increaseAmount = GameManager.Instance.availableWorkers;
    }
    private void WorkerButtonPlus(ref int thisWorkerAmouunt, GameObject thisWorkerObject, string thisWorkerName)
    {
        increaseAmountCache = increaseAmount;

        if ((xMax) || (increaseAmountCache > GameManager.Instance.availableWorkers))
        {
            increaseAmountCache = GameManager.Instance.availableWorkers;
            GameManager.Instance.availableWorkers -= increaseAmountCache;
            thisWorkerAmouunt += increaseAmountCache;
        }

        else if (increaseAmount <= GameManager.Instance.availableWorkers)
        {
            GameManager.Instance.availableWorkers -= increaseAmountCache;
            thisWorkerAmouunt += increaseAmountCache;
        }
        else
        {
            Debug.Log("Catch all");
        }
        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        thisWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: [{1}]", thisWorkerName, thisWorkerAmouunt);

        Debug.Log(string.Format("{0}, {1}, {2}", thisWorkerAmouunt, thisWorkerObject, thisWorkerName));
    }

    private void WorkerButtonMinus(ref int thisWorkerAmouunt, GameObject thisWorkerObject, string thisWorkerName)
    {
        increaseAmountCache = increaseAmount;

        if ((increaseAmountCache > thisWorkerAmouunt) || (xMax))
        {
            workerAmountToMinus = thisWorkerAmouunt;
            thisWorkerAmouunt -= workerAmountToMinus;
            GameManager.Instance.availableWorkers += workerAmountToMinus;
        }

        else if (increaseAmount <= thisWorkerAmouunt)
        {
            thisWorkerAmouunt -= increaseAmountCache;
            GameManager.Instance.availableWorkers += increaseAmountCache;
        }

        else
        {
            Debug.Log("Catch all");
        }

        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        thisWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: [{1}]", thisWorkerName, thisWorkerAmouunt);
    }
    public void ButtonWoodcutterPlus()
    {
        WorkerButtonPlus(ref woodcutterWorkerAmount, woodcutterText, "Woodcutters");
    }
    public void ButtonWoodcutterMinus()
    {
        WorkerButtonPlus(ref woodcutterWorkerAmount, woodcutterText, "Woodcutters");
    }
    public void ButtonMinerPlus()
    {
        WorkerButtonPlus(ref minerWorkerAmount, minerText, "Miners");
    }

    public void ButtonMinerMinus()
    {
        WorkerButtonMinus(ref minerWorkerAmount, minerText, "Miners");
    }

    public void ButtonFarmerPlus()
    {
        WorkerButtonPlus(ref farmerWorkerAmount, farmerText, "Farmers");
    }

    public void ButtonFarmerMinus()
    {
        WorkerButtonMinus(ref farmerWorkerAmount, farmerText, "Farmers");
    }
}

