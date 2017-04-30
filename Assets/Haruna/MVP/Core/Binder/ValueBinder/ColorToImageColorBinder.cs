using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/ColorToImageColorBinder")]
	[RequireComponent(typeof(Image))]
	public class ColorToImageColorBinder : MonoBehaviour, IMvpCustomTypeBinder<Color>
	{
		public MValue<Color> GetData()
		{
			return new MValue<Color>(GetComponent<Image>().color);
		}

		public void SetData(MValue<Color> data)
		{
			if (data == null)
				GetComponent<Image>().color = Color.white;
			else
				GetComponent<Image>().color = data.Value;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}