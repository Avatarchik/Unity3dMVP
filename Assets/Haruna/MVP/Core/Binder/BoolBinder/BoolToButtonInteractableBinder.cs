using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/BoolToButtonBinder")]
	[RequireComponent(typeof(Button))]
	public class BoolToButtonInteractableBinder : MonoBehaviour, IMvpBoolBinder
	{

		public void SetData(MBool data)
		{
			GetComponent<Button>().interactable = data.Value;
		}

		public MBool GetData()
		{
			return GetComponent<Button>().interactable;
		}

		public bool HasEditorError()
		{
			return false;
		}
	}
}