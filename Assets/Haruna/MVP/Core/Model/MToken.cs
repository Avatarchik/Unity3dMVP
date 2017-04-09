using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	public enum MTokenType
	{
		Dynamic,
		String,
		Float,
		Bool,
		Object,
		Array,
		Custom,
	}

	public abstract class MToken
	{
		//protected Type _reflectiontype;
		//public Type ReflectionType { get { return _reflectiontype; } }

		protected MTokenType _tokenType;
		public virtual MTokenType TokenType { get { return _tokenType; } }
		
		public static MToken FromObject(object obj)
		{
			var type = obj.GetType();

			if (type.IsPrimitive)
			{
				if (type == typeof(bool))
					return new MBool((bool)obj);
				else
				{
					return new MFloat(ModelUtil.ConvertNumberToFloat(obj));
				}
			}
			else if(type == typeof(string))
			{
				return new MString((string)obj);
			}
			else if(type.GetCustomAttributes(typeof(MvpModelAttribute), true).Length != 0)
			{
				return new MObject(obj);
			}
			else if(obj is IList && type.GetInterface(typeof(IList<>).FullName, false) != null)
			{
				return new MArray((IList)obj);
			}
			else
			{
				var mvalueType = typeof(MValue<>).MakeGenericType(type);
				var value = Activator.CreateInstance(mvalueType, obj);
				return (MToken)value;
			}
		}
		
		public abstract object ToObject(Type type);
	}
}
