using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/BoolExtendBinder")]
	public class ExtendBinder : MonoBehaviour, IMvpTokenBinder
	{
		[SerializeField]
		ArrayElementTypeEnum _dataType;
		[SerializeField]
		string _customValueTypeString;

		[SerializeField]
		Component _mainBinder;

		[SerializeField]
		List<Component> _extendBinders;
		
		public void SetData(MToken data)
		{
			BinderUtil.SetValueToBinder(data, _mainBinder);
			for(var i = 0; i < _extendBinders.Count; i++)
			{
				BinderUtil.SetValueToBinder(data, _extendBinders[i]);
			}
		}

		public MToken GetData()
		{
			return BinderUtil.GetValueFromBinder(_mainBinder) as MBool;
		}

		public bool HasEditorError()
		{
			if (_mainBinder == null)
				return true;

			if (_extendBinders.Any(b => b == null))
				return true;

			return false;
		}
	}
}