using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountInfoController : MonoBehaviour
{
    public Transform accountInfoPanel;

    public void setAccountInfo()
    {
        AccountBase account = (AccountBase)Init.Datas["account"];


        string name = account.name;
        int winCount = account.winCount;
        int loseCount = account.loseCount;
        int countSum = winCount + loseCount;
        double winRate;
        if (countSum == 0)
        {
            winRate = Math.Round(100.00,2);
        }
        else
        {
            winRate = Math.Round(100.00 * winCount / countSum, 2);
        }
        Debug.LogFormat("name:{0},winCount:{1},loseCount:{2},winRate:{3}",name,winCount,loseCount,winRate);
        Transform namePanel = accountInfoPanel.Find("Name");
        namePanel.GetComponent<Text>().text = name;
        Transform winCountPanel = accountInfoPanel.Find("WinCount");
        winCountPanel.GetComponent<Text>().text = "胜利" + winCount;
        Transform loseCountPanel = accountInfoPanel.Find("LoseCount");
        loseCountPanel.GetComponent<Text>().text = "失败" + loseCount;
        Transform winRatePanel = accountInfoPanel.Find("WinRate");
        winRatePanel.GetComponent<Text>().text = "胜率" + winRate + "%";
    }

    // Use this for initialization
    void Start () {

        setAccountInfo();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
