using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/Bool/BoolToEventBinder")]
	public class BoolToEventBinder : MonoBehaviour, IMvpBoolBinder
	{
		public class BoolUnityEvent : UnityEvent<bool> { }

		[SerializeField]
		BoolUnityEvent _onValueChangeEvent;
		[SerializeField]
		UnityEvent _onValueTrueEvent;
		[SerializeField]
		UnityEvent _onValueFalseEvent;

		MBool _value;
		
		public void SetDataWithOutEvent(MBool data)
		{
			_value = data;
		}

		public void SetData(MBool data)
		{
			_value = data;
			if (_onValueChangeEvent != null)
				_onValueChangeEvent.Invoke(_value);
			if (_value && _onValueTrueEvent != null)
				_onValueTrueEvent.Invoke();
			if (!_value && _onValueFalseEvent != null)
				_onValueFalseEvent.Invoke();
		}

		public MBool GetData()
		{
			Debug.LogWarningFormat(this, "Get data from BoolToEventBinder.");
			return _value;
		}

		public bool HasEditorError()
		{
			if (BinderUtil.IsUnityEventHasError(_onValueChangeEvent))
				return true;
			if (BinderUtil.IsUnityEventHasError(_onValueTrueEvent))
				return true;
			if (BinderUtil.IsUnityEventHasError(_onValueFalseEvent))
				return true;

			return false;
		}
	}
}