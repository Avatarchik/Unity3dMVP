using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[InitializeOnLoad]
	public class PreferenceEditor
	{
		public const string SpaceToHerarchyEnd = "Haruna_UnityMVP_SpaceToHerarchyEnd";
		public const string ShowLinkerIconInHierarchy = "Haruna_UnityMVP_ShowLinkerIconInHierarchy";
		public const string ShowObjectBinderIconInHierarchy = "Haruna_UnityMVP_ShowModelBinderIconInHierarchy";
		public const string ShowArrayBinderIconInHierarchy = "Haruna_UnityMVP_ShowArrayBinderIconInHierarchy";
		public const string ShowFloatBinderIconInHierarchy = "Haruna_UnityMVP_ShowFloatBinderIconInHierarchy";
		public const string ShowStringBinderIconInHierarchy = "Haruna_UnityMVP_ShowStringBinderIconInHierarchy";
		public const string ShowBoolBinderIconInHierarchy = "Haruna_UnityMVP_ShowBoolBinderIconInHierarchy";
		public const string ShowCustomBinderIconInHierarchy = "Haruna_UnityMVP_ShowCustomBinderIconInHierarchy";

		static Vector2 _scrollPos;
		[PreferenceItem("Unity3d MVP")]
		static void OnPrefsGUI()
		{
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			{
				var value = EditorPrefs.GetFloat(SpaceToHerarchyEnd);
				value = EditorGUILayout.FloatField("Space To Hierachy End", value);
				EditorPrefs.SetFloat(SpaceToHerarchyEnd, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowLinkerIconInHierarchy, true);
				value = EditorGUILayout.Toggle("Show Linker Icon", value);
				EditorPrefs.SetBool(ShowLinkerIconInHierarchy, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowObjectBinderIconInHierarchy, true);
				value = EditorGUILayout.Toggle("Show Object Binder Icon", value);
				EditorPrefs.SetBool(ShowObjectBinderIconInHierarchy, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowArrayBinderIconInHierarchy, true);
				value = EditorGUILayout.Toggle("Show Array Binder Icon", value);
				EditorPrefs.SetBool(ShowArrayBinderIconInHierarchy, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowBoolBinderIconInHierarchy, false);
				value = EditorGUILayout.Toggle("Show Bool Binder Icon", value);
				EditorPrefs.SetBool(ShowBoolBinderIconInHierarchy, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowStringBinderIconInHierarchy, false);
				value = EditorGUILayout.Toggle("Show String Binder Icon", value);
				EditorPrefs.SetBool(ShowStringBinderIconInHierarchy, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowFloatBinderIconInHierarchy, false);
				value = EditorGUILayout.Toggle("Show Float Binder Icon", value);
				EditorPrefs.SetBool(ShowFloatBinderIconInHierarchy, value);
			}
			{
				var value = EditorPrefs.GetBool(ShowCustomBinderIconInHierarchy, false);
				value = EditorGUILayout.Toggle("Show Custom Binder Icon", value);
				EditorPrefs.SetBool(ShowCustomBinderIconInHierarchy, value);
			}
			EditorGUILayout.EndScrollView();
		}
	}
}