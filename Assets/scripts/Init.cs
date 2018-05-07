using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour {
    private static Dictionary<string, object> datas = new Dictionary<string, object>();

    public static Dictionary<string,object> Datas
    {
        get { return datas; }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        Debug.LogFormat("初始化完成，跳转到登陆界面");
        SceneManager.LoadScene("login");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
