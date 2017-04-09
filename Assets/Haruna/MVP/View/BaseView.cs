using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public class BaseView : MonoBehaviour
	{
		[SerializeField]
		bool _allowMultiple;

		public void Initialize()
		{

		}
	}
}