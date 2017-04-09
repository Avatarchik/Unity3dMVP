using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[CustomEditor(typeof(PresenterLinker), true)]
	public class PresenterLinkerEditor : Editor
	{
		List<PresenterAction> _allActions;
		List<string> _urls;

		void OnEnable()
		{
			_allActions = PresenterUtil.GetAllPresenterAction().Values.ToList();
			_urls = _allActions.Select(a => a.Url).ToList();
		}

		public override void OnInspectorGUI()
		{
			var urlProp = serializedObject.FindProperty("_url");
			int index = 0;
			if (!string.IsNullOrEmpty(urlProp.stringValue))
				index = _urls.FindIndex(u => u == urlProp.stringValue);
			if (index < 0)
			{
				EditorGUILayout.HelpBox("action is not exist!\n" + urlProp.stringValue, MessageType.Error);
			}
			else
			{
				index = EditorGUILayout.Popup("Presenter", index, _urls.ToArray());
				urlProp.stringValue = _urls[index];
			}

			if (index >= 0)
			{
				DrawParameters(_allActions[index]);
				DrawResponse(_allActions[index]);
			}
			serializedObject.ApplyModifiedProperties();
		}

		void DrawParameters(PresenterAction action)
		{
			EditorGUILayout.LabelField("Parameters");
			EditorGUI.indentLevel++;

			var parameters = action.Method.GetParameters();
			if (parameters.Length == 0)
			{
				EditorGUILayout.HelpBox("No parameter to send", MessageType.Info);
				return;
			}
			else
			{
				var binderProperties = serializedObject.FindProperty("_toSendDataBinders");

				for(var i = 0; i < parameters.Length; i++)
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
				for(var i = parameters.Length; i < binderProperties.arraySize; i++)
				{
					binderProperties.DeleteArrayElementAtIndex(parameters.Length);
				}
			}
			EditorGUI.indentLevel--;
		}

		void DrawResponse(PresenterAction action)
		{
			EditorGUILayout.LabelField("Responese");
			EditorGUI.indentLevel++;
			var retType = action.Method.ReturnType;
			if (retType == null)
			{
				EditorGUILayout.HelpBox("No response data", MessageType.Info);
			}
			else
			{
				var binderProperty = serializedObject.FindProperty("_responseDataBinder");
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
						"Return Value", binderInfo.ValueTypeName, binderProperty.objectReferenceValue, retType);
				}
			}
			EditorGUI.indentLevel--;
		}
	}
}