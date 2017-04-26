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
			_value = data.Value;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}