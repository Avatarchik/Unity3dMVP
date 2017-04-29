using Haruna.UnityMVP.Model;
using System;
using System.Linq;
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

		class AsyncActionTask
		{
			public bool Completed;

			public PresenterActionInfo ActionInfo;
			public string CallerName;
			public Component CallLinker;
			public Action<IPresenterResponse> OnResponse;
			public object[] Data;
		}
		List<AsyncActionTask> _asyncActionTasks = new List<AsyncActionTask>();

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
		
		void Update()
		{
			lock(_asyncActionTasks)
			{
				for (var i = 0; i < _asyncActionTasks.Count; i++)
				{
					var task = _asyncActionTasks[i];
					if (task.Completed)
					{
						if (task.CallLinker == null)
						{
							Debug.LogWarningFormat("liner {0} is already destroyed", task.CallerName);
						}
						else
						{
							var responseArgs = task.Data.Select(a => a is MToken ? (MToken)a : MToken.FromObject(a)).ToArray();
							task.OnResponse(new PresenterResponse()
							{
								StatusCode = 200,
								Data = responseArgs
							});
						}
					}
				}
				_asyncActionTasks.RemoveAll(a => a.Completed);
			}
		}

		public IPresenterResponse RequestWithMTokens(string url, params MToken[] args)
		{
			PresenterActionInfo action;
			if(!_actionMapping.TryGetValue(url, out action))
			{
				Debug.LogErrorFormat("Can not find requested url : {0} ", url);
				return new PresenterResponse()
				{
					StatusCode = 404,
					ErrorMessage = "Can not find requested url : " + url
				};
			}

			var parameters = action.Method.GetParameters();
			if (parameters.Length != args.Length)
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
					if (parameterType != typeof(MToken) && !parameterType.IsSubclassOf(typeof(MToken)))
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
				if (returnValue is IPresenterResponse)
				{
					return (IPresenterResponse)returnValue;
				}

				var data = returnValue is MToken ? (MToken)returnValue : MToken.FromObject(returnValue);
				return new PresenterResponse()
				{
					StatusCode = 200,
					Data = new MToken[] { data }
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

		public void RequestWithMTokensAsync(Component caller, Action<IPresenterResponse> onResponse, string url, params MToken[] args)
		{
			PresenterActionInfo action;
			if (!_actionMapping.TryGetValue(url, out action))
			{
				Debug.LogErrorFormat("Can not find requested url : {0} ", url);
				onResponse(new PresenterResponse()
				{
					StatusCode = 404,
					ErrorMessage = "Can not find requested url : " + url
				});
				return;
			}

			var parameters = action.Method.GetParameters();
			if (parameters.Length - 1 != args.Length)
			{
				onResponse(new PresenterResponse()
				{
					StatusCode = 500,
					ErrorMessage = string.Format("request parameter count {0} is not equal with action method parameter count {1}",
					parameters.Length, args.Length)
				});
				return;
			}
			var lastParameter = parameters[parameters.Length - 1];
			if (lastParameter.ParameterType != typeof(AsyncReturn)
				&& !lastParameter.ParameterType.IsSubclassOf(typeof(AsyncReturn)))
			{
				onResponse(new PresenterResponse()
				{
					StatusCode = 500,
					ErrorMessage = string.Format("for async action, the last parameter should be AsyncReturn or its sub class. current is {0}", lastParameter.ParameterType),
				});
				return;
			}

			try
			{
				var pList = new List<object>();
				for (var i = 0; i < args.Length; i++)
				{
					var parameterType = parameters[i].ParameterType;
					if (parameterType != typeof(MToken) && !parameterType.IsSubclassOf(typeof(MToken)))
					{
						var temp = args[i].ToObject(parameterType);
						pList.Add(temp);
					}
					else
					{
						pList.Add(args[i]);
					}
				}

				var task = new AsyncActionTask();
				task.ActionInfo = action;
				task.CallLinker = caller;
				task.CallerName = caller.name;
				task.OnResponse = onResponse;
				_asyncActionTasks.Add(task);

				var asyncReturn = Activator.CreateInstance(lastParameter.ParameterType) as AsyncReturn;
				pList.Add(asyncReturn);

				asyncReturn.CallbackMethod = (returnArgs) =>
				{
					lock (_asyncActionTasks)
					{
						task.Completed = true;
						task.Data = returnArgs;
					}
				};

				action.Method.Invoke(null, pList.ToArray());
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				onResponse(new PresenterResponse()
				{
					StatusCode = 500,
					ErrorMessage = e.Message
				});
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

		public void BroadcastEvent(string url, object[] data, Func<IOnPresenterBroadcast, bool> validater, bool needReceiver = false)
		{
			List<IOnPresenterBroadcast> list;
			if (!_registedEvents.TryGetValue(url, out list))
			{
				if (needReceiver)
					throw new Exception("can not find registed event " + url);
				else
					return;
			}

			var toBroadCastEvent = validater == null ? list.ToArray() : list.Where(e => validater(e)).ToArray();
			if (toBroadCastEvent.Count() == 0)
			{
				if (needReceiver)
					throw new Exception("can not find registed event " + url);
				else
					return;
			}
			var toSendArgs = new List<MToken>();
			for (var i = 0; i < data.Length; i++)
			{
				var d = data[i];
				var arg = d is MToken ? (MToken)d : MToken.FromObject(d);
				toSendArgs.Add(arg);
			}
			for (var i = 0; i < toBroadCastEvent.Length; i++)
			{
				var target = toBroadCastEvent[i];
				target.OnEvent(toSendArgs.ToArray());
			}
		}
	}
}