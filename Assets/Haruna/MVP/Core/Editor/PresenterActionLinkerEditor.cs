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

		void OnEnable()
		{
			_allActions = PresenterUtil.GetAllPresenterAction().Values.ToList();
			_urls = _allActions.Select(a => a.Url).ToList();
		}

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
				if (GUILayout.Button("Reset"))
				{
					urlProp.stringValue = "";
				}
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
						"Return Value", binderInfo.ValueTypeName, binderProperty.objectReferenceValue, binderInfo.InterfaceType);
				}
			}
			EditorGUI.indentLevel--;
		}
	}
}