namespace KBEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Account : AccountBase
    {

        public override void __init__()
        {
            KBEngine.Event.registerIn("reqSetName", this, "reqSetName");
            KBEngine.Event.registerIn("reqCreateRoom", this, "reqCreateRoom");
            KBEngine.Event.registerIn("reqRoomsByParams", this, "reqRoomsByParams");
            KBEngine.Event.registerIn("reqJoinRoom", this, "reqJoinRoom");
            KBEngine.Event.registerIn("reqEnterWorld", this, "reqEnterWorld");
            KBEngine.Event.registerIn("reqRoomInfo", this, "reqRoomInfo");
            KBEngine.Event.registerIn("reqReadyPlay", this, "reqReadyPlay");
            KBEngine.Event.registerIn("reqPlayGame", this, "reqPlayGame");
            KBEngine.Event.registerIn("reqRoomPassword", this, "reqRoomPassword");
            KBEngine.Event.registerIn("reqEditRoomInfo", this, "reqEditRoomInfo");
            KBEngine.Event.registerIn("reqExitRoom", this, "reqExitRoom");
            KBEngine.Event.registerIn("reqPlayersInfo", this, "reqPlayersInfo");
            KBEngine.Event.registerIn("reqGameInit", this, "reqGameInit");
            KBEngine.Event.registerIn("reqExitGame", this, "reqExitGame");
            KBEngine.Event.registerIn("callPoint", this, "callPoint");
            KBEngine.Event.registerIn("reqPlayCards", this, "reqPlayCards");
            KBEngine.Event.registerIn("passCards", this, "passCards");
            KBEngine.Event.registerIn("reqAddCard", this, "reqAddCard");


            //base.__init__();
            Debug.Log("已创建account实体");
            KBEngine.Event.fireOut("onLoginSuccessfully");
        }

        public void reqSetName(string name)
        {
            Debug.LogFormat("请求设定昵称,name:{0}", name);
            baseCall("reqSetName", name);
        }

        public void reqCreateRoom(string name, string intro, string password)
        {
            Debug.LogFormat("请求创建房间,name:{0},intro:{1},password:{2}", name, intro, password);
            baseCall("reqCreateRoom", name, intro, password);
        }

        public void reqRoomsByParams(string nameOrIntro,int isFull,int isPlaying,int hasPassword)
        {
            Debug.LogFormat("请求所有符合条件的房间，nameOrIntro:{0},isFull:{1},isPlaying:{2},hasPassword:{3}", nameOrIntro, isFull, isPlaying, hasPassword);
            baseCall("reqRoomsByParams", nameOrIntro, isFull, isPlaying, hasPassword);
        }

        public void reqJoinRoom(ulong roomKey,string password)
        {
            Debug.LogFormat("请求加入房间，roomKey:{0},password:{1}", roomKey,password);
            baseCall("reqJoinRoom", roomKey,password);
        }

        public void reqEnterWorld()
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求加入房间");
                baseEntityCall.reqEnterWorld(long.Parse(Init.Datas["roomKey"].ToString()));
            }
        }

        public void reqRoomInfo()
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求房间信息");
                cellEntityCall.reqRoomInfo(long.Parse(Init.Datas["roomKey"].ToString()));
            }
        }

        public void reqRoomPassword(object roomKey)
        {
            Debug.LogFormat("请求房间密码,roomKey{0}", roomKey);
            baseCall("reqRoomPassword", roomKey);
        }

        public void reqEditRoomInfo(object roomKey,string name,string intro,string password)
        {
            Debug.LogFormat("请求修改房间信息,roomKey{0},name:{1},intro:{2},password:{3}",roomKey, name, intro, password);
            cellCall("reqEditRoomInfo",roomKey, name, intro);
            baseCall("reqEditRoomPassword", roomKey, password);
        }

        public void reqReadyPlay(object roomKey,int isReady)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求准备");
                cellCall("reqReadyPlay", roomKey, isReady);
            }
        }

        public void reqPlayGame(object roomKey)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求开始游戏");
                baseCall("reqPlayGame", roomKey);
            }
        }

        public void reqExitRoom(object roomKey)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求退出房间，roomKey:{0}", roomKey);
                baseCall("reqExitRoom", roomKey);
            }
        }

        public void reqGameInit(object roomKey)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求游戏初始化，roomKey:{0}", roomKey);
                cellCall("reqGameInit", roomKey);
            }
        }

        public void reqPlayersInfo(object roomKey)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求玩家信息，roomKey:{0}", roomKey);
                cellCall("reqPlayersInfo", roomKey);
            }
        }

        public void reqExitGame(object roomKey)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求退出游戏，roomKey:{0}", roomKey);
                baseCall("reqExitGame", roomKey);
            }
        }

        public void callPoint(int point)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求叫牌：{0}分", point);
                cellCall("callPoint", point);
            }
        }

        public void reqPlayCards(List<Int32> cardsList)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求出牌");
                string res = "";
                foreach(Int32 num in cardsList)
                {
                    res += num + ", ";
                }
                Debug.LogFormat("cardsList:{0}",res);
                cellEntityCall.reqPlayCards(cardsList);
            }
        }

        public void passCards()
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求过牌");
                cellCall("passCards", 4);
            }
        }

        public void reqAddCard(int color,int cardNum)
        {
            if (isPlayer())
            {
                Debug.LogFormat("请求加一张牌：color:{0},num:{1}",color,cardNum);
                cellCall("reqAddCard", color,cardNum);
            }
        }

        //--------------------------------------------
        //回调函数
        //--------------------------------------------

        public override void onEnterSetName(string name)
        {
            Debug.LogFormat("正在进入昵称设定界面");
            KBEngine.Event.fireOut("onEnterSetName");
        }

        public override void onEnterHall(UInt64 id)
        {
            Debug.Log("正在进入大厅");
            KBEngine.Event.fireOut("onEnterHall");
        }

        public override void onSetNameFailed(ushort retcode, string name)
        {
            Debug.LogFormat("设定昵称:{0} 失败，原因:{1}", name, KBEngineApp.app.serverErr((UInt16)retcode));
            KBEngine.Event.fireOut("onSetNameFailed", retcode,name);
        }

        public override void onSetNameSuccessfully(string name)
        {
            Debug.LogFormat("设定昵称:{0} 成功", name);
            KBEngine.Event.fireOut("onSetNameSuccessfully", name);
        }

        public override void onEnterWorld()
        {
            Debug.LogFormat("正在进入房间,roomKey:{0}", this.roomKey);
            base.onEnterWorld();
        }

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();
            if (isPlayer())
            {
                KBEngine.Event.fireOut("onLeaveWorld",this.name);
            }
        }

        public override void onReqRoomsByParams(ROOM_INFOS_LIST roomList)
        {
            Debug.LogFormat("room list:{0}",roomList.values);
            KBEngine.Event.fireOut("onReqRoomsByParams", roomList.values);
        }

        public override void onReqJoinRoomFailed(ushort retcode)
        {
            Debug.LogFormat("加入房间失败，原因：{0}", retcode);
            KBEngine.Event.fireOut("onReqJoinRoomFailed", retcode);
        }

        public override void onReqJoinRoomSuccess(ushort retcode)
        {
            Debug.LogFormat("加入房间成功，原因：{0}", retcode);
            KBEngine.Event.fireOut("onReqJoinRoomSuccess", this.roomKey);
        }

        public override void onReqRoomPassword(string password)
        {
            Debug.LogFormat("正在请求房间密码：{0}", password);
            KBEngine.Event.fireOut("onReqRoomPassword", password);
        }

        public override void onReqPlayersInfo(SEATS_INFO playersInfo)
        {
            if (isPlayer())
            {
                List<PLAYER_INFOS> playersInfoList = playersInfo.values;
                KBEngine.Event.fireOut("onReqPlayersInfo", playersInfoList);
            }
        }

        public override void onReqExitRoom(long entityId)
        {
            if(isPlayer())
            {
                Debug.LogFormat("正在离开房间，entityId:{0}", entityId);
                KBEngine.Event.fireOut("onReqExitRoom", entityId);
            }
        }

        //--------------------------------------------
        //属性设置回调
        //--------------------------------------------

        public override void onNameChanged(string oldValue)
        {
            Debug.LogFormat("设定昵称：{0}", this.name);
        }

        public override void onWinCountChanged(Int32 oldValue)
        {
            Debug.LogFormat("设定胜场：{0}", this.winCount);
        }

        public override void onLoseCountChanged(Int32 oldValue)
        {
            Debug.LogFormat("设定败场：{0}", this.loseCount);
        }

        public override void onHandCardsChanged(List<Int32> oldValue)
        {
            Debug.LogFormat("设定手牌：手牌:{0}", this.handCards);
            KBEngine.Event.fireOut("setHandCards", this.handCards);
        }

        public override void onHandCardsCountChanged(Int32 oldValue)
        {
            Debug.LogFormat("设定手牌数：座位:{0},手牌数:{1}", this.seatIndex, this.handCardsCount);
            KBEngine.Event.fireOut("setHandCardsCount", this.seatIndex, this.handCardsCount);
        }

        public override void onPlayCardsChanged(List<Int32> oldValue)
        {
            Debug.LogFormat("设定已出牌：座位:{0},已出牌:{1}", this.seatIndex, this.playCards);
            KBEngine.Event.fireOut("setPlayCards", this.seatIndex, this.playCards);
        }

        public override void onMessageChanged(Int32 oldValue)
        {
            Debug.LogFormat("设定消息：座位:{0},消息:{1}", this.seatIndex, this.message);
            KBEngine.Event.fireOut("setMessage", this.seatIndex, this.message);
        }

        public override void onGameResultChanged(Int32 oldVale)
        {
            Debug.LogFormat("设定游戏结果：{0}", this.gameResult);
            KBEngine.Event.fireOut("setGameResult", this.gameResult);
        }

        public override void onDestroy()
        {
            KBEngine.Event.deregisterIn(this);
        }
    }
}
