using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/ModelArrayBinder")]
	public class ModelArrayBinder : MonoBehaviour, IMvpArrayBinder
	{
		[SerializeField]
		ArrayElementBinder _arrayElementTemplate;
		[SerializeField]
		UnityEvent _afterReceiveTemplateEvent;

		List<ArrayElementBinder> _elements = new List<ArrayElementBinder>();
		
		public void SetData(MArray data)
		{
			if (data == null)
				return;

			for(int i = 0; i < data.Count; i++)
			{
				ArrayElementBinder element;
				if (_elements.Count > i)
					element = _elements[i];
				else
				{
					element = Instantiate(_arrayElementTemplate);
					element.name = _arrayElementTemplate.name;
					element.gameObject.SetActive(true);
					element.transform.SetParent(transform, false);
					element.transform.localPosition = Vector3.zero;
					element.transform.localScale = Vector3.one;

					_elements.Add(element);
				}

				var value = data[i];
				element.SetData(value);
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
				ret.Add(e.GetData());
			});

			return ret;
		}

		public bool HasEditorError()
		{
			if (_arrayElementTemplate == null)
				return true;
			
			return false;
		}
	}
}