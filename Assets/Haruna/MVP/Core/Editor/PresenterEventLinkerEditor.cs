using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[CustomEditor(typeof(PresenterEventLinker), true)]
	public class PresenterEventLinkerEditor : Editor
	{
		List<PresenterEventInfo> _allEvents;
		List<string> _urls;
		string[] _displayUrls;

		void OnEnable()
		{
			_allEvents = PresenterUtil.GetAllPresetnerEvents().Values.ToList();
			_urls = _allEvents.Select(a => a.Url).ToList();
			_displayUrls = _allEvents.Select(a => string.IsNullOrEmpty(a.DisplayUrl) ? a.Url : a.DisplayUrl).ToArray();
		}

		int _selectedPageIndex;
		int _tempIndex;

		public override void OnInspectorGUI()
		{
			if(_urls.Count == 0)
			{
				EditorGUILayout.HelpBox("Event is not exist!", MessageType.Info);
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
					EditorGUILayout.HelpBox("Event is deleted or changed its name.\n" + urlProp.stringValue, MessageType.Error);
					if (GUILayout.Button("Reset Serialized Value"))
					{
						urlProp.stringValue = "";
					}

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					_tempIndex = EditorGUILayout.Popup(_tempIndex, _displayUrls);
					if (GUILayout.Button("Set As New"))
					{
						urlProp.stringValue = _allEvents[_tempIndex].Url;
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
					DrawParameters(_allEvents[index]);
				}
			}
			serializedObject.ApplyModifiedProperties();
		}

		void DrawParameters(PresenterEventInfo action)
		{
			EditorGUILayout.LabelField("Parameters");
			EditorGUI.indentLevel++;

			var parameters = action.Field.FieldType.GetGenericArguments();

			var binderProperties = serializedObject.FindProperty("_eventParameterBinders");
			while (binderProperties.arraySize > parameters.Length)
				binderProperties.DeleteArrayElementAtIndex(parameters.Length);

			if (parameters.Length == 0)
			{
				EditorGUILayout.HelpBox("No parameter data", MessageType.Error);
				return;
			}
			else
			{
				for (var i = 0; i < parameters.Length; i++)
				{
					if (binderProperties.arraySize <= i)
						binderProperties.InsertArrayElementAtIndex(i);

					var binderProperty = binderProperties.GetArrayElementAtIndex(i);
					var parameterType = parameters[i];
					
					var requireBinderInfo = BinderUtil.GetRequireBinderInfoByValueType(parameterType);
					binderProperty.objectReferenceValue = EditorKit.DrawBinderField(
						i.ToString(), requireBinderInfo.ValueTypeName, binderProperty.objectReferenceValue, requireBinderInfo.InterfaceType);
				}
			}
			EditorGUI.indentLevel--;
		}
	}
}