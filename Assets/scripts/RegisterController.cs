using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour {
    public Transform canvas;
    public InputField userNameInput;
    public InputField passwordInput;
    public InputField emailInput;

    public void register()
    {
        KBEngine.Event.fireIn("createAccount", userNameInput.text, passwordInput.text, System.Text.Encoding.UTF8.GetBytes("PC"));
    }

    public void cancel()
    {
        SceneManager.LoadScene("login");
    }

    public void onCreateAccountResult(UInt16 resultCode,byte[] datas)
    {
        if(resultCode != 0)
        {
            Debug.LogFormat("注册失败，错误码:{0},原因:{1}", resultCode, KBEngineApp.app.serverErr(resultCode));
            openMessagePanel("注册失败，错误码:"+ resultCode + ",原因:"+ KBEngineApp.app.serverErr(resultCode));
        }
        else
        {
            Debug.LogFormat("注册成功,原因:{0}", KBEngineApp.app.serverErr(resultCode));
            openMessagePanel("注册成功,返回登陆界面");

            SceneManager.LoadScene("login");
        }
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

    // Use this for initialization
    void Start () {
        KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");

        passwordInput.inputType = InputField.InputType.Password;
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
