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
		
		static float _iconWidth = 10f;
		static float _iconHeight = 9f;
		static float _space = 2f;

		static void OnHierarchyGUI(int instanceID, Rect selectionRect)
		{
			var wdithToRight = EditorPrefs.GetFloat(PreferenceEditor.SpaceToHerarchyEnd, 0);

			var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (obj == null)
				return;

			List<DrawIcon> toDrawList = new List<DrawIcon>();

			var actionLinker = obj.GetComponent<PresenterActionLinker>();
			var eventLinker = obj.GetComponent<PresenterEventLinker>();
			if(EditorPrefs.GetBool(PreferenceEditor.ShowLinkerIconInHierarchy, true)
				&& (actionLinker != null || eventLinker != null))
			{
				toDrawList.Add(new DrawIcon() { IconColor = Color.red });
			}

			{
				var binder = obj.GetComponent<Model.IMvpObjectBinder>();
				if (EditorPrefs.GetBool(PreferenceEditor.ShowObjectBinderIconInHierarchy, true) && binder != null)
					toDrawList.Add(new DrawIcon() { IconColor = Color.yellow });
			}

			{
				var binder = obj.GetComponent<Model.IMvpArrayBinder>();
				if (EditorPrefs.GetBool(PreferenceEditor.ShowArrayBinderIconInHierarchy, true) && binder != null)
					toDrawList.Add(new DrawIcon() { IconColor = Color.yellow });
			}
			{
				var binder = obj.GetComponent<Model.IMvpBoolBinder>();
				if (EditorPrefs.GetBool(PreferenceEditor.ShowBoolBinderIconInHierarchy, false) && binder != null)
					toDrawList.Add(new DrawIcon() { IconColor = Color.yellow });
			}
			{
				var binder = obj.GetComponent<Model.IMvpFloatBinder>();
				if (EditorPrefs.GetBool(PreferenceEditor.ShowFloatBinderIconInHierarchy, false) && binder != null)
					toDrawList.Add(new DrawIcon() { IconColor = Color.yellow });
			}
			{
				var binder = obj.GetComponent<Model.IMvpStringBinder>();
				if (EditorPrefs.GetBool(PreferenceEditor.ShowStringBinderIconInHierarchy, false) && binder != null)
					toDrawList.Add(new DrawIcon() { IconColor = Color.yellow });
			}
			{
				var binders = obj.GetComponents<Component>();
				if (EditorPrefs.GetBool(PreferenceEditor.ShowCustomBinderIconInHierarchy, false) 
					&& binders.Any(b => b.GetType().GetInterface(typeof(Model.IMvpCustomTypeBinder<>).FullName) != null))
					toDrawList.Add(new DrawIcon() { IconColor = Color.yellow });
			}

			var rect = selectionRect;
			rect.x = rect.x + rect.width - wdithToRight;
			rect.width = _iconWidth;
			rect.y += (rect.height - _iconHeight) / 2f;
			rect.height = _iconHeight;

			foreach(var toDraw in toDrawList)
			{
				rect.x -= _iconWidth;
				EditorGUI.DrawRect(rect, toDraw.IconColor);
				rect.x -= _space;
			}
		}
		
		struct DrawIcon
		{
			public Color IconColor;
		}
	}
}