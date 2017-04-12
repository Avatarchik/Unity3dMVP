using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	public class ConstatStringBinder : MonoBehaviour, IMvpStringBinder
	{
		[SerializeField]
		string _content;

		[SerializeField]
		bool _receiveSetData;

		public MString GetData()
		{
			return _content;
		}

		public void SetData(MString data)
		{
			if (_receiveSetData)
			{
				if (data == null)
					_content = "";
				else
					_content = data;
			}
			//do nothing;
		}
	}
}