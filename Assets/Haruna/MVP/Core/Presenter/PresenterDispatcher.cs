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
				_instance = new GameObject("LocalRequestDispatcher").AddComponent<PresenterDispatcher>();
				_instance.InitActionMapping();
				DontDestroyOnLoad(_instance);
			}
			return _instance;
		}
		
		Dictionary<string, PresenterAction> _actionMapping;
		
		void InitActionMapping()
		{
			_actionMapping = PresenterUtil.GetAllPresenterAction();
		}
		
		public IPresenterResponse RequestWithJTokens(string url, params MToken[] args)
		{
			PresenterAction action;
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
	}
}