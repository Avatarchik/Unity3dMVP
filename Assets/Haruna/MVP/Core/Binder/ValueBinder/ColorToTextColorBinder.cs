using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/ColorToTextColorBinder")]
	[RequireComponent(typeof(Text))]
	public class ColorToTextColorBinder : MonoBehaviour, IMvpCustomTypeBinder<Color>
	{
		public MValue<Color> GetData()
		{
			return new MValue<Color>(GetComponent<Text>().color);
		}

		public void SetData(MValue<Color> data)
		{
			if (data == null)
				GetComponent<Text>().color = Color.white;
			else
				GetComponent<Text>().color = data.Value;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}