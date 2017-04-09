using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public interface IOnPresenterBroadcast
	{
		void OnEvent(params MToken[] data);
	}

	public class PresenterEventLinker : MonoBehaviour, IOnPresenterBroadcast
	{
		[SerializeField]
		string _url;
		
		[SerializeField]
		List<Component> _eventParameterBinders;
		
		public void OnEvent(params MToken[] data)
		{
			if(data.Length != _eventParameterBinders.Count)
			{
				Debug.LogErrorFormat(this, "parameter count {0} is not equal with binders coutn {1}", _eventParameterBinders.Count);
				return;
			}

			for(var i = 0; i < _eventParameterBinders.Count; i++)
			{
				BinderUtil.SetValueToBinder(data[i], _eventParameterBinders[i]);
			}
		}

		public void RegistEvent()
		{
			PresenterDispatcher.GetInstance().RegistPresenterEvent(_url, this);
		}
	}
}