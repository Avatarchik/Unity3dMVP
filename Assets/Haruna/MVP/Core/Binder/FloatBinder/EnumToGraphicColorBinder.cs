using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/Enum To Graphic Color Binder")]
	[RequireComponent(typeof(Graphic))]
	public class EnumToGraphicColorBinder : MonoBehaviour, IMvpFloatBinder
	{
		[SerializeField]
		bool _specifyEnumType;
		[SerializeField]
		string _enumTypeString;

		[System.Serializable]
		public class EnumToColorPair
		{
			public int Value;
			public Color ToSetColor;
		}

		[SerializeField]
		List<EnumToColorPair> _settings = new List<EnumToColorPair>() { new EnumToColorPair() { ToSetColor = Color.white } } ;

		float _value;

		public MFloat GetData()
		{
			return _value;
		}

		public void SetData(MFloat data)
		{
			_value = data.Value;

			var temp = _settings.Find(s => s.Value == _value);
			if(temp == null)
			{
				Debug.LogErrorFormat(this, "Receive value {0} does not exist in the settings", _value);
			}
			else
			{
				GetComponent<Graphic>().color = temp.ToSetColor;
			}
		}

		public bool HasEditorError()
		{
			if (_settings == null || _settings.Count == 0)
				return true;

			if (_settings.Any(s => s == null))
				return true;

			if (GetComponent<Graphic>() == null)
				return true;

			return false;
		}
	}
}