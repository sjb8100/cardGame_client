using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform playersInfoPanel;
    public Transform openCardsPanel;
    public Transform ownCardPanel;
    public Transform ownPlayPanel;
    public Transform leftCardPanel;
    public Transform leftPlayPanel;
    public Transform rightCardPanel;
    public Transform rightPlayPanel;
    public Transform chatShowPanel;

    public Transform pointPanel;
    public Transform actionPanel;
    public Transform scorePanel;

    public Text ownMessageText;
    public Text leftNameText;
    public Text leftMessageText;
    public Text rightNameText;
    public Text rightMessageText;
    public Text ownLandlordText;
    public Text leftLandlordText;
    public Text rightLandlordText;

    public Button onePointButton;
    public Button twoPointButton;
    public Button threePointButton;
    public Button noPointButton;
    public Button passButton;

    private int raiseIndex;
    private int leftIndex;
    private int rightIndex;

    private int lastPlayIndex = 0;

    private List<Int32> playCardsList = new List<Int32>();
    private string lastPlayType = "";
    private Dictionary<string, List<int>> lastPlayTypeCount = new Dictionary<string, List<int>>();

    private int color = 0;
    private string chatMessage = "";

    public void reqPlayersInfo()
    {
        Debug.LogFormat("请求玩家信息");
        KBEngine.Event.fireIn("reqPlayersInfo", Init.Datas["roomKey"]);
    }

    public void setPlayersInfo(List<PLAYER_INFOS> playersInfo)
    {
        Debug.LogFormat("设置玩家信息");
        for (int i = 0; i < 3; i++)
        {
            PLAYER_INFOS player = playersInfo[i];
            if (player.id == ((AccountBase)Init.Datas["account"]).dbid)
            {
                leftIndex = (i + 2) % 3;
                rightIndex = (i + 1) % 3;
                break;
            }
        }

        leftNameText.text = playersInfo[leftIndex].name;
        rightNameText.text = playersInfo[rightIndex].name;

        for(int i = 0;i < playersInfo.Count; i++)
        {
            PLAYER_INFOS player = playersInfo[i];
            Transform playerInfoPanel = playersInfoPanel.Find("Player" + (i + 1) + "Panel");
            playerInfoPanel.Find("Name").GetComponent<Text>().text = player.name;
            int winCount = int.Parse(player.winCount.ToString());
            playerInfoPanel.Find("WinCount").GetComponent<Text>().text = winCount.ToString();
            int loseCount = int.Parse(player.loseCount.ToString());
            playerInfoPanel.Find("LoseCount").GetComponent<Text>().text = loseCount.ToString();
            int countSum = winCount + loseCount;
            double winRate;
            if (countSum == 0)
            {
                winRate = Math.Round(100.00, 2);
            }
            else
            {
                winRate = Math.Round(100.00 * winCount / countSum, 2);
            }
            playerInfoPanel.Find("WinRate").GetComponent<Text>().text = winRate.ToString();
        }
    }

    public void gameInit()
    {
        Debug.LogFormat("游戏初始化");
        KBEngine.Event.fireIn("reqGameInit",Init.Datas["roomKey"]);
    }

    public void reqExitGame()
    {
        Debug.LogFormat("请求退出游戏");
        KBEngine.Event.fireIn("reqExitGame", Init.Datas["roomKey"]);
    }

    public void setHandCards(List<Int32> handCards)
    {
        Debug.LogFormat("设置自己的手牌");

        for (int i = 0; i < ownCardPanel.transform.childCount; i++)
        {
            Destroy(ownCardPanel.transform.GetChild(i).gameObject);
        }

        float maxWidth = -ownCardPanel.GetComponent<RectTransform>().rect.x * 2;
        double startLeft = 94.5 - 10.5 * ((handCards.Count - 1) / 2);
        float space = maxWidth / 19;
        for (int i = 0;i < handCards.Count; i++)
        {
            int cardIndex = int.Parse(handCards[i].ToString());

            string cardPerfabName = "perfabs/poker/" + getCardName(cardIndex);
            GameObject card = Instantiate(Resources.Load(cardPerfabName)) as GameObject;
            card.transform.SetParent(ownCardPanel,false);
            float left = (float)(startLeft + space * i);
            float right = left;
            card.GetComponent<RectTransform>().offsetMin = new Vector2(left, card.GetComponent<RectTransform>().offsetMin.y);
            card.GetComponent<RectTransform>().offsetMax = new Vector2(right, card.GetComponent<RectTransform>().offsetMax.y);

            Button cardButton = card.GetComponent<Button>();
            cardButton.onClick.AddListener(delegate () {
                this.clickCard(cardIndex);
            });
        }
    }

    public void setHandCardsCount(Int16 seatIndex, Int32 handCardsCount)
    {
        Debug.LogFormat("设置其他玩家的手牌");
        Transform handCardsPanel = leftCardPanel;
        if (seatIndex == rightIndex)
            handCardsPanel = rightCardPanel;

        for (int i = 0; i < handCardsPanel.transform.childCount; i++)
        {
            Destroy(handCardsPanel.transform.GetChild(i).gameObject);
        }
        
        for(int i = 0;i < handCardsCount; i++)
        {
            GameObject card = Instantiate(Resources.Load("perfabs/poker/back")) as GameObject;
            card.transform.SetParent(handCardsPanel,false);
        }
    }

    public void setRaiseIndex(Int32 raiseIndex)
    {
        Debug.LogFormat("设置叫牌位:{0},玩家位置:{0}", raiseIndex, ((AccountBase)Init.Datas["account"]).seatIndex);
        this.raiseIndex =int.Parse(raiseIndex.ToString());
        int seatIndex = ((Account)Init.Datas["account"]).seatIndex;
        if (this.raiseIndex == seatIndex)
        {
            pointPanel.gameObject.SetActive(true);
        }
    }

    public void setOpenCard(Int16 openCard)
    {
        Debug.LogFormat("设置明牌");
        Transform transformPlayPanel = ownPlayPanel;
        if (this.raiseIndex == leftIndex)
            transformPlayPanel = leftPlayPanel;
        else if (this.raiseIndex == rightIndex)
            transformPlayPanel = rightPlayPanel;

        for (int i = 0; i < transformPlayPanel.transform.childCount; i++)
        {
            Destroy(transformPlayPanel.transform.GetChild(i).gameObject);
        }
        int openCardIndex = int.Parse(openCard.ToString());
        string cardPerfabName = "perfabs/poker/" + getCardName(openCardIndex);
        Debug.LogFormat("open card:{0}", cardPerfabName);
        GameObject card = Instantiate(Resources.Load(cardPerfabName)) as GameObject;
        card.transform.SetParent(transformPlayPanel, false);
    }

    public void setHiddenCards()
    {
        Debug.LogFormat("设置底牌");
        for (int i = 0; i < openCardsPanel.transform.childCount; i++)
        {
            Destroy(openCardsPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject card = Instantiate(Resources.Load("perfabs/poker/back")) as GameObject;
            card.transform.SetParent(openCardsPanel, false);
        }
    }

    public void setHighestPoint(Int16 highestPoint)
    {
        Debug.LogFormat("设置最高分:{0}", highestPoint);
        int highestPointNumber = int.Parse(highestPoint.ToString());
        if(highestPointNumber >= 1)
        {
            disableButton(onePointButton);
        }
        if (highestPointNumber >= 2)
        {
            disableButton(twoPointButton);
        }
        if (highestPointNumber == 3)
        {
            disableButton(threePointButton);
        }
    }

    public void setHiddenCardsOpen(List<Int32> hiddenCardsOpen)
    {
        Debug.LogFormat("设置打开底牌");
        for (int i = 0; i < openCardsPanel.transform.childCount; i++)
        {
            Destroy(openCardsPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < hiddenCardsOpen.Count; i++)
        {
            int openCardIndex = int.Parse(hiddenCardsOpen[i].ToString());
            string cardPerfabName = "perfabs/poker/" + getCardName(openCardIndex);
            Debug.LogFormat("hidden card open:{0}", cardPerfabName);
            GameObject card = Instantiate(Resources.Load(cardPerfabName)) as GameObject;
            card.transform.SetParent(openCardsPanel, false);
        }
    }

    public void setLandlordIndex(Int16 landlordIndex)
    {
        Debug.LogFormat("设置地主位:{0}",landlordIndex);
        ownMessageText.text = "";
        leftMessageText.text = "";
        rightMessageText.text = "";

        for(int i = 0;i < ownPlayPanel.childCount; i++)
        {
            Destroy(ownPlayPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < leftPlayPanel.childCount; i++)
        {
            Destroy(leftPlayPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < rightPlayPanel.childCount; i++)
        {
            Destroy(rightPlayPanel.GetChild(i).gameObject);
        }

        Text landlordText = ownLandlordText;
        if(landlordIndex == leftIndex)
        {
            landlordText = leftLandlordText;
        }
        else if(landlordIndex == rightIndex)
        {
            landlordText = rightLandlordText;
        }
        landlordText.gameObject.SetActive(true);
    }

    public void setPlayIndex(Int16 playIndex)
    {
        Debug.LogFormat("设置出牌位：{0}", playIndex);
        Transform playPanel = ownPlayPanel;
        Text messageText = ownMessageText;
        int seatIndex = ((AccountBase)Init.Datas["account"]).seatIndex;
        if (playIndex == seatIndex)
        {
            actionPanel.gameObject.SetActive(true);
        }
        if(playIndex == this.lastPlayIndex)
        {
            this.lastPlayType = "";
        }
        else if(playIndex == leftIndex)
        {
            playPanel = leftPlayPanel;
            messageText = leftMessageText;
        }
        else if(playIndex == rightIndex)
        {
            playPanel = rightPlayPanel;
            messageText = rightMessageText;
        }
        for (int i = 0; i < playPanel.childCount; i++)
        {
            Destroy(playPanel.GetChild(i).gameObject);
        }
        messageText.text = "";
    }

    public void setLastPlayIndex(Int16 lastPlayIndex)
    {
        Debug.LogFormat("设置最后出牌位：{0}", lastPlayIndex);
        this.lastPlayIndex = lastPlayIndex;
        int seatIndex = ((AccountBase)Init.Datas["account"]).seatIndex;
        if (lastPlayIndex == seatIndex)
        {
            Debug.LogFormat("禁用pass按钮");
            passButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogFormat("启用pass按钮");
            passButton.gameObject.SetActive(true);
        }
    }

    public void setPlayCards(Int16 seatIndex,List<Int32> playCards)
    {
        this.playCardsList.Clear();
        Debug.LogFormat("设置打出的牌");
        Transform playCardsPanel = ownPlayPanel;
        if (seatIndex == leftIndex)
            playCardsPanel = leftPlayPanel;
        else if (seatIndex == rightIndex)
            playCardsPanel = rightPlayPanel;

        for (int i = 0; i < playCardsPanel.transform.childCount; i++)
        {
            Destroy(playCardsPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < playCards.Count; i++)
        {
            int playCardIndex = int.Parse(playCards[i].ToString());
            string cardPerfabName = "perfabs/poker/" + getCardName(playCardIndex);
            GameObject card = Instantiate(Resources.Load(cardPerfabName)) as GameObject;
            card.transform.SetParent(playCardsPanel, false);
        }
    }

    public void setMessage(Int16 seatIndex,Int32 message)
    {
        Debug.LogFormat("设置消息");
        Text messageText = ownMessageText;
        if (seatIndex == leftIndex)
            messageText = leftMessageText;
        else if (seatIndex == rightIndex)
            messageText = rightMessageText;
        
        if(message == 0)
        {
            messageText.text = "不叫";
        }
        else if(message < 3)
        {
            messageText.text = message + "分";
        }
        else if (message == 3)
        {
            messageText.text = "";
        }
        else if (message == 4)
        {
            messageText.text = "pass";
        }
    }

    public void setLastPlayCards(List<int> lastPlayCardsList)
    {
        string cardNameList = "";
        for(int i = 0; i < lastPlayCardsList.Count; i++)
        {
            cardNameList += getCardName(lastPlayCardsList[i]);
        }
        Debug.LogFormat("设定最后出牌:{0}", cardNameList);
        this.lastPlayTypeCount = getTypeCount(lastPlayCardsList);
        this.lastPlayType = checkCardsType(this.lastPlayTypeCount);
        Debug.LogFormat("最后牌型：{0}", this.lastPlayType);
    }

    public void setIsGameOver(short isGameOver)
    {
        if(isGameOver == 1)
        {
            Debug.LogFormat("游戏结束");
            scorePanel.gameObject.SetActive(true);
        }
    }

    public void setGameResult(Int32 gameResult)
    {
        if(gameResult == 0)
        {
            Debug.LogFormat("游戏胜利");
            scorePanel.Find("Result").GetComponent<Text>().text = "胜利";
        }
        else if (gameResult == 1)
        {
            Debug.LogFormat("游戏失败");
            scorePanel.Find("Result").GetComponent<Text>().text = "失败";
        }
    }

    public void callPoint(int point)
    {
        Debug.LogFormat("请求叫牌：{0}分",point);
        KBEngine.Event.fireIn("callPoint", point);
        pointPanel.gameObject.SetActive(false);
    }

    public void clickCard(int cardIndex)
    {
        Debug.LogFormat("点击牌：{0}", getCardName(cardIndex));
        if (playCardsList.Contains(cardIndex))
        {
            playCardsList.Remove(cardIndex);
        }
        else
        {
            playCardsList.Add(cardIndex);
            playCardsList.Sort();
        }
    }

    public void playCards()
    {
        string cardNameList = "";
        for (int i = 0; i < this.playCardsList.Count; i++)
        {
            cardNameList += getCardName(this.playCardsList[i])+",";
        }
        Debug.LogFormat("出牌:{0}", cardNameList);
        Dictionary<string, List<int>> typeCount = getTypeCount(this.playCardsList);
        string currentType = checkCardsType(typeCount);
        string compareResult = numberCompare(typeCount, currentType);
        Debug.LogFormat("lastType:{0},current type:{1},compareResult:{2}",this.lastPlayType, currentType,compareResult);
        if (currentType == "error")
        {
            ownMessageText.text = "牌型错误！";
            return;
        }
        else if(compareResult == "error")
        {
            ownMessageText.text = "牌型不匹配！";
            return;
        }
        else if(compareResult == "smaller")
        {
            ownMessageText.text = "牌面太小！";
            return;
        }
        ownMessageText.text = "";
        actionPanel.gameObject.SetActive(false);
        KBEngine.Event.fireIn("reqPlayCards", this.playCardsList);
    }

    public void pass()
    {
        Debug.LogFormat("过牌");
        actionPanel.gameObject.SetActive(false);
        KBEngine.Event.fireIn("passCards");
    }

    //0:梅花3，1:方块3，2:红桃3，3:黑桃3
    //4:梅花4，5:方块4，6:红桃4，7:黑桃4
    //……
    //40:梅花K，41:方块K，42:红桃K，43:黑桃K
    //44:梅花A，45:方块A，46:红桃A，47:黑桃A
    //48:梅花2，49:方块2，50;红桃2，51:黑桃2
    //52:小王，53:大王
    private string getCardName(int cardIndex)
    {
        if (cardIndex == 52)
            return "joker1";
        if (cardIndex == 53)
            return "joker2";

        int color = cardIndex % 4;
        int cardNum = cardIndex / 4 + 3;
        if (cardNum > 13)
            cardNum -= 13;
        string colorName = "";
        switch (color)
        {
            case 0: colorName = "club"; break;
            case 1: colorName = "diamond"; break;
            case 2: colorName = "heart"; break;
            case 3: colorName = "spade"; break;
        }
        return colorName + cardNum;
    }

    public Dictionary<string, List<int>> getTypeCount(List<int> cardsList)
    {
        Debug.LogFormat("统计牌型数量");
        //统计各个点数的张数
        Dictionary<int, int> numberCount = new Dictionary<int, int>();
        for (int i = 0; i < cardsList.Count; i++)
        {
            int cardIndex = cardsList[i];
            if(cardIndex == 52 || cardIndex == 53)
            {
                numberCount.Add(cardIndex,1);
                continue;
            }
            int cardNum = cardIndex / 4 + 3;
            if (numberCount.ContainsKey(cardNum))
            {
                numberCount[cardNum]++;
            }
            else
            {
                numberCount.Add(cardNum, 1);
            }
        }
        //统计单张、对子、三条、炸弹的数量
        Dictionary<string, List<int>> typeCount = new Dictionary<string, List<int>>();
        typeCount.Add("single", new List<int>());
        typeCount.Add("pair", new List<int>());
        typeCount.Add("three", new List<int>());
        typeCount.Add("four", new List<int>());
        string single = "";
        string pair = "";
        string three = "";
        string four = "";
        foreach (var item in numberCount)
        {
            if (item.Value == 1)
            {
                typeCount["single"].Add(item.Key);
                single += item.Key + ",";
            }
            else if (item.Value == 2)
            {
                typeCount["pair"].Add(item.Key);
                pair += item.Key + ",";
            }
            else if (item.Value == 3)
            {
                typeCount["three"].Add(item.Key);
                three += item.Key + ",";
            }
            else if (item.Value == 4)
            {
                typeCount["four"].Add(item.Key);
                four += item.Key + ",";
            }
        }
        Debug.LogFormat("single:{0},pair:{1},three:{2},four:{3}",single, pair, three, four);
        return typeCount;
    }

    /* 1.fourTwoPair四带两张
     * 2.fourTwoSingle四带两对
     * 3.boom炸弹
     * 4.threeShun*三顺
     * 5.threePair*三带二
     * 6.threeSingle*三带一
     * 7.pair*对子
     * 8.single *顺子
     * 9.jokerBoom王炸
     * 10.error错误
    */
    public string checkCardsType(Dictionary<string, List<int>> typeCount)
    {
        Debug.LogFormat("检查牌型");
        //检查牌型
        if (typeCount ["four"].Count > 1)
        {
            //没有连炸
            return "error";
        }
        if(typeCount["four"].Count == 1)
        {
            if(typeCount["three"].Count == 0)
            {
                //1.四带两对
                if (typeCount["pair"].Count == 2 && typeCount["single"].Count == 0)
                {
                    return "fourTwoPair";
                }
                //2.四带两张
                if (typeCount["pair"].Count == 0 && typeCount["single"].Count == 2)
                {
                    return "fourTwoSingle";
                }
                //3.炸弹
                if (typeCount["pair"].Count == 0 && typeCount["single"].Count == 0)
                {
                    return "boom";
                }
            }
            else
            {
                //四带三错误
                return "error";
            }
        }
        //4.三顺
        if(typeCount["three"].Count >= 2 && typeCount["pair"].Count == 0 && typeCount["single"].Count == 0)
        {
            int currentNum = typeCount["three"][0];
            for(int i = 1;i < typeCount["three"].Count; i++)
            {
                if(typeCount["three"][i] != currentNum + 1 || typeCount["three"][i] == 15/*即2*/)
                {
                    return "error";
                }
                currentNum = typeCount["three"][i];
            }
            return "threeShun"+ typeCount["three"].Count;
        }
        if(typeCount["three"].Count > 0)
        {
            int currentNum = typeCount["three"][0];
            for (int i = 1; i < typeCount["three"].Count; i++)
            {
                if (typeCount["three"][i] != currentNum + 1 || typeCount["three"][i] == 15/*即2*/)
                {
                    return "error";
                }
                currentNum = typeCount["three"][i];
            }
            if (typeCount["pair"].Count == typeCount["three"].Count && typeCount["single"].Count == 0)
            {
                //5.三带二
                return "threePair" +typeCount["three"].Count;
            }
            if (typeCount["single"].Count == typeCount["three"].Count && typeCount["pair"].Count == 0)
            {
                //6.三带一
                return "threeSingle" + typeCount["three"].Count;
            }
            return "error";
        }
        if (typeCount["pair"].Count == 1 || typeCount["pair"].Count >= 3)
        {
            if(typeCount["single"].Count == 0)
            {
                //7.对子、双顺
                int currentNum = typeCount["pair"][0];
                for (int i = 1; i < typeCount["pair"].Count; i++)
                {
                    if (typeCount["pair"][i] != currentNum + 1 || typeCount["pair"][i] == 15/*即2*/)
                    {
                        return "error";
                    }
                    currentNum = typeCount["pair"][i];
                }
                return "pair" + typeCount["pair"].Count;
            }
            else
            {
                return "error";
            }
        }
        if(typeCount["pair"].Count == 2)
        {
            return "error";
        }
        if (typeCount["single"].Count == 1 || typeCount["single"].Count >= 5)
        {
            //8.单张、顺子
            int currentNum = typeCount["single"][0];
            for (int i = 1; i < typeCount["single"].Count; i++)
            {
                if (typeCount["single"][i] != currentNum + 1 || typeCount["single"][i] == 15/*即2*/)
                {
                    return "error";
                }
                currentNum = typeCount["single"][i];
            }
            return "single" + typeCount["single"].Count;
        }
        if (typeCount["single"].Count == 2 && typeCount["single"].Contains(52) && typeCount["single"].Contains(53))
        {
            //9.王炸
            return "jokerBoom";
        }
        return "error";
    }

    public string numberCompare(Dictionary<string, List<int>> playCardsTypeCount,string currentType)
    {
        Debug.LogFormat("牌型比较：lastType:{0},currentType:{1}",this.lastPlayType,currentType);
        if(this.lastPlayType == "")
        {
            return "bigger";
        }
        if (currentType == "jokerBoom")
        {
            Debug.LogFormat("王炸");
            return "bigger";
        }
        if (currentType == "boom")
        {
            Debug.LogFormat("炸弹，current number:{0}", playCardsTypeCount["four"][0]);
            if (this.lastPlayType != "boom")
                return "bigger";
            Debug.LogFormat("炸弹，last number:{0},current number:{1}",this.lastPlayTypeCount["four"][0], playCardsTypeCount["four"][0]);
            if (playCardsTypeCount["four"][0] > this.lastPlayTypeCount["four"][0])
                return "bigger";
        }
        if(currentType == this.lastPlayType)
        {
            if ((currentType == "fourTwoPair" || currentType == "fourTwoSingle") && playCardsTypeCount["four"][0] > this.lastPlayTypeCount["four"][0])
            {
                Debug.LogFormat("四带二，last number:{0},current number:{1}", this.lastPlayTypeCount["four"][0], playCardsTypeCount["four"][0]);
                return "bigger";
            }
            if ((currentType.Contains("threeShun") || currentType.Contains("threePair") || currentType.Contains("threeSingle")) && playCardsTypeCount["three"][0] > this.lastPlayTypeCount["three"][0])
            {
                Debug.LogFormat("三顺，last number:{0},current number:{1}", this.lastPlayTypeCount["three"][0], playCardsTypeCount["three"][0]);
                return "bigger";
            }
            if (currentType.Contains("pair") && playCardsTypeCount["pair"][0] > this.lastPlayTypeCount["pair"][0])
            {
                Debug.LogFormat("双顺，last number:{0},current number:{1}", this.lastPlayTypeCount["pair"][0], playCardsTypeCount["pair"][0]);
                return "bigger";
            }
            if (currentType.Contains("single") && playCardsTypeCount["single"][0] > this.lastPlayTypeCount["single"][0])
            {
                Debug.LogFormat("单顺，last number:{0},current number:{1}", this.lastPlayTypeCount["single"][0], playCardsTypeCount["single"][0]);
                return "bigger";
            }
            return "smaller";
        }
        else
        {
            return "error";
        }
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

    public void setColor(int color)
    {
        this.color = color;
    }

    public void addCard(int cardNum)
    {
        Debug.LogFormat("请求加一张牌:color:{0},num:{1}",this.color,cardNum);
        KBEngine.Event.fireIn("reqAddCard",color,cardNum);
    }

    public void returnRoomSence()
    {
        SceneManager.LoadScene("room");
    }

    //----------------------------------
    //回调函数
    //----------------------------------

    public void onReqExitGame(string name)
    {
        Debug.LogFormat("正在退出游戏：room:{0},name:{1}", Init.Datas["roomKey"], name);
        Init.Datas.Remove("roomKey");
        SceneManager.LoadScene("hall");
    }

    public void onReqPlayersInfo(List<PLAYER_INFOS> playersInfo)
    {
        Debug.LogFormat("正在设置玩家信息");
        setPlayersInfo(playersInfo);
    }

    // Use this for initialization
    void Start () {
        KBEngine.Event.registerOut("setSeatsData", this, "onReqPlayersInfo");
        KBEngine.Event.registerOut("setHandCards", this, "setHandCards");
        KBEngine.Event.registerOut("setHandCardsCount", this, "setHandCardsCount");
        KBEngine.Event.registerOut("setRaiseIndex", this, "setRaiseIndex");
        KBEngine.Event.registerOut("setOpenCard", this, "setOpenCard");
        KBEngine.Event.registerOut("setHiddenCardsOpen", this, "setHiddenCardsOpen");
        KBEngine.Event.registerOut("setHighestPoint", this, "setHighestPoint");
        KBEngine.Event.registerOut("setLandlordIndex", this, "setLandlordIndex");
        KBEngine.Event.registerOut("setPlayIndex", this, "setPlayIndex");
        KBEngine.Event.registerOut("setLastPlayIndex", this, "setLastPlayIndex");
        KBEngine.Event.registerOut("setPlayCards", this, "setPlayCards");
        KBEngine.Event.registerOut("setLastPlayCards", this, "setLastPlayCards");
        KBEngine.Event.registerOut("setIsGameOver", this, "setIsGameOver");
        KBEngine.Event.registerOut("setGameResult", this, "setGameResult");
        KBEngine.Event.registerOut("setMessage", this, "setMessage");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onReqExitGame");

        reqPlayersInfo();
        gameInit();
        setHiddenCards();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
