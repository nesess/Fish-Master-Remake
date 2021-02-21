using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private GameObject currentScreen;

    public GameObject endScreen;
    public GameObject gameScreen;
    public GameObject mainScreen;
    public GameObject returnScreen;

    public Button lengthButton;
    public Button strengthButton;
    public Button offlineButton;

    public Text gameScreenMoney;

    public Text lengthCostText;
    public Text lengthValueText;

    public Text strengthCostText;
    public Text strengthValueText;
    public Text offlineCostText;
    public Text offlineValueText;
    public Text endScreenMoney;
    public Text returnScreenMoney;

    private int gameCount;


    private void Awake()
    {
        if(GameManager.instance)
        {
            Destroy(base.gameObject);
        }
        else
        {
            GameManager.instance = this;
        }
        currentScreen = mainScreen;
    }

    void Start()
    {
        checkIdles();
        updateTexts();
        
    }

    public void changeScreen(Screens screen)
    {
        

        currentScreen.SetActive(false);
        switch(screen)
        {
            case Screens.MAIN:
                currentScreen = mainScreen;
                updateTexts();
                checkIdles();
                break;

            case Screens.GAME:
                currentScreen = gameScreen;
                gameCount++;
                break;

            case Screens.END:
                currentScreen = endScreen;
                setEndScreenMoney();
                break;

            case Screens.RETURN:
                currentScreen = returnScreen;
                setReturnScreenMoney();
                break;
        }
        currentScreen.SetActive(true);
    }

    public void setEndScreenMoney()
    {
        endScreenMoney.text = "$" + IdleManager.instance.totalGain;
    }

    public void setReturnScreenMoney()
    {
        returnScreenMoney.text = "$" + IdleManager.instance.totalGain + "gained while waiting!";
    }


    public void updateTexts()
    {
        gameScreenMoney.text = "$" + IdleManager.instance.wallet;

        lengthCostText.text = "$" + IdleManager.instance.lengthCost;
        lengthValueText.text = -IdleManager.instance.length + "m";

        strengthCostText.text = "$" + IdleManager.instance.strengthCost;
        strengthValueText.text = IdleManager.instance.strength + "fishes";

        offlineCostText.text = "$" + IdleManager.instance.offlineMoneyCost;
        offlineValueText.text = "$" + IdleManager.instance.offlineMoney + "/min";
        
    }

    public void checkIdles()
    {
        int lengthCost = IdleManager.instance.lengthCost;
        int length = IdleManager.instance.length;
        int strengthCost = IdleManager.instance.strengthCost;
        int offlineCost = IdleManager.instance.offlineMoneyCost;
        int wallet = IdleManager.instance.wallet;

        if (wallet >= lengthCost  && length > -310)
            lengthButton.interactable = true;
        else if(length <= -310)
        {
            lengthValueText.text = "MAX";
            lengthButton.interactable = false;
        }
        else
            lengthButton.interactable = false;

        if (wallet >= strengthCost)
            strengthButton.interactable = true;
        else
            strengthButton.interactable = false;

        if (wallet >= offlineCost)
            offlineButton.interactable = true;
        else
            offlineButton.interactable = false;
    }

}
