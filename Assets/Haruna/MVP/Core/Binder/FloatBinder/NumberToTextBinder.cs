using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	public class NumberToTextBinder : MonoBehaviour, IMvpFloatBinder
	{
		[SerializeField]
		Text _toBindText;

		//[SerializeField]
		//string _formatter;
		
		public MFloat GetData()
		{
			return float.Parse(_toBindText.text);
		}

		public void SetData(MFloat data)
		{
			_toBindText.text = data.Value.ToString();
		}
	}
}