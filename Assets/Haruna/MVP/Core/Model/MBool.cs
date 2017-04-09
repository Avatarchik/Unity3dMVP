using System;

namespace Haruna.UnityMVP.Model
{
	public class MBool : MToken
	{
		bool _value;

		public bool Value { get { return _value; } }

		public MBool(bool value)
		{
			_value = value;
			_tokenType = MTokenType.Bool;
		}

		public override object ToObject(Type type)
		{
			return _value;
		}

		public static implicit operator bool(MBool b)
		{
			return b.Value;
		}
		public static implicit operator MBool(bool b)
		{
			return new MBool(b);
		}
	}
}
