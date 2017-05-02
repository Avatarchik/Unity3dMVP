using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/FloatConstantBinder")]
	public class FloatConstantBinder : MonoBehaviour, IMvpFloatBinder
	{
		[SerializeField]
		float _value;

		public MFloat GetData()
		{
			return _value;
		}

		public void SetData(MFloat data)
		{
			if (data == null)
				_value = 0;
			else
				_value = data;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}