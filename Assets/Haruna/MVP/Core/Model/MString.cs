using System;

namespace Haruna.UnityMVP.Model
{
	public class MString : MToken
	{
		string _value;
		public string Value { get { return _value; } }

		public MString(string value)
		{
			_value = value;
			_tokenType = MTokenType.String;
		}

		public override object ToObject(Type type)
		{
			return _value;
		}
		public static implicit operator string(MString v)
		{
			return v.Value;
		}
		public static implicit operator MString(string v)
		{
			return new MString(v);
		}
	}
}
