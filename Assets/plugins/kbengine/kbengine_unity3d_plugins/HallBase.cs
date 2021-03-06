/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Hall : HallBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Hall.def
	public abstract class HallBase : Entity
	{
		public EntityBaseEntityCall_HallBase baseEntityCall = null;
		public EntityCellEntityCall_HallBase cellEntityCall = null;



		public override void onGetBase()
		{
			baseEntityCall = new EntityBaseEntityCall_HallBase();
			baseEntityCall.id = id;
			baseEntityCall.className = className;
		}

		public override void onGetCell()
		{
			cellEntityCall = new EntityCellEntityCall_HallBase();
			cellEntityCall.id = id;
			cellEntityCall.className = className;
		}

		public override void onLoseCell()
		{
			cellEntityCall = null;
		}

		public override EntityCall getBaseEntityCall()
		{
			return baseEntityCall;
		}

		public override EntityCall getCellEntityCall()
		{
			return cellEntityCall;
		}

		public override void onRemoteMethodCall(Method method, MemoryStream stream)
		{
			switch(method.methodUtype)
			{
				default:
					break;
			};
		}

		public override void onUpdatePropertys(Property prop, MemoryStream stream)
		{
			switch(prop.properUtype)
			{
				case 40001:
					Vector3 oldval_direction = direction;
					direction = stream.readVector3();

					if(prop.isBase())
					{
						if(inited)
							onDirectionChanged(oldval_direction);
					}
					else
					{
						if(inWorld)
							onDirectionChanged(oldval_direction);
					}

					break;
				case 40000:
					Vector3 oldval_position = position;
					position = stream.readVector3();

					if(prop.isBase())
					{
						if(inited)
							onPositionChanged(oldval_position);
					}
					else
					{
						if(inWorld)
							onPositionChanged(oldval_position);
					}

					break;
					case 40002:
						stream.readUint32();
						break;
				default:
					break;
			};
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs[className];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			Vector3 oldval_direction = direction;
			Property prop_direction = pdatas[1];
			if(prop_direction.isBase())
			{
				if(inited && !inWorld)
					onDirectionChanged(oldval_direction);
			}
			else
			{
				if(inWorld)
				{
					if(prop_direction.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onDirectionChanged(oldval_direction);
					}
				}
			}

			Vector3 oldval_position = position;
			Property prop_position = pdatas[0];
			if(prop_position.isBase())
			{
				if(inited && !inWorld)
					onPositionChanged(oldval_position);
			}
			else
			{
				if(inWorld)
				{
					if(prop_position.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onPositionChanged(oldval_position);
					}
				}
			}

		}
	}
}