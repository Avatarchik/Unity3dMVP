using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace Haruna.UnityMVP.Model
{
	[CustomEditor(typeof(ModelObjectBinder), true)]
	public class ModelObjectBinderEditor : Editor
	{
		SerializedProperty _modelTypeProperty;
		SerializedProperty _bindersProperty;

		List<MvpModelTypeInfo> _avaliableModels;
		List<EditorKit.SerializedType> _avaliableModelTypes;

		public struct MvpModelTypeInfo
		{
			public EditorKit.SerializedType ReflectionType;
			public string DisplayName;
			public MvpModelTypeInfo(Type type)
			{
				ReflectionType = new EditorKit.SerializedType(type);
				var attr = ((MvpModelAttribute)type.GetCustomAttributes(typeof(MvpModelAttribute), true)[0]);
				DisplayName = string.IsNullOrEmpty(attr.DisplayName) ? ReflectionType.TypeString.Split(';')[0] : attr.DisplayName;
			}
		}

		void OnEnable()
		{
			_modelTypeProperty = serializedObject.FindProperty("_modelTypeString");
			_bindersProperty = serializedObject.FindProperty("_binders");

			_avaliableModels = TypeUtil.GetAllTypes((t) =>
			{
				return t.GetCustomAttributes(typeof(MvpModelAttribute), true).Length != 0;
			}).Select(t => new MvpModelTypeInfo(t)).OrderBy(s => s.DisplayName).ToList();
			
			_avaliableModelTypes = _avaliableModels.Select(t => t.ReflectionType).ToList();
		}

		int _tempIndex;
		public override void OnInspectorGUI()
		{
			if (_avaliableModels == null || _avaliableModels.Count == 0)
			{
				EditorGUILayout.HelpBox("no model!", MessageType.Warning);
				return;
			}

			var temp = EditorKit.DrawTypeSelector("Model Type", _modelTypeProperty.stringValue, _avaliableModelTypes);
			if(temp != null)
			{
				_modelTypeProperty.stringValue = temp.Value.TypeString;
				DrawSerializeFields(temp.Value.Type);
			}

			//int index = _avaliableModels.FindIndex(d => d.TypeString == _modelTypeProperty.stringValue);

			//if (index < 0 && !string.IsNullOrEmpty(_modelTypeProperty.stringValue))
			//{
			//	var displayTypeString = _modelTypeProperty.stringValue;
			//	var temp = displayTypeString.Split(';');
			//	if (temp.Length == 2)
			//	{
			//		displayTypeString = temp[0] + "\n" + temp[1];
			//	}
			//	EditorGUILayout.HelpBox("Serialized Value Error. Model Type is not exist\n" + displayTypeString, MessageType.Error, true);
			//	if (GUILayout.Button("Reset Serialized Value"))
			//	{
			//		_modelTypeProperty.stringValue = "";
			//	}

			//	EditorGUILayout.Space();
			//	EditorGUILayout.BeginHorizontal();
			//	_tempIndex = EditorGUILayout.Popup(_tempIndex, _avaliableModelTypes);
			//	if (GUILayout.Button("Set As New"))
			//	{
			//		_modelTypeProperty.stringValue = _avaliableModels[_tempIndex].TypeString;
			//	}
			//	EditorGUILayout.EndHorizontal();
			//}
			//else
			//{
			//	if (index < 0) index = 0;

			//	index = EditorGUILayout.Popup("Model Type", index, _avaliableModelTypes);

			//	_modelTypeProperty.stringValue = _avaliableModels[index].TypeString;
			//	DrawSerializeFields(_avaliableModels[index].ReflectionType);
			//}

			serializedObject.ApplyModifiedProperties();
		}

		void DrawSerializeFields(Type modelType)
		{
			var beforeSerializedValues = new Dictionary<string, ModelObjectBinder.SerializedBinder>();

			for (var i = 0; i < _bindersProperty.arraySize; i++)
			{
				var element = _bindersProperty.GetArrayElementAtIndex(i);
				var value = new ModelObjectBinder.SerializedBinder()
				{
					FieldName = element.FindPropertyRelative("FieldName").stringValue,
					ManualInput = element.FindPropertyRelative("ManualInput").boolValue,
					TokenType = (MTokenType)element.FindPropertyRelative("TokenType").intValue,
					BinderInstance = element.FindPropertyRelative("BinderInstance").objectReferenceValue
				};
				if (!string.IsNullOrEmpty(value.FieldName))
					beforeSerializedValues.Add(value.FieldName, value);
			}

			var afterSerializedValues = new List<ModelObjectBinder.SerializedBinder>();
			//var afterSerializedValues = new Dictionary<string, UnityEngine.Object>();
			var toSerializeMembers = BinderUtil.GetRequireBinderInfoFromModelMembers(modelType);
			foreach (var member in toSerializeMembers)
			{
				var fieldName = member.Key;
				ModelObjectBinder.SerializedBinder binderInfo;
				if (!beforeSerializedValues.TryGetValue(fieldName, out binderInfo))
				{
					binderInfo = new ModelObjectBinder.SerializedBinder()
					{
						FieldName = fieldName,
						ManualInput = false,
						TokenType = member.Value.BinderInfo.TokenType
					};
				}

				if (member.Value.BinderInfo.TokenType == MTokenType.Dynamic)
				{
					EditorGUILayout.LabelField(fieldName, member.Value.BinderInfo.ValueTypeName + "is not supported. Use float instead.");
				}
				else
				{
					binderInfo.BinderInstance = EditorKit.DrawBinderField(
						fieldName, member.Value.BinderInfo.ValueTypeName, binderInfo.BinderInstance, member.Value.BinderInfo.InterfaceType);
				}
				afterSerializedValues.Add(binderInfo);
			}

			for (var i = beforeSerializedValues.Count - 1; i >= afterSerializedValues.Count; i--)
			{
				_bindersProperty.DeleteArrayElementAtIndex(i);
			}

			for (var i = 0; i < afterSerializedValues.Count; i++)
			{
				if (i >= _bindersProperty.arraySize)
				{
					_bindersProperty.InsertArrayElementAtIndex(i);
				}
				var data = afterSerializedValues[i];
				var element = _bindersProperty.GetArrayElementAtIndex(i);
				element.FindPropertyRelative("FieldName").stringValue = data.FieldName;
				element.FindPropertyRelative("ManualInput").boolValue = data.ManualInput;
				element.FindPropertyRelative("TokenType").intValue = (int)data.TokenType;
				element.FindPropertyRelative("BinderInstance").objectReferenceValue = data.BinderInstance;
			}

		}
	}
}