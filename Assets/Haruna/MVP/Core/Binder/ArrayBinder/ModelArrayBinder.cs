using System;
using System.Collections.Generic;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	public enum ArrayElementTypeEnum
	{
		String,
		Float,
		Bool,
		Object,
		Custom
	}
	[AddComponentMenu("UnityMVP/Binder/ArrayBinder")]
	public class ModelArrayBinder : MonoBehaviour, IMvpArrayBinder
	{
		[SerializeField]
		ArrayElementTypeEnum _arrayElementType;
		[SerializeField]
		string _customElementTypeString;
		[SerializeField]
		Component _arrayElementTemplate;

		List<Component> _elements = new List<Component>();
		
		public void SetData(MArray data)
		{
			for(int i = 0; i < data.Count; i++)
			{
				Component element;
				if (_elements.Count > i)
					element = _elements[i];
				else
				{
					element = Instantiate(_arrayElementTemplate);
					element.transform.SetParent(transform, false);
					element.transform.localPosition = Vector3.zero;
					element.transform.localScale = Vector3.one;

					_elements.Add(element);
				}

				var value = data[i];
				BinderUtil.SetValueToBinder(value, element);
			}
			for(int i = _elements.Count - 1; i >= data.Count; i++)
			{
				Destroy(_elements[i].gameObject);
				_elements.RemoveAt(i);
			}
		}

		public MArray GetData()
		{
			MArray ret = new MArray();
			_elements.ForEach(e =>
			{
				ret.Add(BinderUtil.GetValueFromBinder(e));
			});

			return ret;
		}
	}
}