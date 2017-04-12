using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	public class StringToTextBinder : MonoBehaviour, IMvpStringBinder
	{
		[SerializeField]
		Text _toBindText;

		public MString GetData()
		{
			return _toBindText.text;
		}

		public void SetData(MString data)
		{
			if (data == null)
				_toBindText.text = "";
			else
				_toBindText.text = data;
		}
	}
}