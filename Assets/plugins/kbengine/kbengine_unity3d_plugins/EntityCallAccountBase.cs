/*
	Generated by KBEngine!
	Please do not modify this file!
	
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Account.def
	public class EntityBaseEntityCall_AccountBase : EntityCall
	{
		public EntityBaseEntityCall_AccountBase() : base()
		{
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_BASE;
		}

		public void reqCreateRoom(string arg1, string arg2, string arg3)
		{
			Bundle pBundle = newCall("reqCreateRoom");
			if(pBundle == null)
				return;

			bundle.writeUnicode(arg1);
			bundle.writeUnicode(arg2);
			bundle.writeUnicode(arg3);
			sendCall(null);
		}

		public void reqEditRoomPassword(Int64 arg1, string arg2)
		{
			Bundle pBundle = newCall("reqEditRoomPassword");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			bundle.writeUnicode(arg2);
			sendCall(null);
		}

		public void reqEnterWorld(Int64 arg1)
		{
			Bundle pBundle = newCall("reqEnterWorld");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqExitGame(Int64 arg1)
		{
			Bundle pBundle = newCall("reqExitGame");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqExitRoom(Int64 arg1)
		{
			Bundle pBundle = newCall("reqExitRoom");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqJoinRoom(Int64 arg1, string arg2)
		{
			Bundle pBundle = newCall("reqJoinRoom");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			bundle.writeUnicode(arg2);
			sendCall(null);
		}

		public void reqPlayGame(Int64 arg1)
		{
			Bundle pBundle = newCall("reqPlayGame");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqRoomPassword(Int64 arg1)
		{
			Bundle pBundle = newCall("reqRoomPassword");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqRoomsByParams(string arg1, Int16 arg2, Int16 arg3, Int16 arg4)
		{
			Bundle pBundle = newCall("reqRoomsByParams");
			if(pBundle == null)
				return;

			bundle.writeUnicode(arg1);
			bundle.writeInt16(arg2);
			bundle.writeInt16(arg3);
			bundle.writeInt16(arg4);
			sendCall(null);
		}

		public void reqSetName(string arg1)
		{
			Bundle pBundle = newCall("reqSetName");
			if(pBundle == null)
				return;

			bundle.writeUnicode(arg1);
			sendCall(null);
		}

	}

	public class EntityCellEntityCall_AccountBase : EntityCall
	{
		public EntityCellEntityCall_AccountBase() : base()
		{
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_CELL;
		}

		public void callPoint(Int32 arg1)
		{
			Bundle pBundle = newCall("callPoint");
			if(pBundle == null)
				return;

			bundle.writeInt32(arg1);
			sendCall(null);
		}

		public void passCards(Int32 arg1)
		{
			Bundle pBundle = newCall("passCards");
			if(pBundle == null)
				return;

			bundle.writeInt32(arg1);
			sendCall(null);
		}

		public void reqAddCard(Int32 arg1, Int32 arg2)
		{
			Bundle pBundle = newCall("reqAddCard");
			if(pBundle == null)
				return;

			bundle.writeInt32(arg1);
			bundle.writeInt32(arg2);
			sendCall(null);
		}

		public void reqEditRoomInfo(Int64 arg1, string arg2, string arg3)
		{
			Bundle pBundle = newCall("reqEditRoomInfo");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			bundle.writeUnicode(arg2);
			bundle.writeUnicode(arg3);
			sendCall(null);
		}

		public void reqGameInit(Int64 arg1)
		{
			Bundle pBundle = newCall("reqGameInit");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqPlayCards(List<Int32> arg1)
		{
			Bundle pBundle = newCall("reqPlayCards");
			if(pBundle == null)
				return;

			((DATATYPE_AnonymousArray_30)EntityDef.id2datatypes[30]).addToStreamEx(bundle, arg1);
			sendCall(null);
		}

		public void reqPlayersInfo(Int64 arg1)
		{
			Bundle pBundle = newCall("reqPlayersInfo");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

		public void reqReadyPlay(Int64 arg1, Int32 arg2)
		{
			Bundle pBundle = newCall("reqReadyPlay");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			bundle.writeInt32(arg2);
			sendCall(null);
		}

		public void reqRoomInfo(Int64 arg1)
		{
			Bundle pBundle = newCall("reqRoomInfo");
			if(pBundle == null)
				return;

			bundle.writeInt64(arg1);
			sendCall(null);
		}

	}
	}