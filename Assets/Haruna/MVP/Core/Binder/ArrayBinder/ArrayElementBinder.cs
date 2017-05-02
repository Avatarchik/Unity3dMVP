using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Haruna.UnityMVP.Model
{
	public enum ArrayElementTypeEnum
	{
		String,
		Float,
		Bool,
		Object,
		Custom
	}
	[AddComponentMenu("UnityMVP/Binder/ArrayElementBinder")]
	public class ArrayElementBinder : MonoBehaviour, IMvpTokenBinder
	{
		[SerializeField]
		ArrayElementTypeEnum _elementType;
		[SerializeField]
		string _customElementTypeString;
		[SerializeField]
		Component _elementBinder;

		[SerializeField]
		UnityEvent _beforeReceiveEvent;

		[SerializeField]
		UnityEvent _afterReceiveEvent;
				
		public void SetData(MToken data)
		{
			_beforeReceiveEvent.Invoke();
			BinderUtil.SetValueToBinder(data, _elementBinder);
			_afterReceiveEvent.Invoke();
		}

		public MToken GetData()
		{
			return BinderUtil.GetValueFromBinder(_elementBinder);
		}

		public bool HasEditorError()
		{
			if (_elementBinder == null)
				return true;

			if (_elementType == ArrayElementTypeEnum.Custom &&
					TypeUtil.GetTypeWithAssemblyTypeString(_customElementTypeString) == null)
				return true;

			if (BinderUtil.IsUnityEventHasError(_afterReceiveEvent))
				return true;

			return false;
		}
	}
}