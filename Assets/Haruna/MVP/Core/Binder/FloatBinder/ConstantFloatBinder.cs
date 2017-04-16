using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/ConstantFloatBinder")]
	public class ConstantFloatBinder : MonoBehaviour, IMvpFloatBinder
	{

		[SerializeField]
		float _value;

		public MFloat GetData()
		{
			return _value;
		}

		public void SetData(MFloat data)
		{
			_value = data.Value;
		}
	}
}