using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
		
		[SerializeField]
		UnityEvent _beforeReceiveData;
		[SerializeField]
		UnityEvent _afterReceiveData;

		public void SendRequest()
		{
			var toSendData = _toSendDataBinders.Select(b => BinderUtil.GetValueFromBinder(b));
			if (!_async)
			{
				var res = PresenterDispatcher.GetInstance().RequestWithMTokens(_url, toSendData.ToArray());
				OnResponse(res);
			}
			else
			{
				PresenterDispatcher.GetInstance().RequestWithMTokensAsync(this, OnResponse, _url, toSendData.ToArray());
			}
		}

		void OnResponse(IPresenterResponse res)
		{
			_beforeReceiveData.Invoke();
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

			_afterReceiveData.Invoke();
		}

		public bool HasEditorError()
		{
			var actionMapping = PresenterUtil.GetAllPresenterAction();
			if (!actionMapping.ContainsKey(_url))
				return true;

			if (_toSendDataBinders.Any(b => b == null)
				|| _responseDataBinder.Any(b => b == null))
				return true;
			
			return BinderUtil.IsUnityEventHasError(_beforeReceiveData)
				|| BinderUtil.IsUnityEventHasError(_afterReceiveData);
		}
	}
}