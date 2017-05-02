using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/StringConstantBinder")]
	public class StringConstantBinder : MonoBehaviour, IMvpStringBinder
	{
		[SerializeField]
		string _content;

		[SerializeField]
		bool _receiveSetData;

		public MString GetData()
		{
			return _content;
		}

		public void SetData(MString data)
		{
			if (data == null)
				_content = "";
			else
				_content = data;
			//do nothing;
		}
		public bool HasEditorError()
		{
			return false;
		}
	}
}