using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePanelController : MonoBehaviour {

    public void closePanel()
    {
        Debug.LogFormat("关闭消息面板,this:{0}",this);
        Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
