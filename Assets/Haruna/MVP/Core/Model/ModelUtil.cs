using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haruna.UnityMVP.Model
{
	public static class ModelUtil
	{
		public static float ConvertNumberToFloat(object value)
		{
			var type = value.GetType();
			if (type == typeof(float))
				return (float)value;
			else if (type == typeof(int))
				return (float)(int)value;
			else if (type == typeof(long))
				return (float)(long)value;
			else if (type == typeof(double))
				return (float)(double)value;
			else if (type == typeof(short))
				return (float)(short)value;
			else if (type == typeof(byte))
				return (float)(byte)value;
			else if (type == typeof(char))
				return (float)(char)value;

			else if (type == typeof(uint))
				return (float)(uint)value;
			else if (type == typeof(ulong))
				return (float)(ulong)value;
			else if (type == typeof(ushort))
				return (float)(ushort)value;
			else if (type == typeof(sbyte))
				return (float)(sbyte)value;
			else if (type == typeof(decimal))
				return (float)(decimal)value;
			else
				throw new Exception("un support number type " + type.FullName);
		}
	}
}
