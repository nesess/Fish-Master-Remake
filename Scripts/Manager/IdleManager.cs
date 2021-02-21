using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.SceneManagement;


public class IdleManager : MonoBehaviour, IUnityAdsListener
{
    [HideInInspector]
    public int length;
    [HideInInspector]
    public int lengthCost;
    [HideInInspector]
    public float lengthCostMulti;

    [HideInInspector]
    public int strength;
    [HideInInspector]
    public int strengthCost;
    [HideInInspector]
    public float strengthCostMulti;

    [HideInInspector]
    public int offlineMoney;
    [HideInInspector]
    public int offlineMoneyCost;
    [HideInInspector]
    public float offlineCostMulti;

    [HideInInspector]
    public int wallet;

    [HideInInspector]
    public int totalGain;

    string gameId = "4013377";
    string placement = "rewardedVideo";
    bool testMode = false;




    public static IdleManager instance;

    private void Awake()
    {
        if(IdleManager.instance)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        else
        {
            IdleManager.instance = this;
        }
        length = -PlayerPrefs.GetInt("Length", 30);
        lengthCost = PlayerPrefs.GetInt("LengthCost",20);
        lengthCostMulti = PlayerPrefs.GetFloat("LengthCostMulti", 1.2f);

        strength = PlayerPrefs.GetInt("Strength", 1);
        strengthCost = PlayerPrefs.GetInt("StrengthCost", 30);
        strengthCostMulti = PlayerPrefs.GetFloat("StrengthCostMulti", 1.2f);

        offlineMoney = PlayerPrefs.GetInt("Offline", 3);
        offlineMoneyCost = PlayerPrefs.GetInt("OfflineCost", 10);
        offlineCostMulti = PlayerPrefs.GetFloat("OfflineCostMulti", 1.2f);

        wallet = PlayerPrefs.GetInt("Wallet", 0);
       
    }

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date",string.Empty);
            if(@string != string.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineMoney + 1);
                GameManager.instance.changeScreen(Screens.RETURN);
            }
            else
            {
                GameManager.instance.changeScreen(Screens.GAME);
                GameManager.instance.changeScreen(Screens.MAIN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void buyLength()
    {
        if(wallet >= lengthCost)
        {
            length -= 10;
            PlayerPrefs.SetInt("Length", -length);

            wallet -= lengthCost;
            PlayerPrefs.SetInt("Wallet", wallet);

            lengthCost = (int)(lengthCost * lengthCostMulti);
            PlayerPrefs.SetInt("LengthCost", lengthCost);

            lengthCostMulti += 0.10f;
            PlayerPrefs.SetFloat("LengthCostMulti", lengthCostMulti);

            GameManager.instance.changeScreen(Screens.MAIN);

        }
        
    }

    public void buySrength()
    {
        if (wallet >= strengthCost)
        {
            strength++;
            PlayerPrefs.SetInt("Strength", strength);

            wallet -= strengthCost;
            PlayerPrefs.SetInt("Wallet", wallet);

            strengthCost = (int)(strengthCost * strengthCostMulti);
            PlayerPrefs.SetInt("StrengthCost", strengthCost);

            strengthCostMulti += 0.20f;
            PlayerPrefs.SetFloat("StrengthCostMulti", strengthCostMulti);
            
            GameManager.instance.changeScreen(Screens.MAIN);


        }

    }

    public void buyOfflineMoney()
    {
        if (wallet >= offlineMoneyCost)
        {
            offlineMoney++;
            PlayerPrefs.SetInt("Offline", offlineMoney);

            wallet -= offlineMoneyCost;
            PlayerPrefs.SetInt("Wallet", wallet);

            offlineMoneyCost = (int)(offlineMoneyCost * offlineCostMulti);
            PlayerPrefs.SetInt("OfflineCost", offlineMoneyCost);

            offlineCostMulti += 0.10f;
            PlayerPrefs.SetFloat("OfflineCostMulti", offlineCostMulti);

            GameManager.instance.changeScreen(Screens.MAIN);

        }
    }

    public void collectMoney()
    {
        wallet += totalGain;
        PlayerPrefs.SetInt("Wallet", wallet);
        GameManager.instance.changeScreen(Screens.GAME);
        GameManager.instance.changeScreen(Screens.MAIN);
    }

    public void ShowAd()
    {
        Advertisement.Show(placement);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Finished:
                
                Debug.Log("başarılı");
                
                break;
            case ShowResult.Skipped:
                Debug.Log("You skipped ad only 1x money awarded to you");
                break;
            case ShowResult.Failed:
                Debug.Log("Ad video failed to play");
                break;
        }
    }

    public void collectMoney2x()
    {
        ShowAd();
        wallet += (2 * totalGain);
        PlayerPrefs.SetInt("Wallet", wallet);
        GameManager.instance.changeScreen(Screens.GAME);
        GameManager.instance.changeScreen(Screens.MAIN);

    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }
}
