using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetNameController : MonoBehaviour {
    public Transform canvas;
    public InputField nameInput;

    public void setName()
    {
        Debug.LogFormat("准备设定昵称：name:{0}", nameInput.text);
        KBEngine.Event.fireIn("reqSetName", nameInput.text);
    }

    public void onSetNameFailed(byte retcode,string name)
    {
        Debug.LogFormat("设定昵称:{0} 失败，原因:{1}", name, KBEngineApp.app.serverErr(retcode));
       openMessagePanel("设定昵称:"+ name + " 失败，原因:"+ KBEngineApp.app.serverErr(retcode));
    }

    public void onSetNameSuccessfully(string name)
    {
        Debug.LogFormat("设定昵称成功，name:{0},即将跳转至大厅", name);
        Entity account = KBEngineApp.app.player();
        Init.Datas.Add("account", account);
        SceneManager.LoadScene("hall");
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
        KBEngine.Event.registerOut("onSetNameFailed", this, "onSetNameFailed");
        KBEngine.Event.registerOut("onSetNameSuccessfully", this, "onSetNameSuccessfully");
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
}
