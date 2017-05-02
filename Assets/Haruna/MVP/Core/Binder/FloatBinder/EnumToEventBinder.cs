using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/Enum To Event Binder")]
	public class EnumToEventBinder : MonoBehaviour, IMvpFloatBinder
	{
		[SerializeField]
		bool _specifyEnumType;
		[SerializeField]
		string _enumTypeString;

		[System.Serializable]
		public class EnumToColorPair
		{
			public int Value;
			public UnityEvent Event;
		}

		[SerializeField]
		List<EnumToColorPair> _eventSettings;

		float _value;

		public MFloat GetData()
		{
			return _value;
		}

		public void SetData(MFloat data)
		{
			_value = data.Value;

			var temp = _eventSettings.Find(s => s.Value == _value);
			if(temp == null)
			{
				Debug.LogErrorFormat(this, "Receive value {0} does not exist in the settings", _value);
			}
			else
			{
				temp.Event.Invoke();
			}
		}

		public bool HasEditorError()
		{
			if (_eventSettings == null || _eventSettings.Count == 0)
				return true;

			if (_eventSettings.Any(s => s == null || BinderUtil.IsUnityEventHasError(s.Event)))
				return true;

			if (GetComponent<Graphic>() == null)
				return true;

			return false;
		}
	}
}