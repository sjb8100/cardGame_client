using KBEngine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HallController : MonoBehaviour {
    public Transform canvas;

    public GameObject createRoomPanel;
    public Transform roomListPanel;
    public GameObject passwordInputPanel;

    public InputField createRoomNameInput;
    public InputField createRoomIntroInput;
    public InputField createRoomPasswordInput;

    public InputField selectNameInput;
    public Dropdown isFullDropdown;
    public Dropdown isPlayingDropdown;
    public Dropdown hasPasswordDropdown;

    public InputField joinRoomPasswordInput;
    public Button submitPasswordJoinButton;

    public Button createButton;
    public Button joinButton;

    private float createCountDown;
    private float joinCountDown;
    private float submitPasswordJoinCountDown;
    
    private GameObject chooseRoom;



    public void selectRoomsByParams()
    {
        Debug.LogFormat("查找所有符合条件的房间，nameOrIntro:{0},isFull:{1},isPlaying:{2},hasPassword:{3}", selectNameInput.text, isFullDropdown.value, isPlayingDropdown.value, hasPasswordDropdown.value);
        KBEngine.Event.fireIn("reqRoomsByParams", selectNameInput.text, isFullDropdown.value, isPlayingDropdown.value, hasPasswordDropdown.value);
    }

    public void resetParams()
    {
        selectNameInput.text = "";
        isFullDropdown.value = 0;
        isPlayingDropdown.value = 0;
        hasPasswordDropdown.value = 0;
    }

    public void openCreateRoomPanel()
    {
        Debug.Log("打开创建房间面板");
        createRoomPanel.SetActive(true);
    }

    public void closeCreateRoomPanel()
    {
        Debug.Log("关闭创建房间面板");
        createRoomPanel.SetActive(false);
    }

    public void createRoom()
    {
        disableButton(createButton);
        createCountDown = 1;
        Debug.LogFormat("准备创建房间,name:{0},intro:{1},password:{2}", createRoomNameInput.text, createRoomIntroInput.text, createRoomPasswordInput.text);
        KBEngine.Event.fireIn("reqCreateRoom", createRoomNameInput.text, createRoomIntroInput.text, createRoomPasswordInput.text);
    }

    public void onReqJoinRoomSuccess(object roomKey)
    {
        Debug.LogFormat("正在进入房间,roomKey:{0}",roomKey);
        Init.Datas["roomKey"] = roomKey;
        //KBEngine.Event.fireIn("reqJoinRoom", roomKey);
        SceneManager.LoadScene("Room");
    }

    public void onReqRoomsByParams(List<ROOM_INFOS> roomList)
    {
        //移除原列表
        for (int i = 0; i < roomListPanel.transform.childCount; i++)
        {
            Destroy(roomListPanel.transform.GetChild(i).gameObject);
        }

        Dictionary<string, ROOM_INFOS> roomsInfo = (Dictionary<string, ROOM_INFOS>)Init.Datas["roomsInfo"];
        roomsInfo.Clear();
        

        for(int i = 0;i < roomList.Count; i++)
        {
            ROOM_INFOS room = roomList[i];
            roomsInfo.Add(room.roomKey+"", room);
            Debug.LogFormat("room name:{0},intro:{1},playerCount:{2},isPlaying:{3},hasPassword:{4}", room.name,room.intro,room.playerCount,room.isPlaying, room.hasPassword);
            GameObject roomInfoPanel = Instantiate(Resources.Load("perfabs/RoomInfo")) as GameObject;
            roomInfoPanel.name = room.roomKey + "";
            Transform[] infos = roomInfoPanel.GetComponentsInChildren<Transform>();
            for(int j = 0;j < infos.Length; j++)
            {
                Transform transform = infos[j];
                if (transform.name == "Name")
                    transform.GetComponent<Text>().text = room.name as string;
                else if (transform.name == "Intro")
                    transform.GetComponent<Text>().text = room.intro as string;
                else if (transform.name == "Lock")
                    transform.GetComponent<Image>().enabled = (byte)room.hasPassword == 1 ? false : true;
                else if (transform.name == "PlayerCount")
                    transform.GetComponent<Text>().text = room.playerCount + "/3";
                else if (transform.name == "IsPlaying")
                    transform.GetComponent<Text>().text = (byte)room.isPlaying == 1 ? "准备中" : "游戏中";
            }
            roomInfoPanel.transform.SetParent(roomListPanel, false);
            float top = roomListPanel.GetComponent<RectTransform>().offsetMax.y;
            float bottom = top + (270 - roomList.Count * 47 + 5);
            roomListPanel.GetComponent<RectTransform>().offsetMin = new Vector2(roomListPanel.GetComponent<RectTransform>().offsetMin.x, bottom);
        }
    }

    public void joinRoom()
    {
        disableButton(joinButton);
        joinCountDown = 1;
        Debug.LogFormat("roomkey:{0}", chooseRoom.name);
        ROOM_INFOS chooseRoomInfo = ((Dictionary<string, ROOM_INFOS>)Init.Datas["roomsInfo"])[chooseRoom.name];
            if ((byte)chooseRoomInfo.hasPassword == 1)
            {
                Debug.LogFormat("正在加入房间,key:{0}", chooseRoomInfo.roomKey);
                KBEngine.Event.fireIn("reqJoinRoom", chooseRoomInfo.roomKey,"");
                //SceneManager.LoadScene("Room");
            }
            else
            {
                openPasswordInputPanel();
            }
    }

    public void submitPasswordJoinRoom()
    {
        disableButton(submitPasswordJoinButton);
        submitPasswordJoinCountDown = 1;
        ROOM_INFOS chooseRoomInfo = ((Dictionary<string, ROOM_INFOS>)Init.Datas["roomsInfo"])[chooseRoom.name];
        Debug.LogFormat("正在加入房间,key:{0},password:{1}", chooseRoomInfo.roomKey, joinRoomPasswordInput.text);
        KBEngine.Event.fireIn("reqJoinRoom", chooseRoomInfo.roomKey, joinRoomPasswordInput.text);
    }

    public void onReqJoinRoomFailed(ushort retcode)
    {
        Debug.LogFormat("加入房间失败，原因:{0}", KBEngineApp.app.serverErr(retcode));
        openMessagePanel("加入房间失败，原因:" + KBEngineApp.app.serverErr(retcode));
    }

    public void exitHall()
    {
        KBEngineApp.app.reset();
        Init.Datas.Clear();
        SceneManager.LoadScene("login");
    }

    public void onDisconnected()
    {
        exitHall();
    }

    public void openPasswordInputPanel()
    {
        Debug.Log("打开密码输入面板");
        passwordInputPanel.SetActive(true);
    }

    public void closePasswordInputPanel()
    {
        Debug.Log("关闭密码输入面板");
        passwordInputPanel.SetActive(false);
    }

    public void openMessagePanel(string message)
    {
        GameObject messagePanel = Instantiate(Resources.Load("perfabs/MessagePanel")) as GameObject;
        Transform[] transforms = messagePanel.GetComponentsInChildren<Transform>();
        for (int j = 0; j < transforms.Length; j++)
        {
            Transform transform = transforms[j];
            if (transform.name == "Text")
            {
                transform.GetComponent<Text>().text = message;
                break;
            }
        }
        messagePanel.transform.SetParent(canvas, false);
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
    void Start () {
        KBEngine.Event.registerOut("onReqRoomsByParams", this, "onReqRoomsByParams");
        //KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        KBEngine.Event.registerOut("onReqJoinRoomFailed", this, "onReqJoinRoomFailed");
        KBEngine.Event.registerOut("onReqJoinRoomSuccess", this, "onReqJoinRoomSuccess");

        joinRoomPasswordInput.inputType = InputField.InputType.Password;
        createRoomPasswordInput.inputType = InputField.InputType.Password;
        //本地房间数据列表
        Dictionary<string, ROOM_INFOS> roomsInfo = new Dictionary<string, ROOM_INFOS>();
        Init.Datas.Remove("roomsInfo");
        Init.Datas.Add("roomsInfo", roomsInfo);

        selectRoomsByParams();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        //当点击鼠标左键时
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //点到了UGUI
                //Debug.Log(EventSystem.current.currentSelectedGameObject);
                long roomKey;
                if(long.TryParse(EventSystem.current.currentSelectedGameObject.name,out roomKey))
                {
                    chooseRoom = EventSystem.current.currentSelectedGameObject;
                    for(int i = 0; i < roomListPanel.childCount; i++)
                    {
                        Transform roomInfoPanel = roomListPanel.GetChild(i);
                        if (long.Parse(roomInfoPanel.name) == roomKey)
                        {
                            Image image = roomInfoPanel.GetComponent<Image>();
                            Color color = new Color32(150, 150, 150, 255);
                            image.color = color;
                        }
                        else
                        {
                            Image image = roomInfoPanel.GetComponent<Image>();
                            Color color = new Color32(255, 255, 255, 100);
                            image.color = color;
                        }
                    }
                }
            }
            else
            //没点到UGUI
            {

            }

        }
        if(createCountDown > 0)
        {
            createCountDown -= Time.deltaTime;
            if (createCountDown <= 0)
            {
                enableButton(createButton);
            }
        }
        if (joinCountDown > 0)
        {
            joinCountDown -= Time.deltaTime;
            if (joinCountDown <= 0)
            {
                enableButton(joinButton);
            }
        }
        if (submitPasswordJoinCountDown > 0)
        {
            submitPasswordJoinCountDown -= Time.deltaTime;
            if (submitPasswordJoinCountDown <= 0)
            {
                enableButton(submitPasswordJoinButton);
            }
        }
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
