using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/FloatToTextBinder")]
	[RequireComponent(typeof(Text))]
	public class FloatToTextBinder : MonoBehaviour, IMvpFloatBinder
	{
		public MFloat GetData()
		{
			return float.Parse(GetComponent<Text>().text);
		}

		public void SetData(MFloat data)
		{
			GetComponent<Text>().text = data.Value.ToString();
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}