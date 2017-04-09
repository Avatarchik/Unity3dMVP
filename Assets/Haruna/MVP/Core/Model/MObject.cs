using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Haruna.UnityMVP.Model
{
	public class MObject : MToken, IDictionary<string, MToken>
	{
		Type _objType;
		Dictionary<string, MToken> _data = new Dictionary<string, MToken>();

		public MObject()
		{
			_tokenType = MTokenType.Object;
		}
		public MObject(object obj)
		{
			_objType = obj.GetType();
			_tokenType = MTokenType.Object;

			var members = obj.GetType().GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			for (var i = 0; i < members.Length; i++)
			{
				if (members[i].GetCustomAttributes(typeof(ModelPropertyAttribute), true).Length != 0)
				{
					var value = TypeUtil.GetValueFromFieldOrProperty(members[i], obj);
					_data.Add(members[i].Name, MToken.FromObject(value));
				}
			}
		}
		
		public override object ToObject(Type type)
		{
			if (_objType != null)
				type = _objType;

			var obj = Activator.CreateInstance(type);
			using(var itr = _data.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					var members = type.GetMember(itr.Current.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (members.Length == 0)
					{
						throw new Exception(string.Format("Can not get member {0} in type {1}", itr.Current.Key, type.FullName));
					}
					var fieldOrPropertyType = TypeUtil.GetFieldOrPropertyType(members[0]);
					TypeUtil.SetValueFromFieldOrProperty(members[0], obj, itr.Current.Value.ToObject(fieldOrPropertyType));
				}
			}
			return obj;
		}

		#region IDictionary
		public MToken this[string key]
		{
			get
			{
				return ((IDictionary<string, MToken>)_data)[key];
			}

			set
			{
				((IDictionary<string, MToken>)_data)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, MToken>)_data).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, MToken>)_data).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, MToken>)_data).Keys;
			}
		}

		public ICollection<MToken> Values
		{
			get
			{
				return ((IDictionary<string, MToken>)_data).Values;
			}
		}

		public void Add(KeyValuePair<string, MToken> item)
		{
			((IDictionary<string, MToken>)_data).Add(item);
		}

		public void Add(string key, MToken value)
		{
			((IDictionary<string, MToken>)_data).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, MToken>)_data).Clear();
		}

		public bool Contains(KeyValuePair<string, MToken> item)
		{
			return ((IDictionary<string, MToken>)_data).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, MToken>)_data).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, MToken>[] array, int arrayIndex)
		{
			((IDictionary<string, MToken>)_data).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, MToken>> GetEnumerator()
		{
			return ((IDictionary<string, MToken>)_data).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, MToken> item)
		{
			return ((IDictionary<string, MToken>)_data).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, MToken>)_data).Remove(key);
		}

		public bool TryGetValue(string key, out MToken value)
		{
			return ((IDictionary<string, MToken>)_data).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, MToken>)_data).GetEnumerator();
		}
		#endregion
	}
}
