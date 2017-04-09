using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public class PresenterActionLinker : MonoBehaviour
	{
		[SerializeField]
		string _url;

		[SerializeField]
		List<Component> _toSendDataBinders;

		[SerializeField]
		Component _responseDataBinder;

		public void SendRequest()
		{
			var toSendData = _toSendDataBinders.Select(b => BinderUtil.GetValueFromBinder(b));
			var res = PresenterDispatcher.GetInstance().RequestWithMTokens(_url, toSendData.ToArray());
			if(res.StatusCode == 200)
			{
				if(res.Data != null)
				{
					BinderUtil.SetValueToBinder(res.Data, _responseDataBinder);
				}
			}
			else
			{
				Debug.LogWarningFormat(this, "Local request {0} returned with error. code {1} : {2}", _url, res.StatusCode, res.ErrorMessage);
			}
		}
	}
}