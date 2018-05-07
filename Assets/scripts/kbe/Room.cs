using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomBase {

	// Use this for initialization
	void Start () {
        KBEngine.Event.registerOut("set_roomName", this, "set_name");
        KBEngine.Event.registerOut("set_roomIntro", this, "set_intro");
        KBEngine.Event.registerOut("set_ready", this, "set_ready");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onNameChanged(string oldValue)
    {
        Debug.LogFormat("设置房间名：{0}", this.name);
        KBEngine.Event.fireOut("setRoomName", this.name);
    }

    public override void onIntroChanged(string oldValue)
    {
        Debug.LogFormat("设置房间简介：{0}", this.intro);
        KBEngine.Event.fireOut("setRoomIntro", this.intro);
    }

    public override void onIsPlayingChanged(Int16 oldValue)
    {
        Debug.LogFormat("设置正在游戏标识：{0}", this.isPlaying);
        KBEngine.Event.fireOut("setIsPlaying", this.isPlaying);
    }

    public override void onSeatsDataChanged(SEATS_INFO oldValue)
    {
        Debug.LogFormat("设置座位信息：{0}", this.seatsData);
        KBEngine.Event.fireOut("setSeatsData", this.seatsData.values);
    }

    public override void onRaiseIndexChanged(Int32 oldValue)
    {
        Debug.LogFormat("设置叫牌位：{0}", this.raiseIndex);
        KBEngine.Event.fireOut("setRaiseIndex", this.raiseIndex);
    }

    public override void onOpenCardChanged(Int16 oldValue)
    {
        Debug.LogFormat("设置明牌：{0}", this.openCard);
        KBEngine.Event.fireOut("setOpenCard", this.openCard);
    }

    public override void onHiddenCardsOpenChanged(List<Int32> oldValue)
    {
        Debug.LogFormat("设置打开底牌：{0}", this.hiddenCardsOpen);
        List<int> hiddenCardsOpen = this.hiddenCardsOpen;
        KBEngine.Event.fireOut("setHiddenCardsOpen", hiddenCardsOpen);
    }

    public override void onHighestPointChanged(Int16 oldValue)
    {
        Debug.LogFormat("设置最高分：{0}", this.highestPoint);
        KBEngine.Event.fireOut("setHighestPoint", this.highestPoint);
    }

    public override void onLandlordIndexChanged(Int16 oldValue)
    {
        Debug.LogFormat("设置地主位：{0}", this.landlordIndex);
        KBEngine.Event.fireOut("setLandlordIndex", this.landlordIndex);
    }

    public override void onPlayIndexChanged(Int16 oldValue)
    {
        Debug.LogFormat("设置出牌位：{0}", this.playIndex);
        KBEngine.Event.fireOut("setPlayIndex", this.playIndex);
    }

    public override void onLastPlayIndexChanged(short oldValue)
    {
        Debug.LogFormat("设置最后出牌位：{0}", this.lastPlayIndex);
        KBEngine.Event.fireOut("setLastPlayIndex", this.lastPlayIndex);
    }

    public override void onLastPlayCardsChanged(List<Int32> oldValue)
    {
        Debug.LogFormat("设置最后出牌：{0}", this.lastPlayCards);
        KBEngine.Event.fireOut("setLastPlayCards", this.lastPlayCards);
    }

    public override void onIsGameOverChanged(short oldVale)
    {
        {
            Debug.LogFormat("设置游戏结束标识：{0}", this.isGameOver);
            KBEngine.Event.fireOut("setIsGameOver", this.isGameOver);
        }
    }
}
