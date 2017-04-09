using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	public class MValue<T> : MToken
	{
		T _obj;
		public Type ValueType { get { return typeof(T); } }

		public MValue(T obj)
		{
			_obj = obj;
			_tokenType = MTokenType.Custom;
		}
		
		public T Value { get { return _obj; } }

		public override object ToObject(Type type)
		{
			return _obj;
		}
	}
}
