using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/NumberToTextBinder")]
	[RequireComponent(typeof(Text))]
	public class NumberToTextBinder : MonoBehaviour, IMvpFloatBinder
	{
		
		public MFloat GetData()
		{
			return float.Parse(GetComponent<Text>().text);
		}

		public void SetData(MFloat data)
		{
			GetComponent<Text>().text = data.Value.ToString();
		}
	}
}