using System;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/FloatToSliderBinder")]
	[RequireComponent(typeof(Slider))]
	public class FloatToSliderBinder : MonoBehaviour, IMvpFloatBinder
	{
		
		public MFloat GetData()
		{
			return GetComponent<Slider>().value;
		}

		public void SetData(MFloat data)
		{
            GetComponent<Slider>().value = data.Value;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}