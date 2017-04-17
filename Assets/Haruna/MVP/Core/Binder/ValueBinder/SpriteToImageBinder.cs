using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/SpriteToImageBinder")]
	[RequireComponent(typeof(Image))]
	public class SpriteToImageBinder : MonoBehaviour, IMvpCustomTypeBinder<Sprite>
	{

		public MValue<Sprite> GetData()
		{
			return new MValue<Sprite>(GetComponent<Image>().sprite);
		}

		public void SetData(MValue<Sprite> data)
		{
			if (data == null)
				GetComponent<Image>().sprite = null;
			else
				GetComponent<Image>().sprite = data.Value;
		}
	}
}