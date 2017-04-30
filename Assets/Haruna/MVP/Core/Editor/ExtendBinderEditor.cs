using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	[CustomEditor(typeof(ExtendBinder), true)]
	public class ExtendBinderEditor : Editor
	{
		List<EditorKit.SerializedType> _allBinderValueTypes;

		void OnEnable()
		{
			_allBinderValueTypes = BinderUtil.GetImplementedBinderValueTypes()
				.Select(t => new EditorKit.SerializedType(t)).ToList();
		}

		//int _selectedPageIndex;
		public override void OnInspectorGUI()
		{
			//_selectedPageIndex = GUILayout.Toolbar(_selectedPageIndex, new string[] { "Settings", "Events" }, EditorStyles.miniButton);
			//EditorGUILayout.Space();
			//Rect rect = EditorGUILayout.GetControlRect(false, 1f);
			//EditorGUI.DrawRect(rect, EditorStyles.label.normal.textColor * 0.5f);
			//EditorGUILayout.Space();

			//if (_selectedPageIndex == 1)
			//{
			//	EditorGUILayout.PropertyField(serializedObject.FindProperty("_beforeReceiveEvent"));
			//	EditorGUILayout.PropertyField(serializedObject.FindProperty("_afterReceiveEvent"));
			//}
			//else
			{
				var typeProperty = serializedObject.FindProperty("_dataType");
				EditorGUILayout.PropertyField(typeProperty);

				Type binderInterfaceType = null;
				var arrayElementType = (ArrayElementTypeEnum)typeProperty.enumValueIndex;

				switch (arrayElementType)
				{
					case ArrayElementTypeEnum.Bool:
						binderInterfaceType = typeof(IMvpBoolBinder);
						break;
					case ArrayElementTypeEnum.String:
						binderInterfaceType = typeof(IMvpStringBinder);
						break;
					case ArrayElementTypeEnum.Float:
						binderInterfaceType = typeof(IMvpFloatBinder);
						break;
					case ArrayElementTypeEnum.Object:
						binderInterfaceType = typeof(IMvpObjectBinder);
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
									binderInterfaceType = typeof(IMvpCustomTypeBinder<>).MakeGenericType(returnType.Value.Type);
								}
							}
						}
						break;
				}
				if (binderInterfaceType != null)
				{
					var mainBinderProperty = serializedObject.FindProperty("_mainBinder");
					var extendBindersProperty = serializedObject.FindProperty("_extendBinders");

					//EditorGUILayout.BeginHorizontal();
					//EditorGUILayout.PrefixLabel("Array Element Template");
					mainBinderProperty.objectReferenceValue = EditorKit.DrawBinderField("Main Binder", mainBinderProperty.objectReferenceValue, binderInterfaceType);

					if(EditorGUILayout.PropertyField(extendBindersProperty, false))
					{
						EditorGUI.indentLevel++;

						var size = EditorGUILayout.DelayedIntField("Size", extendBindersProperty.arraySize);
						while (extendBindersProperty.arraySize > size)
							extendBindersProperty.DeleteArrayElementAtIndex(size);

						for(var i = 0; i < size; i++)
						{
							if (i >= extendBindersProperty.arraySize)
								extendBindersProperty.InsertArrayElementAtIndex(i);

							var elementProp = extendBindersProperty.GetArrayElementAtIndex(i);
							elementProp.objectReferenceValue = EditorKit.DrawBinderField("Element " + i, elementProp.objectReferenceValue, binderInterfaceType);
						}

						EditorGUI.indentLevel--;
					}

					//EditorGUILayout.PropertyField(extendBindersProperty, true);
					//for(var i = 0; i < extendBindersProperty.arraySize; i++)
					//{
					//	var exbp = extendBindersProperty.GetArrayElementAtIndex(i);
						
					//}

					//EditorGUILayout.EndHorizontal();
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}