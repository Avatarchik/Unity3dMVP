using System;
using System.Collections.Generic;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	public class ModelObjectBinder : MonoBehaviour, IMvpObjectBinder
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
		
		public void SetData(MObject data)
		{
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
			}
			return obj;
		}
	}
}