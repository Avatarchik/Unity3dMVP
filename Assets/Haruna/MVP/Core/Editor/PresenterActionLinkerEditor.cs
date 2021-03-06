﻿using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[CustomEditor(typeof(PresenterActionLinker), true)]
	public class PresenterActionLinkerEditor : Editor
	{
		List<PresenterActionInfo> _allActions;
		List<string> _urls;
		string[] _displayUrls;

		void OnEnable()
		{
			_allActions = PresenterUtil.GetAllPresenterAction().Values.ToList();
			_urls = _allActions.Select(a => a.Url).ToList();
			_displayUrls = _allActions.Select(a => string.IsNullOrEmpty(a.DisplayUrl) ? a.Url : a.DisplayUrl).ToArray();
		}

		int _selectedPageIndex;
		int _tempIndex;

		public override void OnInspectorGUI()
		{
			if(_urls.Count == 0)
			{
				EditorGUILayout.HelpBox("Action is not exist!", MessageType.Info);
				return;
			}

			_selectedPageIndex = GUILayout.Toolbar(_selectedPageIndex, new string[] { "Settings", "Events" }, EditorStyles.miniButton);
			EditorGUILayout.Space();
			Rect rect = EditorGUILayout.GetControlRect(false, 1f);
			EditorGUI.DrawRect(rect, EditorStyles.label.normal.textColor * 0.5f);
			EditorGUILayout.Space();

			if (_selectedPageIndex == 1)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_beforeReceiveData"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_afterReceiveData"));
			}
			else
			{
				var urlProp = serializedObject.FindProperty("_url");
				int index = 0;
				if (!string.IsNullOrEmpty(urlProp.stringValue))
					index = _urls.FindIndex(u => u == urlProp.stringValue);
				if (index < 0)
				{
					EditorGUILayout.HelpBox("Action is deleted or changed its name.\n" + urlProp.stringValue, MessageType.Error);
					if (GUILayout.Button("Reset Serialized Value"))
					{
						urlProp.stringValue = "";
					}

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					_tempIndex = EditorGUILayout.Popup(_tempIndex, _displayUrls);
					if (GUILayout.Button("Set As New"))
					{
						urlProp.stringValue = _allActions[_tempIndex].Url;
					}
					EditorGUILayout.EndHorizontal();
				}
				else
				{
					index = EditorGUILayout.Popup("Presenter", index, _displayUrls);
					urlProp.stringValue = _urls[index];
				}

				if (index >= 0)
				{
					var isAsyncProperty = serializedObject.FindProperty("_async");
					isAsyncProperty.boolValue = _allActions[index].IsAsync;

					DrawParameters(_allActions[index]);
					DrawResponse(_allActions[index]);
				}
			}
			serializedObject.ApplyModifiedProperties();
		}

		void DrawParameters(PresenterActionInfo action)
		{
			EditorGUILayout.LabelField("Parameters");
			EditorGUI.indentLevel++;

			var binderProperties = serializedObject.FindProperty("_toSendDataBinders");
			var parameters = action.Method.GetParameters();

			var parameterLength = parameters.Length;
			if (action.IsAsync)
				parameterLength--;

			if (parameterLength == 0)
			{
				EditorGUILayout.HelpBox("No parameter to send", MessageType.Info);
			}
			else
			{
				for(var i = 0; i < parameterLength; i++)
				{
					if (binderProperties.arraySize <= i)
						binderProperties.InsertArrayElementAtIndex(i);

					var binderProperty = binderProperties.GetArrayElementAtIndex(i);
					var parameter = parameters[i];

					var paraType = parameter.ParameterType;
					var requireBinderInfo = BinderUtil.GetRequireBinderInfoByValueType(paraType);
					binderProperty.objectReferenceValue = EditorKit.DrawBinderField(
						parameter.Name, requireBinderInfo.ValueTypeName, binderProperty.objectReferenceValue, requireBinderInfo.InterfaceType);
				}
			}
			while (binderProperties.arraySize > parameters.Length)
				binderProperties.DeleteArrayElementAtIndex(parameters.Length);

			EditorGUI.indentLevel--;
		}

		void DrawResponse(PresenterActionInfo action)
		{
			EditorGUILayout.LabelField("Responese");
			EditorGUI.indentLevel++;

			List<Type> retTypeList = new List<Type>();
			if (action.IsAsync)
			{
				var parameters = action.Method.GetParameters();
				var lastParam = parameters[parameters.Length - 1];
				if (lastParam.ParameterType == typeof(AsyncReturn) || lastParam.ParameterType.IsSubclassOf(typeof(AsyncReturn)))
				{
					var genericTypes = lastParam.ParameterType.GetGenericArguments();
					foreach(var gt in genericTypes)
					{
						retTypeList.Add(gt);
					}
				}
				else
				{
					EditorGUILayout.HelpBox("it is an async action but the last parameter is not AysncReturn", MessageType.Error);
					return;
				}
			}
			else if(action.Method.ReturnType != typeof(void))
			{
				retTypeList.Add(action.Method.ReturnType);
			}

			var binderListProperty = serializedObject.FindProperty("_responseDataBinder");
			while (binderListProperty.arraySize > retTypeList.Count)
				binderListProperty.DeleteArrayElementAtIndex(retTypeList.Count);

			if (retTypeList.Count == 0)
			{
				EditorGUILayout.HelpBox("No response data", MessageType.Info);
			}
			else
			{
				for (var i = 0; i < retTypeList.Count; i++)
				{
					if (binderListProperty.arraySize <= i)
						binderListProperty.InsertArrayElementAtIndex(i);

					var binderProperty = binderListProperty.GetArrayElementAtIndex(i);
					var retType = retTypeList[i];
					
					var requireBinderInfo = BinderUtil.GetRequireBinderInfoByValueType(retType);
					
					binderProperty.objectReferenceValue = EditorKit.DrawBinderField(
						"return " + i, requireBinderInfo.ValueTypeName, binderProperty.objectReferenceValue, requireBinderInfo.InterfaceType);
				}
			}

			EditorGUI.indentLevel--;
		}
	}
}