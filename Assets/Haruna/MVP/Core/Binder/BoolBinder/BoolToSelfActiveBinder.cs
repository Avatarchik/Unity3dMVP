using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/BoolToSelfActiveBinder")]
	public class BoolToSelfActiveBinder : MonoBehaviour, IMvpBoolBinder
	{
		[SerializeField]
		bool _isReverse;
		MBool _value;

		public void SetData(MBool data)
		{
			_value = data;
			if(_isReverse)
			{
				gameObject.SetActive(!_value.Value);
			}else
			{
				gameObject.SetActive(_value.Value);
			}
		}

		public MBool GetData()
		{
			return gameObject.activeSelf;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}