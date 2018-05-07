using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    public Text roomNameText;
    public Text roomIntroText;
    
    public Transform playersPanel;
    public Transform editPanel;

    public Button startButton;
    public Button readyButton;
    public Button openEditButton;
    public Button closeEditButton;
    public Button editButton;

    public InputField roomNameInput;
    public InputField roomIntroInput;
    public InputField roomPasswordInput;
    
    private int isReady = 0;
    private bool canPlay = false;

    public void setRoomName(string name)
    {
        Debug.LogFormat("设置房间名：room name:{0}", name);
        roomNameText.text = name;
    }

    public void setRoomIntro(string intro)
    {
        Debug.LogFormat("设置房间简介：room intro:{0}", intro);
        roomIntroText.text = intro;
    }

    public void setSeatsData(List<PLAYER_INFOS> seatsData)
    {
        Debug.LogFormat("setSeatsData::seatsData:{0},count:{1}", seatsData,seatsData.Count);
        this.canPlay = true;
        for (int i = 0; i < seatsData.Count; i++)
        {
            PLAYER_INFOS player = seatsData[i];
            Debug.LogFormat("设置玩家：player:{0}", player.name);
            int seatIndex = int.Parse(player.seatIndex.ToString());
            Transform playerPanel = playersPanel.Find("Player" + (seatIndex + 1));
            //玩家名字
            Transform playerNamePanel = playerPanel.Find("Name");
            Debug.LogFormat("seatIndex:{0}", seatIndex);
            playerNamePanel.GetComponent<Text>().text = player.name;
            //已准备图标
            Transform readyPanel = playerPanel.Find("ReadyPanel");
            Debug.LogFormat("ready[i] = {0}", i, player.isReady);
            if (int.Parse(player.isReady.ToString()) == 0)
            {
                readyPanel.gameObject.SetActive(false);
            }
            else
            {
                readyPanel.gameObject.SetActive(true);
            }
            //房主标识
            Transform roomMasterPanel = playerPanel.Find("RoomMasterPanel");
            Debug.LogFormat("isRoomMaster = {0}", player.isRoomMaster);
            if (int.Parse(player.isRoomMaster.ToString()) == 0)
            {
                roomMasterPanel.gameObject.SetActive(false);
            }
            else
            {
                roomMasterPanel.gameObject.SetActive(true);
            }
            //按钮状态
            Debug.LogFormat("设置按钮状态");
            if(int.Parse(player.id.ToString()) == int.Parse(((Account)Init.Datas["account"]).dbid.ToString()))
            {
                if (int.Parse(player.isRoomMaster.ToString()) == 1)
                {
                    Debug.LogFormat("设置按钮状态：房主");
                    startButton.gameObject.SetActive(true);
                    disableButton(startButton);
                    readyButton.gameObject.SetActive(false);
                    enableButton(openEditButton);
                }
                else
                {
                    Debug.LogFormat("设置按钮状态：玩家");
                    startButton.gameObject.SetActive(false);
                    readyButton.gameObject.SetActive(true);
                    disableButton(openEditButton);
                }
            }
            //是否可以开始游戏
            if (int.Parse(player.isRoomMaster.ToString()) == 1)
            {
                continue;
            }
            if (int.Parse(player.isReady.ToString()) == 0)
            {
                this.canPlay = false;
            }
        }
        if (seatsData.Count == 3 && canPlay)
        {
            enableButton(startButton);
        }
        else
        {
            disableButton(startButton);
        }
    }

    public void readyPlay()
    {
        Debug.LogFormat("请求准备/取消准备");
        KBEngine.Event.fireIn("reqReadyPlay", Init.Datas["roomKey"], isReady);
        Text readyButtonText = readyButton.GetComponentInChildren<Text>();
        if (isReady == 0)
        {
            isReady = 1;
            readyButtonText.text = "取消准备";
        }
        else
        {
            isReady = 0;
            readyButtonText.text = "准备";
        }
    }

    public void playGame()
    {
        Debug.LogFormat("请求开始游戏");
        KBEngine.Event.fireIn("reqPlayGame",Init.Datas["roomKey"]);
    }

    public void setIsPlaying(object isPlaying)
    {
        if (int.Parse(isPlaying.ToString()) == 2)
        {
            Debug.LogFormat("正在开始游戏");
            SceneManager.LoadScene("game");
        }
    }

    public void openEditPanel()
    {
        Debug.LogFormat("打开修改房间信息面板");
        roomNameInput.text = roomNameText.text;
        roomIntroInput.text = roomIntroText.text;
        reqRoomPassword();
        editPanel.gameObject.SetActive(true);
    }

    public void reqRoomPassword()
    {
        Debug.LogFormat("请求房间密码");
        KBEngine.Event.fireIn("reqRoomPassword", Init.Datas["roomKey"]);
    }

    public void closeEditPanel()
    {
        Debug.LogFormat("关闭修改房间信息面板");
        editPanel.gameObject.SetActive(false);
    }

    public void editRoomInfo()
    {
        Debug.LogFormat("修改房间信息,name:{0},intro:{1},password:{2}",roomNameInput.text,roomIntroInput.text,roomPasswordInput.text);
        KBEngine.Event.fireIn("reqEditRoomInfo",Init.Datas["roomKey"], roomNameInput.text, roomIntroInput.text, roomPasswordInput.text);
        closeEditPanel();
    }

    public void reqExitRoom()
    {
        Debug.LogFormat("请求退出房间");
        KBEngine.Event.fireIn("reqExitRoom", Init.Datas["roomKey"]);
    }

    public void onReqRoomPassword(string password)
    {
        Debug.LogFormat("正在请求房间密码:{0}", password);
        roomPasswordInput.text = password;
    }

    public void onReqExitRoom(UInt64 entityId)
    {
        Debug.LogFormat("正在退出房间,entityId:{0}", entityId);
        Init.Datas.Remove("roomKey");
        SceneManager.LoadScene("hall");
    }

    public void onReqPlayGame()
    {
        Debug.LogFormat("正在开始游戏");
        SceneManager.LoadScene("game");
    }

    public void onExitRoom(Account account)
    {
        Debug.LogFormat("正在离开房间");
        SceneManager.LoadScene("hall");
    }

    public void onDisconnected()
    {
        KBEngineApp.app.reset();
        Init.Datas.Clear();
        SceneManager.LoadScene("login");
    }

    public void onLeaveWorld(string name)
    {
        SceneManager.LoadScene("Hall");
    }

    public void disableButton(Button button)
    {
        button.enabled = false;
        Image image = button.GetComponent<Image>();
        Color color = new Color32(150, 150, 150, 255);
        image.color = color;
        Debug.LogFormat("禁用按钮,button name:{0},color:{1}", button.name, color);
    }

    public void enableButton(Button button)
    {
        button.enabled = true;
        Image image = button.GetComponent<Image>();
        Color color = new Color32(255, 255, 255, 255);
        image.color = color;
        Debug.LogFormat("启用按钮,button name:{0},color:{1}", button.name, color);
    }

    // Use this for initialization
    void Start ()
    {
        KBEngine.Event.registerOut("onReqRoomPassword", this, "onReqRoomPassword");
        KBEngine.Event.registerOut("onReqExitRoom", this, "onReqExitRoom");
        //KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        KBEngine.Event.registerOut("onReqPlayGame", this, "onReqPlayGame");
        KBEngine.Event.registerOut("setRoomName", this, "setRoomName");
        KBEngine.Event.registerOut("setRoomIntro", this, "setRoomIntro");
        KBEngine.Event.registerOut("setSeatsData", this, "setSeatsData");
        KBEngine.Event.registerOut("setIsPlaying", this, "setIsPlaying");
        KBEngine.Event.registerOut("onExitRoom", this, "onExitRoom");

        KBEngine.Event.fireIn("reqEnterWorld");
        KBEngine.Event.fireIn("reqRoomInfo");
        roomPasswordInput.inputType = InputField.InputType.Password;

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
