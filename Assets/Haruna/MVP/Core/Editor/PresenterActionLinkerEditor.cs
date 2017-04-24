using Haruna.UnityMVP.Model;
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

		int _tempIndex;

		public override void OnInspectorGUI()
		{
			if(_urls.Count == 0)
			{
				EditorGUILayout.HelpBox("Action is not exist!", MessageType.Info);
				return;
			}

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
			serializedObject.ApplyModifiedProperties();
		}

		void DrawParameters(PresenterActionInfo action)
		{
			EditorGUILayout.LabelField("Parameters");
			EditorGUI.indentLevel++;

			var binderProperties = serializedObject.FindProperty("_toSendDataBinders");
			var parameters = action.Method.GetParameters();
			if (parameters.Length == 0)
			{
				EditorGUILayout.HelpBox("No parameter to send", MessageType.Info);
			}
			else
			{
				var parameterLength = parameters.Length;
				if (action.IsAsync)
					parameterLength--;

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
			for (var i = parameters.Length; i < binderProperties.arraySize; i++)
			{
				binderProperties.DeleteArrayElementAtIndex(parameters.Length);
			}
			EditorGUI.indentLevel--;
		}

		void DrawResponse(PresenterActionInfo action)
		{
			EditorGUILayout.LabelField("Responese");
			EditorGUI.indentLevel++;

			Type retType = null;
			if (action.IsAsync)
			{
				var parameters = action.Method.GetParameters();
				var lastParam = parameters[parameters.Length - 1];
				if (lastParam.ParameterType.IsSubclassOf(typeof(AsyncReturn)))
				{
					var genericTypes = lastParam.ParameterType.GetGenericArguments();
					if(genericTypes.Length == 0)
					{
						retType = typeof(object);
					}
					else
					{
						retType = genericTypes[0];
					}
				}
				else
				{
					EditorGUILayout.HelpBox("it is an async action but the last parameter is not AysncReturn", MessageType.Error);
					return;
				}
			}
			else
			{
				retType = action.Method.ReturnType;
			}

			if (retType == null)
			{
				EditorGUILayout.HelpBox("No response data", MessageType.Info);
			}
			else
			{
				var binderListProperty = serializedObject.FindProperty("_responseDataBinder");
				if (binderListProperty.arraySize == 0)
					binderListProperty.InsertArrayElementAtIndex(0);

				var binderProperty = binderListProperty.GetArrayElementAtIndex(0);

				if (retType.GetInterface(typeof(IPresenterResponse).FullName) != null
					|| retType == typeof(MToken) || retType.IsSubclassOf(typeof(MToken)))
				{
					binderProperty.objectReferenceValue = EditorKit.DrawBinderField(
						"Return Value", "dynamic", binderProperty.objectReferenceValue, null);
				}
				else
				{
					var binderInfo = BinderUtil.GetRequireBinderInfoByValueType(retType);
					binderProperty.objectReferenceValue = EditorKit.DrawBinderField(
						"Return Value", binderInfo.ValueTypeName, binderProperty.objectReferenceValue, binderInfo.InterfaceType);
				}
			}
			EditorGUI.indentLevel--;
		}
	}
}