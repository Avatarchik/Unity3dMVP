using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	[CustomEditor(typeof(ModelArrayBinder), true)]
	public class ModelArrayBinderEditor : Editor
	{
		List<EditorKit.SerializedType> _allBinderValueTypes;

		void OnEnable()
		{
			_allBinderValueTypes = BinderUtil.GetImplementedBinderValueTypes()
				.Select(t => new EditorKit.SerializedType(t)).ToList();
		}

		public override void OnInspectorGUI()
		{
			var typeProperty = serializedObject.FindProperty("_arrayElementType");
			EditorGUILayout.PropertyField(typeProperty);

			var templateProperty = serializedObject.FindProperty("_arrayElementTemplate");

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
							customElementTypeStringProp.stringValue = returnType.TypeString;
							templateInterfaceType = typeof(IMvpCustomTypeBinder<>).MakeGenericType(returnType.Type);
						}
					}
					break;
			}
			if (templateInterfaceType != null)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Array Element Template");
				templateProperty.objectReferenceValue = EditorKit.DrawBinderField("", templateProperty.objectReferenceValue, templateInterfaceType);
				EditorGUILayout.EndHorizontal();
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}