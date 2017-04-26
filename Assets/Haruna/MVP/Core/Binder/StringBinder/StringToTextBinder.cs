using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/StringToTextBinder")]
	[RequireComponent(typeof(Text))]
	public class StringToTextBinder : MonoBehaviour, IMvpStringBinder
	{
		public MString GetData()
		{
			return GetComponent<Text>().text;
		}

		public void SetData(MString data)
		{
			if (data == null)
				GetComponent<Text>().text = "";
			else
				GetComponent<Text>().text = data;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}