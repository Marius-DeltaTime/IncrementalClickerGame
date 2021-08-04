using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResearchType
{
    // Housing should maybe be after you have the 'Log' resource.
    // Hamster Wheel
    // Fire?
    // Grinding stone/Sharpening of tools (Increases production of everything that uses tools, or maybe just increase worker values.
    // Cooking of food.
    // Clothing, need to research first and then craft perhaps, this might go for a lot of things.
    // Power Lines, enables buildings to receive power.
    // Maybe have knowledge points as the currency to research things.
    // Maybe if you get x amount of workers. Make an event happen where you get someone who is quite smart. and this person gives you an instant 50 knowledge points. Which is just enouggh
    // to research paper, then after researching paper you can have students that study papers. 
    // And with that you'll gain knowledge.
    // Smelting
    // Need to probably unlock researchables through the same formula as unlocking crafting stuff. Except based on knowledge.

    //Paper,
    Weapons,
    StoneEquipment,
    Fire,
    Cooking,
    FireHardenedWeapons,
    Smelting,
    ManualEnergyProduction
}

public abstract class Researchable : SuperClass
{
    
    public static int researchSimulActive = 0, researchSimulAllowed = 1;
   

    public ResearchType Type;

    public float secondsToCompleteResearch;

    [System.NonSerialized] public bool isResearched;

    private bool isResearchStarted, _isIncrementedViaResources;
    private string _stringIsResearched, _stringResearchTimeRemaining, _stringIsResearchStarted, _stringHeader;
    private float _currentTimer, _researchTimeRemaining;
    protected Transform _tformImgResearchBar, _tformImgProgressCircle;
    protected Image _imgProgressCircle, _imgResearchBar;
    protected WorkerType[] _workerTypesToModify;

    void OnValidate()
    {
        if (typesToModify.buildingTypesToModify.Length != 0)
        {
            typesToModify.isModifyingBuilding = true;
        }
        else
        {
            typesToModify.isModifyingBuilding = false;
        }

        if (typesToModify.craftingTypesToModify.Length != 0)
        {
            typesToModify.isModifyingCrafting = true;
        }
        else
        {
            typesToModify.isModifyingCrafting = false;
        }

        if (typesToModify.researchTypesToModify.Length != 0)
        {
            typesToModify.isModifyingResearch = true;
        }
        else
        {
            typesToModify.isModifyingResearch = false;
        }

        if (typesToModify.workerTypesToModify.Length != 0)
        {
            typesToModify.isModifyingWorker = true;
        }
        else
        {
            typesToModify.isModifyingWorker = false;
        }

        if (typesToModify.resourceTypesToModify.Length != 0)
        {
            typesToModify.isModifyingResource = true;
        }
        else
        {
            typesToModify.isModifyingResource = false;
        }
    }
    public void SetInitialValues()
    {
        InitializeObjects();
        //isUnlocked = true;

        if (TimeManager.hasPlayedBefore)
        {
            isResearchStarted = PlayerPrefs.GetInt(_stringIsResearchStarted) == 1 ? true : false;
            isResearched = PlayerPrefs.GetInt(_stringIsResearched) == 1 ? true : false;
            _researchTimeRemaining = PlayerPrefs.GetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
        }

        if (!isResearched && isResearchStarted)
        {
            if (_researchTimeRemaining <= TimeManager.difference.TotalSeconds)
            {
                isResearchStarted = false;
                isResearched = true;
                Debug.Log("Research was completed while you were gone");
                Researched();
            }
            else
            {
                secondsToCompleteResearch = _researchTimeRemaining - (float)TimeManager.difference.TotalSeconds;
                Debug.Log("You still have ongoing research");
                _objProgressCircle.SetActive(false);
            }
        }

        else if (isResearched && !isResearchStarted)
        {
            Researched();
        }

    }
    public virtual void UpdateResearchTimer()
    {
        if (isResearchStarted)
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = _maxValue;

                _currentTimer += 0.1f;

                _imgResearchBar.fillAmount = _currentTimer / secondsToCompleteResearch;
                _researchTimeRemaining = secondsToCompleteResearch - _currentTimer;
                TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_researchTimeRemaining)));

                if (span.Days == 0 && span.Hours == 0 && span.Minutes == 0)
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%s}s</b>)", _stringHeader, span.Duration());
                }
                else if (span.Days == 0 && span.Hours == 0)
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%m}m {1:%s}s</b>)", _stringHeader, span.Duration());
                }
                else if (span.Days == 0)
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%h}h {1:%m}m {1:%s}s</b>)", _stringHeader, span.Duration());
                }
                else
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%d}d {1:%h}h {1:%m}m {1:%s}s</b>)", _stringHeader, span.Duration());
                }
                CheckIfResearchIsComplete();
            }
        }

    }
    public void OnResearch()
    {
        if (researchSimulActive >= researchSimulAllowed)
        {
            Debug.Log(string.Format("You can only have {0} research active at the same time", researchSimulAllowed));
        }
        else
        {
            if (!isResearchStarted && !isResearched)
            {

                bool canPurchase = true;

                for (int i = 0; i < resourceCost.Length; i++)
                {
                    if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
                    {
                        canPurchase = false;
                        break;
                    }
                }

                if (canPurchase)
                {
                    for (int i = 0; i < resourceCost.Length; i++)
                    {
                        Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                    }
                    StartResearching();
                }
            }
        }


    }
    private void CheckIfResearchIsComplete()
    {
        if (_currentTimer >= secondsToCompleteResearch)
        {
            isResearchStarted = false;
            isResearched = true;
            Researched();
        }
    }
    private void CheckIfUnlockedByResource()
    {
        if (!isUnlocked)
        {
            if (GetCurrentFill() >= 0.8f)
            {
                if (isUnlockableByResource && !_isIncrementedViaResources)
                {
                    unlockAmount++;
                    _isIncrementedViaResources = true;

                    if (unlockAmount == unlocksRequired)
                    {
                        isUnlocked = true;

                        if (UIManager.isResearchVisible)
                        {
                            objMainPanel.SetActive(true);
                            objSpacerBelow.SetActive(true);
                            hasSeen = true;
                        }
                        else
                        {
                            isUnlockedEvent = true;
                            hasSeen = false;
                            PointerNotification.rightAmount++;
                        }

                        PointerNotification.HandleRightAnim();
                    }
                }
            }
        }
    }
    private void Researched()
    {
        isResearched = true;
        if (Menu.isResearchHidden)
        {
            researchSimulActive--;
            StaticMethods.UnlockCrafting(typesToModify.isModifyingCrafting, typesToModify.craftingTypesToModify);
            StaticMethods.UnlockBuilding(typesToModify.isModifyingBuilding, typesToModify.buildingTypesToModify);
            StaticMethods.UnlockResearchable(typesToModify.isModifyingResearch, typesToModify.researchTypesToModify);
            _objProgressCircle.SetActive(false);
            _objTxtHeader.SetActive(false);
            _objTxtHeaderUncraft.SetActive(true);

            string htmlValue = "#D4D4D4";

            if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
            {
                _imgExpand.color = greyColor;
                _imgCollapse.color = greyColor;
            }

            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
        }
        else
        {
            researchSimulActive--;
            StaticMethods.UnlockCrafting(typesToModify.isModifyingCrafting, typesToModify.craftingTypesToModify);
            StaticMethods.UnlockBuilding(typesToModify.isModifyingBuilding, typesToModify.buildingTypesToModify);
            StaticMethods.UnlockResearchable(typesToModify.isModifyingResearch, typesToModify.researchTypesToModify);
            _objProgressCircle.SetActive(false);
            _objTxtHeader.SetActive(false);
            _objTxtHeaderUncraft.SetActive(true);

            string htmlValue = "#D4D4D4";

            if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
            {
                _imgExpand.color = greyColor;
                _imgCollapse.color = greyColor;
            }
        }

    }
    private void MakeResearchableAgain()
    {
        // This will probably only happen after prestige.
        _objBtnMain.GetComponent<Button>().interactable = true;
        _objProgressCircle.SetActive(true);
        _objTxtHeader.SetActive(true);
        _objTxtHeaderUncraft.SetActive(false);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _imgExpand.color = darkGreyColor;
            _imgCollapse.color = darkGreyColor;
        }
    }
    protected override void InitializeObjects()
    {
        _tformBody = transform.Find("Panel_Main/Body");

        #region Prefab Initializion

        _prefabResourceCost = Resources.Load<GameObject>("ResourceCost_Prefab/ResourceCost_Panel");
        _prefabBodySpacer = Resources.Load<GameObject>("ResourceCost_Prefab/Body_Spacer");

        for (int i = 0; i < resourceCost.Length; i++)
        {
            GameObject newObj = Instantiate(_prefabResourceCost, _tformBody);

            //This loop just makes sure that there is a never a body spacer underneath the last element(the last resource cost panel)
            for (int spacerI = i + 1; spacerI < resourceCost.Length; spacerI++)
            {
                Instantiate(_prefabBodySpacer, _tformBody);
            }

            Transform _tformNewObj = newObj.transform;
            Transform _tformCostName = _tformNewObj.Find("Cost_Name_Panel/Text_CostName");
            Transform _tformCostAmount = _tformNewObj.Find("Cost_Amount_Panel/Text_CostAmount");

            resourceCost[i].uiForResourceCost.textCostName = _tformCostName.GetComponent<TMP_Text>();
            resourceCost[i].uiForResourceCost.textCostAmount = _tformCostAmount.GetComponent<TMP_Text>();
        }

        #endregion

        _tformImgResearchBar = transform.Find("Panel_Main/Header_Panel/Research_FillBar");
        _tformDescription = transform.Find("Panel_Main/Body/Description_Panel/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        _tformImgProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformTxtHeaderUncraft = transform.Find("Panel_Main/Header_Panel/Text_Header_Done");
        _tformExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        _tformCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformObjMain = transform.Find("Panel_Main");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");

        _imgProgressCircle = _tformImgProgressCircle.GetComponent<Image>();
        _imgResearchBar = _tformImgResearchBar.GetComponent<Image>();
        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _objTxtHeader = _tformTxtHeader.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _objProgressCircle = _tformObjProgressCircle.gameObject;
        _objTxtHeaderUncraft = _tformTxtHeaderUncraft.gameObject;
        _imgExpand = _tformExpand.GetComponent<Image>();
        _imgCollapse = _tformCollapse.GetComponent<Image>();
        objMainPanel = _tformObjMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;
        _stringHeader = _objTxtHeader.GetComponent<TMP_Text>().text;

        _objBtnExpand.GetComponent<Button>().onClick.AddListener(OnExpandCloseAll);

        _stringIsResearched = Type.ToString() + "isCrafted";
        _stringResearchTimeRemaining = Type.ToString() + "ResearchTimeRemaining";
        _stringIsResearchStarted = Type.ToString() + "IsResearchStarted";
    }
    public void GetTimeToCompleteResearch()
    {
        isResearchStarted = true;
        DateTime currentTime = DateTime.Now;
        //Debug.Log(currentTime);
        DateTime timeToCompletion = currentTime.AddSeconds(60);
        //Debug.Log(timeToCompletion);
        TimeSpan differenceAmount = timeToCompletion.Subtract(currentTime);
        //Debug.Log(differenceAmount + " " + differenceAmount.Seconds);
        secondsToCompleteResearch = differenceAmount.Seconds;
    }
    public void StartResearching()
    {
        researchSimulActive++;
        isResearchStarted = true;
        _objProgressCircle.SetActive(false);
    }
    protected void SetDescriptionText(string description)
    {
        Researchables[Type]._txtDescription.text = string.Format("{0}", description);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchStarted, isResearchStarted ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsResearched, isResearched ? 1 : 0);
        PlayerPrefs.SetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
    }
}

