using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Haruna.UnityMVP.Presenter
{
	public interface IOnPresenterBroadcast
	{
		void OnEvent(params MToken[] data);
	}

	[AddComponentMenu("UnityMVP/Linker/PresenterEventLinker")]
	public class PresenterEventLinker : MonoBehaviour, IOnPresenterBroadcast
	{
		[SerializeField]
		string _url;

		[SerializeField]
		List<Component> _eventParameterBinders;

		[SerializeField]
		UnityEvent _beforeReceiveData;
		[SerializeField]
		UnityEvent _afterReceiveData;

		public void OnEvent(params MToken[] data)
		{
			try
			{
				if (_beforeReceiveData != null)
					_beforeReceiveData.Invoke();

				if (data.Length != _eventParameterBinders.Count)
				{
					Debug.LogErrorFormat(this, "parameter count {0} is not equal with binders coutn {1}", _eventParameterBinders.Count);
					return;
				}

				for (var i = 0; i < _eventParameterBinders.Count; i++)
				{
					BinderUtil.SetValueToBinder(data[i], _eventParameterBinders[i]);
				}

				if (_afterReceiveData != null)
					_afterReceiveData.Invoke();
			}
			catch (Exception e)
			{
				Debug.LogErrorFormat(this, e.Message);
				Debug.LogException(e);
			}
		}


		public void RegistEvent()
		{
			PresenterDispatcher.GetInstance().RegistPresenterEvent(_url, this);
		}

		public bool HasEditorError()
		{
			var eventsMapping = PresenterUtil.GetAllPresetnerEvents();
			if (!eventsMapping.ContainsKey(_url))
				return true;

			if (_eventParameterBinders.Any(b => b == null))
				return true;

			return BinderUtil.IsUnityEventHasError(_beforeReceiveData)
				|| BinderUtil.IsUnityEventHasError(_afterReceiveData);
		}
	}
}