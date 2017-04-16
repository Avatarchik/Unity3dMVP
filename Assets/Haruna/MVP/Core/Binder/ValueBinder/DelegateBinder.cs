using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/DelegateBinder")]
	public class DelegateBinder : MonoBehaviour, IMvpCustomTypeBinder<Action>
	{
		[SerializeField]
		Action _action;
		
		public MValue<Action> GetData()
		{
			return new MValue<Action>(_action);
		}

		public void SetData(MValue<Action> data)
		{
			if (data == null)
				_action = null;
			else
				_action = data.Value;
		}

		public void CallDelegate()
		{
			if (_action != null)
			{
				_action();
			}
		}
	}
}