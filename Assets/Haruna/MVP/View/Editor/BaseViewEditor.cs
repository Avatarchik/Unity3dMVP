using UnityEditor;
using UnityEngine;

namespace Haruna.Unity.View
{
	[CustomEditor(typeof(BaseView), true)]
	public class BaseViewEditor : Editor
	{
		int _selectedPageIndex;
		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space();
			_selectedPageIndex = GUILayout.Toolbar(_selectedPageIndex, new string[] { "Settings", "Events" }, EditorStyles.miniButton);
			EditorGUILayout.Space();
			Rect rect = EditorGUILayout.GetControlRect(false, 1f);
			EditorGUI.DrawRect(rect, EditorStyles.label.normal.textColor * 0.5f);
			serializedObject.Update();
			EditorGUILayout.Space();

			switch (_selectedPageIndex)
			{
				case 0:
					//EditorGUILayout.PropertyField(serializedObject.FindProperty("_ignoreTimeScale"));
					//EditorGUILayout.PropertyField(serializedObject.FindProperty("_delay"));
					var property = serializedObject.FindProperty("_hideEvent");
					while (property.NextVisible(false))
					{
						EditorGUILayout.PropertyField(property, true);
					}

					EditorGUILayout.Space();
					break;
				case 1:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_initializeEvent"));
					EditorGUILayout.Space();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_showEvent"));
					EditorGUILayout.Space();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_hideEvent"));
					EditorGUILayout.Space();
					break;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}