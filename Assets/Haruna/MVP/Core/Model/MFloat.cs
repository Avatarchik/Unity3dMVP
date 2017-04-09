using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	public class MFloat : MToken
	{
		float _value;

		public float Value { get { return _value; } }

		public MFloat(float value)
		{
			_value = value;
			_tokenType = MTokenType.Float;
		}

		public override object ToObject(Type type)
		{
			return _value;
		}
		public static implicit operator float(MFloat v)
		{
			return v.Value;
		}
		public static implicit operator MFloat(float v)
		{
			return new MFloat(v);
		}
	}
}
