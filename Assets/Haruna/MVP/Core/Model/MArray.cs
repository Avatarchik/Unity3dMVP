using System;
using System.Collections;
using System.Collections.Generic;

namespace Haruna.UnityMVP.Model
{
	public class MArray : MToken, IList<MToken>
	{
		List<MToken> _tokenList = new List<MToken>();

		public MArray()
		{
			_tokenType = MTokenType.Array;
		}
		public MArray(IList list)
		{
			_tokenType = MTokenType.Array;

			for(var i = 0; i < list.Count; i++)
			{
				_tokenList.Add(MToken.FromObject(list[i]));
			}
		}

		public override object ToObject(Type type)
		{
			var @interface = type.GetInterface(typeof(IList<>).FullName);
			if (@interface != null)
			{
				var obj = Activator.CreateInstance(type) as IList;
				for (var i = 0; i < _tokenList.Count; i++)
				{
					obj.Add(_tokenList[i].ToObject(@interface.GetGenericArguments()[0]));
				}
			}

			throw new Exception(string.Format("can not convert MArray to type {0} since it does not implement IList<>.", type.FullName));
		}

		#region IList
		public MToken this[int index]
		{
			get
			{
				return ((IList<MToken>)_tokenList)[index];
			}

			set
			{
				((IList<MToken>)_tokenList)[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IList<MToken>)_tokenList).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<MToken>)_tokenList).IsReadOnly;
			}
		}

		public void Add(MToken item)
		{
			((IList<MToken>)_tokenList).Add(item);
		}

		public void Clear()
		{
			((IList<MToken>)_tokenList).Clear();
		}

		public bool Contains(MToken item)
		{
			return ((IList<MToken>)_tokenList).Contains(item);
		}

		public void CopyTo(MToken[] array, int arrayIndex)
		{
			((IList<MToken>)_tokenList).CopyTo(array, arrayIndex);
		}

		public IEnumerator<MToken> GetEnumerator()
		{
			return ((IList<MToken>)_tokenList).GetEnumerator();
		}

		public int IndexOf(MToken item)
		{
			return ((IList<MToken>)_tokenList).IndexOf(item);
		}

		public void Insert(int index, MToken item)
		{
			((IList<MToken>)_tokenList).Insert(index, item);
		}

		public bool Remove(MToken item)
		{
			return ((IList<MToken>)_tokenList).Remove(item);
		}

		public void RemoveAt(int index)
		{
			((IList<MToken>)_tokenList).RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<MToken>)_tokenList).GetEnumerator();
		}
		#endregion
	}
}
