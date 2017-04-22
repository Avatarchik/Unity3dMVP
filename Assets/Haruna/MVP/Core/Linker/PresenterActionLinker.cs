using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[AddComponentMenu("UnityMVP/Linker/PresenterActionLinker")]
	public class PresenterActionLinker : MonoBehaviour
	{
		[SerializeField]
		bool _async;
		[SerializeField]
		string _url;

		[SerializeField]
		List<Component> _toSendDataBinders;

		[SerializeField]
		List<Component> _responseDataBinder;

		public void SendRequest()
		{
			var toSendData = _toSendDataBinders.Select(b => BinderUtil.GetValueFromBinder(b));
			if (!_async)
			{
				var res = PresenterDispatcher.GetInstance().RequestWithMTokens(_url, toSendData.ToArray());
				OnAsyncResponse(res);
			}
			else
			{
				PresenterDispatcher.GetInstance().RequestWithMTokensAsync(this, OnAsyncResponse, _url, toSendData.ToArray());
			}
		}

		void OnAsyncResponse(IPresenterResponse res)
		{
			if (res.StatusCode == 200)
			{
				if (res.Data != null)
				{
					for(var i = 0; i < _responseDataBinder.Count; i++)
					{
						BinderUtil.SetValueToBinder(res.Data[i], _responseDataBinder[i]);
					}
				}
			}
			else
			{
				Debug.LogWarningFormat(this, "Local request {0} returned with error. code {1} : {2}", _url, res.StatusCode, res.ErrorMessage);
			}
		}
	}
}