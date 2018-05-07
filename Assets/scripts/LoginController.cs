using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {
    public Transform canvas;
    public InputField userNameInput;
    public InputField passwordInput;

    public void login()
    {
        Debug.LogFormat("请求登陆：id:{0},password:{1}", userNameInput.text, passwordInput.text);
        KBEngine.Event.fireIn("login", userNameInput.text, passwordInput.text, System.Text.Encoding.UTF8.GetBytes("PC"));
    }

    public void onConnectionState(string state)
    {
        if (state != "")
        {
            Debug.LogFormat(state);
            openMessagePanel(state);
        }
    }

    public void onLoginSuccessfully()
    {
        Debug.Log("登陆成功");

    }

    public void onLoginFailed(UInt16 failedCode)
    {
        Debug.LogFormat("登陆失败，错误码:{0},原因:{1}", failedCode, KBEngineApp.app.serverErr(failedCode));
        openMessagePanel("登陆失败，错误码:" + failedCode + ",原因:" + KBEngineApp.app.serverErr(failedCode));
    }

    public void onLoginBaseappFailed(UInt16 failedcode)
    {
        Debug.LogFormat("loginBaseapp is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr(failedcode));
        openMessagePanel("登陆网关失败，"+"原因:" + KBEngineApp.app.serverErr(failedcode));
    }

    public void onLoginBaseapp()
    {
        Debug.LogFormat("connect to loginBaseapp, please wait...(连接到网关， 请稍后...)");
    }

    public void onEnterHall()
    {
        Debug.LogFormat("跳转至大厅");
        Entity account = KBEngineApp.app.player();
        Init.Datas.Add("account",account);
        SceneManager.LoadScene("hall");
    }

    public void onEnterSetName()
    {
        Debug.LogFormat("跳转至创建角色界面");
        SceneManager.LoadScene("setName");
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

    public void Start()
    {
        KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
        KBEngine.Event.registerOut("onLoginFailed",this, "onLoginFailed");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
        KBEngine.Event.registerOut("onLoginBaseappFailed", this, "onLoginBaseappFailed");
        KBEngine.Event.registerOut("onLoginBaseapp", this, "onLoginBaseapp");
        KBEngine.Event.registerOut("onEnterHall", this, "onEnterHall");
        KBEngine.Event.registerOut("onEnterSetName", this, "onEnterSetName");
        
        passwordInput.inputType = InputField.InputType.Password;
    }

    public void register()
    {
        Debug.Log("跳转到注册场景");
        SceneManager.LoadScene("register");
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
