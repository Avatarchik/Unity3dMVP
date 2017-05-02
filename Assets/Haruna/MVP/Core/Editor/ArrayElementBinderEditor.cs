using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	[CustomEditor(typeof(ArrayElementBinder), true)]
	public class ArrayElementBinderEditor : Editor
	{
		List<EditorKit.SerializedType> _allBinderValueTypes;

		void OnEnable()
		{
			_allBinderValueTypes = BinderUtil.GetImplementedBinderValueTypes()
				.Select(t => new EditorKit.SerializedType(t)).ToList();
		}

		int _selectedPageIndex;
		public override void OnInspectorGUI()
		{
			_selectedPageIndex = GUILayout.Toolbar(_selectedPageIndex, new string[] { "Settings", "Events" }, EditorStyles.miniButton);
			EditorGUILayout.Space();
			Rect rect = EditorGUILayout.GetControlRect(false, 1f);
			EditorGUI.DrawRect(rect, EditorStyles.label.normal.textColor * 0.5f);
			EditorGUILayout.Space();

			if (_selectedPageIndex == 1)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_beforeReceiveEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_afterReceiveEvent"));
			}
			else
			{
				var typeProperty = serializedObject.FindProperty("_elementType");
				EditorGUILayout.PropertyField(typeProperty);

				var templateProperty = serializedObject.FindProperty("_elementBinder");

				Type templateInterfaceType = null;
				var arrayElementType = (ArrayElementTypeEnum)typeProperty.enumValueIndex;

				switch (arrayElementType)
				{
					case ArrayElementTypeEnum.Bool:
						templateInterfaceType = typeof(IMvpBoolBinder);
						break;
					case ArrayElementTypeEnum.String:
						templateInterfaceType = typeof(IMvpStringBinder);
						break;
					case ArrayElementTypeEnum.Float:
						templateInterfaceType = typeof(IMvpFloatBinder);
						break;
					case ArrayElementTypeEnum.Object:
						templateInterfaceType = typeof(IMvpObjectBinder);
						break;
					case ArrayElementTypeEnum.Custom:
						{
							var customElementTypeStringProp = serializedObject.FindProperty("_customElementTypeString");
							if (_allBinderValueTypes.Count == 0)
							{
								EditorGUILayout.HelpBox("No custom type binder implemented.", MessageType.Warning);
							}
							else
							{
								var returnType = EditorKit.DrawTypeSelector("Custom Element Type", customElementTypeStringProp.stringValue, _allBinderValueTypes);
								if (returnType != null)
								{
									customElementTypeStringProp.stringValue = returnType.Value.TypeString;
									templateInterfaceType = typeof(IMvpCustomTypeBinder<>).MakeGenericType(returnType.Value.Type);
								}
							}
						}
						break;
				}
				if (templateInterfaceType != null)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Element Binder");
					templateProperty.objectReferenceValue = EditorKit.DrawBinderField("", templateProperty.objectReferenceValue, templateInterfaceType);
					EditorGUILayout.EndHorizontal();
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}