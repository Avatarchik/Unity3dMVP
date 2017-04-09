using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public class PresenterAction
	{
		public string Url;
		public Type PresenterType;
		public MethodInfo Method;
	}

	public static class PresenterUtil
	{
		static Dictionary<string, PresenterAction> _actionMapping = new Dictionary<string, PresenterAction>();
		static PresenterUtil()
		{
			FindAllPresenterAction();
		}

		static void FindAllPresenterAction()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					var attr = type.GetCustomAttributes(typeof(ViewPresenterAttribute), true);
					if (attr.Length == 0)
						continue;

					var presenterName = type.Name.EndsWith("Presenter") ?
						type.Name.Substring(0, type.Name.LastIndexOf("Presenter")) : type.Name;
					var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (var method in methods)
					{
						var url = presenterName + "/" + method.Name;
						if (_actionMapping.ContainsKey(url))
						{
							var existed = _actionMapping[url];
							Debug.LogErrorFormat("Url duplicate {0}.\n{1}, {2}.{3} \n{4}, {5}.{6}", url,
								existed.PresenterType.Assembly.FullName, existed.PresenterType.FullName, existed.Method.Name,
								assembly.FullName, type.FullName, method.Name);
						}
						else
						{
							var urlMapping = new PresenterAction()
							{
								Url = url,
								PresenterType = type,
								Method = method
							};
							_actionMapping.Add(url, urlMapping);
						}
					}
				}
			}
		}

		public static Dictionary<string, PresenterAction> GetAllPresenterAction()
		{
			return _actionMapping;
		}
	}
}
