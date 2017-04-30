using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[CustomEditor(typeof(EnumToGraphicColorBinder), true)]
	public class EnumToGraphicColorBinderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var specifyEnumTypeProp = serializedObject.FindProperty("_specifyEnumType");
			EditorGUILayout.PropertyField(specifyEnumTypeProp);

			Type enumType = null;
			if (specifyEnumTypeProp.boolValue)
			{
				var enumTypeStringProperty = serializedObject.FindProperty("_enumTypeString");
				var allEnumTypes = TypeUtil.GetAllTypes(t => t.IsEnum && t.GetCustomAttributes(typeof(MvpModelAttribute), true).Length != 0)
					.Select(t => new EditorKit.SerializedType(t))
					.OrderBy(t => t.TypeString)
					.ToList();

				if(allEnumTypes.Count == 0)
				{
					EditorGUILayout.HelpBox("No enum implement MvpModelAttribute", MessageType.Info);
				}
				else
				{
					var type = EditorKit.DrawTypeSelector("Select Enum Type", enumTypeStringProperty.stringValue, allEnumTypes);
					if (type != null)
					{
						enumTypeStringProperty.stringValue = type.Value.TypeString;
						enumType = type.Value.Type;
					}
				}
			}


			var graphic = ((Component)target).GetComponent<Graphic>();
			var settingsListProperty = serializedObject.FindProperty("_settings");
			
			if (EditorGUILayout.PropertyField(settingsListProperty, false))
			{
				EditorGUI.indentLevel++;

				var size = EditorGUILayout.DelayedIntField("Size", settingsListProperty.arraySize);
				while (settingsListProperty.arraySize > size)
					settingsListProperty.DeleteArrayElementAtIndex(size);

				for (var i = 0; i < size; i++)
				{
					if (i >= settingsListProperty.arraySize)
						settingsListProperty.InsertArrayElementAtIndex(i);

					var elementProp = settingsListProperty.GetArrayElementAtIndex(i);

					var valueProp = elementProp.FindPropertyRelative("Value");
					var colorProp = elementProp.FindPropertyRelative("ToSetColor");
					
					EditorGUILayout.BeginHorizontal();

					if (enumType == null)
					{
						valueProp.intValue = EditorGUILayout.IntField("Element " + i, valueProp.intValue);
					}
					else
					{
						var enumNames = Enum.GetNames(enumType);
						var enumValues = (int[])Enum.GetValues(enumType);
						var index = enumValues.ToList().FindIndex(t => t == valueProp.intValue);
						index = EditorGUILayout.Popup("Element " + i, index, enumNames);
						valueProp.intValue = enumValues[index];
					}

					var oldColor = colorProp.colorValue;
					colorProp.colorValue = EditorGUILayout.ColorField(colorProp.colorValue);
					if(colorProp.colorValue != oldColor && graphic != null)
						graphic.color = colorProp.colorValue;


					if (graphic != null && GUILayout.Button("Set"))
					{
						graphic.color = colorProp.colorValue;
						graphic.enabled = !graphic.enabled;
						graphic.enabled = !graphic.enabled;
					}

					EditorGUILayout.EndHorizontal();
				}

				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}