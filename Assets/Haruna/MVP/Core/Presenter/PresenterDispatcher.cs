using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public class PresenterDispatcher : MonoBehaviour
	{
		static PresenterDispatcher _instance;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static PresenterDispatcher GetInstance()
		{
			if (_instance == null)
			{
				_instance = new GameObject("PresenterDispatcher").AddComponent<PresenterDispatcher>();
				_instance.InitActionMapping();
				DontDestroyOnLoad(_instance);
			}
			return _instance;
		}
		
		Dictionary<string, PresenterActionInfo> _actionMapping;
		Dictionary<string, List<IOnPresenterBroadcast>> _registedEvents = new Dictionary<string, List<IOnPresenterBroadcast>>();

		void InitActionMapping()
		{
			_actionMapping = PresenterUtil.GetAllPresenterAction();

			var eventsInfo = PresenterUtil.GetAllPresetnerEvents();
			foreach(var info in eventsInfo)
			{
				var v = info.Value.Field.GetValue(null);
				if(v == null)
				{
					v = Activator.CreateInstance(info.Value.Field.FieldType);
					info.Value.Field.SetValue(null, v);
				}
				((PresenterEvent)v).Url = info.Value.Url;
			}
		}
		
		public IPresenterResponse RequestWithMTokens(string url, params MToken[] args)
		{
			PresenterActionInfo action;
			if(_actionMapping.TryGetValue(url, out action))
			{
				var parameters = action.Method.GetParameters();
				if(parameters.Length != args.Length)
				{
					return new PresenterResponse()
					{
						StatusCode = 500,
						ErrorMessage = string.Format("request parameter count {0} is not equal with action method parameter count {1}",
						parameters.Length, args.Length)
					};
				}

				try
				{
					var pList = new List<object>();
					for (var i = 0; i < args.Length; i++)
					{
						var parameterType = parameters[i].ParameterType;
						if(parameterType != typeof(MToken) && !parameterType.IsSubclassOf(typeof(MToken)))
						{
							var temp = args[i].ToObject(parameterType);
							pList.Add(temp);
						}
						else
						{
							pList.Add(args[i]);
						}
					}
					var returnValue = action.Method.Invoke(null, pList.ToArray());
					if(returnValue is IPresenterResponse)
					{
						return (IPresenterResponse)returnValue;
					}

					var data = returnValue is MToken ? (MToken)returnValue : MToken.FromObject(returnValue);
					return new PresenterResponse()
					{
						StatusCode = 200,
						Data = data
					};
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					return new PresenterResponse()
					{
						StatusCode = 500,
						ErrorMessage = e.Message
					};
				}
			}
			else
			{
				Debug.LogErrorFormat("Can not find requested url : {0} ", url);
				return new PresenterResponse()
				{
					StatusCode = 404,
					ErrorMessage = "Can not find requested url : " + url
				};
			}
		}


		public void RegistPresenterEvent(string url, IOnPresenterBroadcast register)
		{
			List<IOnPresenterBroadcast> list;
			if(!_registedEvents.TryGetValue(url, out list))
			{
				list = new List<IOnPresenterBroadcast>();
				_registedEvents.Add(url, list);
			}
			list.Add(register);
		}

		public void BroadcastEvent(string url, object[] data, bool needReceiver = false)
		{
			List<IOnPresenterBroadcast> list;
			if (!_registedEvents.TryGetValue(url, out list))
			{
				if (needReceiver)
					throw new Exception("can not find registed event " + url);
				else
					return;
			}
			var toSendArgs = new List<MToken>();
			for(var i = 0; i < data.Length; i++)
			{
				var d = data[i];
				var arg = d is MToken ? (MToken)d : MToken.FromObject(d);
				toSendArgs.Add(arg);
			}
			for(var i = 0; i < list.Count; i++)
			{
				var target = list[i];
				target.OnEvent(toSendArgs.ToArray());
			}
		}
	}
}