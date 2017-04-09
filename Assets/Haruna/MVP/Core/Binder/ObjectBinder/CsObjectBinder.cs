using System;
using System.Collections.Generic;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	public class CsObjectBinder : MonoBehaviour, IMvpObjectBinder
	{
		[SerializeField]
		protected string _modelTypeString;

		[Serializable]
		public struct SerializedBinder
		{
			public string FieldName;
			public bool ManualInput;
			public MTokenType TokenType;
			public UnityEngine.Object BinderInstance;
		}
		[SerializeField]
		List<SerializedBinder> _binders;

		Type _modelType;
		//Dictionary<string, ModelPropertyInfo> _modelMemberToBinderInfo;

		void Awake()
		{
			_modelType = TypeUtil.GetTypeWithAssemblyTypeString(_modelTypeString);
			if (_modelType == null)
			{
				Debug.LogErrorFormat(this, "can not get model type {0}", _modelTypeString);
				return;
			}

			//if (_modelMemberToBinderInfo == null)
			//	_modelMemberToBinderInfo = BinderUtil.GetRequireBinderInfoFromModelMembers(_modelType);
		}

		public void SetData(MObject data)
		{
			if (_modelType != data.GetType())
			{
				Debug.LogErrorFormat(this,
					"data type is not current. current {0} . expected {1}",
					data.GetType().Name, _modelType.Name);
			}

			for (var i = 0; i < _binders.Count; i++)
			{
				var serializedBinder = _binders[i];
				//var modelMember = _modelMemberToBinderInfo[serializedBinder.FieldName];
				if(serializedBinder.BinderInstance == null)
				{
					Debug.LogWarningFormat(this, "binder for [{0}] is null", serializedBinder.FieldName);
					continue;
				}

				var value = data[serializedBinder.FieldName];
				//var memberInfo = _modelType.GetMember(serializedBinder.FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				//var valueType = TypeUtil.GetFieldOrPropertyType(memberInfo);

				BinderUtil.SetValueToBinder(value, serializedBinder.BinderInstance);
			}
		}

		public MObject GetData()
		{
			var obj = new MObject();
			for (var i = 0; i < _binders.Count; i++)
			{
				var serializedBinder = _binders[i];

				if (serializedBinder.BinderInstance == null)
				{
					Debug.LogWarningFormat(this, "binder for [{0}] is null", serializedBinder.FieldName);
					continue;
				}
				
				obj.Add(serializedBinder.FieldName, BinderUtil.GetValueFromBinder(serializedBinder.BinderInstance));

				//TypeUtil.SetValueFromFieldOrProperty(modelMember.MemberInfo, obj, value);
			}
			return obj;
		}
	}
}