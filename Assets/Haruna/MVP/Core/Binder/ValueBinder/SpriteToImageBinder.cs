using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	public class SpriteToImageBinder : MonoBehaviour, IMvpCustomTypeBinder<Sprite>
	{
		[SerializeField]
		Image _targetImage;

		public MValue<Sprite> GetData()
		{
			return new MValue<Sprite>(_targetImage.sprite);
		}

		public void SetData(MValue<Sprite> data)
		{
			if (data == null)
				_targetImage.sprite = null;
			else
				_targetImage.sprite = data.Value;
		}
	}
}