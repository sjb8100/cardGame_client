using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour {

    public bool isChoose = false;

	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(this.chooseCard);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void chooseCard()
    {
        if (!isChoose)
        {
            Debug.LogFormat("选中牌，弹出");
            this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.x, 15);
            this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, 15);
            isChoose = true;
        }
        else
        {
            Debug.LogFormat("取消选牌，塞回");
            this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.x, 0);
            this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, 0);
            isChoose = false;
        }
    }
}
