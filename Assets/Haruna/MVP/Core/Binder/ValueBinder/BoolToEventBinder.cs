using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
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
	}
}