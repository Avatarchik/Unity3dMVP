using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[InitializeOnLoad]
	public class HierarchyViewEditor
	{
		static HierarchyViewEditor()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
		}
		
		static float _iconWidth = 14f;
		static float _iconHeight = 13f;
		static float _space = 2f;

		static void OnHierarchyGUI(int instanceID, Rect selectionRect)
		{
			var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (obj == null)
				return;

			try
			{
				var toDrawList = GetToDrawIcon(obj);
				if (IsChildrenHasError(obj))
					toDrawList.Insert(0, GetErrorIcon());
				DrawIcon(toDrawList, selectionRect);
			}
			catch(Exception e)
			{
				Debug.LogException(e);
			}
		}

		static List<Sprite> GetToDrawIcon(GameObject obj)
		{
			List<Sprite> toDrawList = new List<Sprite>();
			{
				var actionLinker = obj.GetComponent<PresenterActionLinker>();
				var eventLinker = obj.GetComponent<PresenterEventLinker>();
				if ((actionLinker != null && actionLinker.HasEditorError())
					|| eventLinker != null && eventLinker.HasEditorError())
				{
					toDrawList.Add(GetLinkerIcon(true));
				}
				else if (EditorPrefs.GetBool(PreferenceEditor.ShowLinkerIconInHierarchy, true)
					&& (actionLinker != null || eventLinker != null))
				{
					toDrawList.Add(GetLinkerIcon(false));
				}
			}
			{
				var binder = obj.GetComponent<Model.IMvpTokenBinder>();
				if (binder != null)
				{
					if (binder.HasEditorError())
						toDrawList.Add(GetObjectBinderIcon(true));
					else if (EditorPrefs.GetBool(PreferenceEditor.ShowObjectBinderIconInHierarchy, true))
						toDrawList.Add(GetObjectBinderIcon(false));
				}
			}
			{
				var binder = obj.GetComponent<Model.IMvpObjectBinder>();
				if (binder != null)
				{
					if (binder.HasEditorError())
						toDrawList.Add(GetObjectBinderIcon(true));
					else if (EditorPrefs.GetBool(PreferenceEditor.ShowObjectBinderIconInHierarchy, true))
						toDrawList.Add(GetObjectBinderIcon(false));
				}
			}

			{
				var binder = obj.GetComponent<Model.IMvpArrayBinder>();
				if (binder != null)
				{
					if (binder.HasEditorError())
						toDrawList.Add(GetArrayBinderIcon(true));
					else if (EditorPrefs.GetBool(PreferenceEditor.ShowArrayBinderIconInHierarchy, true))
						toDrawList.Add(GetArrayBinderIcon(false));
				}
			}
			{
				var binder = obj.GetComponent<Model.IMvpBoolBinder>();
				if (binder != null)
				{
					if (binder.HasEditorError())
						toDrawList.Add(GetValueBinderIcon(true));
					else if (EditorPrefs.GetBool(PreferenceEditor.ShowBoolBinderIconInHierarchy, false))
						toDrawList.Add(GetValueBinderIcon(false));
				}
			}
			{
				var binder = obj.GetComponent<Model.IMvpFloatBinder>();
				if (binder != null)
				{
					if (binder.HasEditorError())
						toDrawList.Add(GetValueBinderIcon(true));
					else if (EditorPrefs.GetBool(PreferenceEditor.ShowFloatBinderIconInHierarchy, false))
						toDrawList.Add(GetValueBinderIcon(false));
				}
			}
			{
				var binder = obj.GetComponent<Model.IMvpStringBinder>();
				if (binder != null)
				{
					if (binder.HasEditorError())
						toDrawList.Add(GetValueBinderIcon(true));
					else if (EditorPrefs.GetBool(PreferenceEditor.ShowStringBinderIconInHierarchy, false))
						toDrawList.Add(GetValueBinderIcon(false));
				}
			}
			{
				var binders = obj.GetComponents<Component>();
				if (binders.Any(b =>
				{
					if (b == null) return false;

					if (b.GetType().GetInterface(typeof(Model.IMvpCustomTypeBinder<>).FullName) == null)
						return false;
					var method = b.GetType().GetMethod("HasEditorError");
					if (method == null || method.ReturnType != typeof(bool))
						return false;
					return (bool)method.Invoke(b, null);
				}))
					toDrawList.Add(GetValueBinderIcon(true));
				else if (EditorPrefs.GetBool(PreferenceEditor.ShowCustomBinderIconInHierarchy, false)
					&& binders.Any(b => b.GetType().GetInterface(typeof(Model.IMvpCustomTypeBinder<>).FullName) != null))
					toDrawList.Add(GetValueBinderIcon(false));
			}

			return toDrawList;
		}

		static bool IsChildrenHasError(GameObject go)
		{
			{
				var linkers = go.GetComponentsInChildren<PresenterActionLinker>(true);
				if (linkers.Any(l => l.gameObject != go && l.HasEditorError()))
					return true;
			}
			{
				var linkers = go.GetComponentsInChildren<PresenterEventLinker>(true);
				if (linkers.Any(l => l.gameObject != go && l.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Model.IMvpTokenBinder>(true);
				if (binders.Any(b => ((Component)b).gameObject != go && b.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Model.IMvpObjectBinder>(true);
				if (binders.Any(b => ((Component)b).gameObject != go && b.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Model.IMvpArrayBinder>(true);
				if (binders.Any(b => ((Component)b).gameObject != go && b.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Model.IMvpBoolBinder>(true);
				if (binders.Any(b => ((Component)b).gameObject != go && b.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Model.IMvpFloatBinder>(true);
				if (binders.Any(b => ((Component)b).gameObject != go && b.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Model.IMvpStringBinder>(true);
				if (binders.Any(b => ((Component)b).gameObject != go && b.HasEditorError()))
					return true;
			}
			{
				var binders = go.GetComponentsInChildren<Component>(true);
				if (binders.Any(b =>
				{
					if (b == null) return true;

					if (b.GetType().GetInterface(typeof(Model.IMvpCustomTypeBinder<>).FullName) == null)
						return false;
					var method = b.GetType().GetMethod("HasEditorError");
					if (method == null || method.ReturnType != typeof(bool))
						return false;
					return (bool)method.Invoke(b, null);
				}))
					return true;
			}

			return false;
		}

		static void DrawIcon(List<Sprite> toDrawList, Rect selectionRect)
		{
			var wdithToRight = EditorPrefs.GetFloat(PreferenceEditor.SpaceToHerarchyEnd, 0);

			var rect = selectionRect;
			rect.x = rect.x + rect.width - wdithToRight;
			rect.width = _iconWidth;
			rect.y += (rect.height - _iconHeight) / 2f;
			rect.height = _iconHeight;

			foreach (var toDraw in toDrawList)
			{
				rect.x -= _iconWidth;
				GUI.DrawTexture(rect, toDraw.texture, ScaleMode.StretchToFill);
				rect.x -= _space;
			}
		}

		const string PathPrefix = "Assets/Haruna/MVP/Core/Icon/";
		static Sprite GetLinkerIcon(bool error)
		{
			string path = PathPrefix + (error ? "Errored" : "") + "Linker.png";
			return AssetDatabase.LoadAssetAtPath<Sprite>(path);
		}
		static Sprite GetObjectBinderIcon(bool error)
		{
			string path = PathPrefix + (error ? "Errored" : "") + "ObjectBinder.png";
			return AssetDatabase.LoadAssetAtPath<Sprite>(path);
		}
		static Sprite GetArrayBinderIcon(bool error)
		{
			string path = PathPrefix + (error ? "Errored" : "") + "ArrayBinder.png";
			return AssetDatabase.LoadAssetAtPath<Sprite>(path);
		}
		static Sprite GetValueBinderIcon(bool error)
		{
			string path = PathPrefix + (error ? "Errored" : "") + "ValueBinder.png";
			return AssetDatabase.LoadAssetAtPath<Sprite>(path);
		}
		static Sprite GetErrorIcon()
		{
			return AssetDatabase.LoadAssetAtPath<Sprite>(PathPrefix + "Error.png");
		}

	}
}