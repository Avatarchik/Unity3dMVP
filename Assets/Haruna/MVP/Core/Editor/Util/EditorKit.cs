﻿using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Haruna.UnityMVP.Model;

namespace Haruna.UnityMVP
{
	public class EditorKit
	{
		public struct SerializedType
		{
			public string TypeString;
			public Type Type;

			public SerializedType(Type t)
			{
				this.Type = t;
				this.TypeString = TypeUtil.GetAssemblyTypeString(t);
			}
		}

		static int _tempIndex;
		public static SerializedType? DrawTypeSelector(string labelName, string typeString, List<SerializedType> typesList)
		{
			var displayString = typesList.Select(s =>
			{
				var temp = s.TypeString.Split(';')[0];
				var lastDot = temp.LastIndexOf('.');
				if(lastDot >= 0)
				{
					temp = temp.Substring(0, lastDot) + "/" + temp.Substring(lastDot + 1);
				}
				return temp;
			}).ToArray();

			var index = typesList.FindIndex(t => t.TypeString == typeString);
			if (!string.IsNullOrEmpty(typeString) && index < 0)
			{
				EditorGUILayout.HelpBox("Type is deleted or changed its name.\n" + typeString.Replace(';', '\n'), MessageType.Error);
				if (GUILayout.Button("Reset Serialized Value"))
				{
					return typesList[0];
				}

				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal();
				_tempIndex = EditorGUILayout.Popup(_tempIndex, displayString);
				if (GUILayout.Button("Set As New"))
				{
					return typesList[_tempIndex];
				}
				EditorGUILayout.EndHorizontal();

				return null;
			}
			else
			{
				if (index < 0) index = 0;

				if (string.IsNullOrEmpty(labelName))
					index = EditorGUILayout.Popup(index, displayString);
				else
					index = EditorGUILayout.Popup(labelName, index, displayString);
				return typesList[index];
			}
		}
		
		public static UnityEngine.Object DrawBinderField(string label, string valueTypeName, UnityEngine.Object value, Type binderInterfaceType)
		{
			EditorGUILayout.LabelField(label, valueTypeName);
			EditorGUILayout.BeginHorizontal();
			//EditorGUI.indentLevel++;
			var ret = DrawBinderField(" ",value, binderInterfaceType);
			EditorGUILayout.EndHorizontal();
			//EditorGUI.indentLevel--;
			return ret;
		}

		//public static UnityEngine.Object DrawBinderField(string valueTypeName, UnityEngine.Object value, Type binderInterfaceType)
		//{
		//	EditorGUILayout.BeginHorizontal();
		//	EditorGUILayout.PrefixLabel(valueTypeName);
		//	var ret = DrawBinderField(value, binderInterfaceType);
		//	EditorGUILayout.EndHorizontal();
		//	return ret;
		//}

		public static UnityEngine.Object DrawBinderField(string valueTypeName, UnityEngine.Object value, Type binderInterfaceType)
		{
			UnityEngine.Object returnValue = value;

			var temp = EditorGUILayout.ObjectField(valueTypeName, value, typeof(Component), true) as Component;
			if (temp == null)
			{
				returnValue = null;
			}
			else if(binderInterfaceType == null)
			{
				var interfaceType = GetImplementedBinderInterface(value.GetType());
				if(interfaceType != null)
				{
					var otherCom = temp.GetComponent(interfaceType);
					if (otherCom != null)
						returnValue = otherCom;
				}
				if(returnValue == null)
				{
					if (value is Component)
					{
						var otherCom = GetMvpBinder(((Component)value).gameObject);
						if (otherCom != null)
							return otherCom;
					}
				}
			}
			else
			{
				if (temp.GetType().GetInterface(binderInterfaceType.FullName) != null || temp is IMvpTokenBinder)
				{
					returnValue = temp;
				}
				else
				{
					var otherCom = temp.GetComponent(binderInterfaceType);
					if (otherCom == null)
						otherCom = temp.GetComponent(typeof(IMvpTokenBinder));

					returnValue = otherCom;
				}
			}
			return returnValue;
		}

		static List<Type> binderInterfaces = new List<Type>()
		{
			typeof(IMvpStringBinder),
			typeof(IMvpFloatBinder),
			typeof(IMvpBoolBinder),
			typeof(IMvpObjectBinder),
			typeof(IMvpArrayBinder),
			typeof(IMvpCustomTypeBinder<>),
			typeof(IMvpTokenBinder),
		};
		static Type GetImplementedBinderInterface(Type type)
		{
			for(var i = 0; i < binderInterfaces.Count; i++)
			{
				var t = type.GetInterface(binderInterfaces[i].FullName, false);
				if (t != null)
					return t;
			}
			return null;
		}

		public static UnityEngine.Object GetMvpBinder(GameObject go)
		{
			for (var i = 0; i < binderInterfaces.Count; i++)
			{
				var t = go.GetComponent(binderInterfaces[i]);
				if (t != null)
					return t;
			}
			return null;
		}
	}
}
