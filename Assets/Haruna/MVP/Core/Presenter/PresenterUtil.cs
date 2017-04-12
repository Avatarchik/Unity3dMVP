using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public class PresenterActionInfo
	{
		public string Url;
		public Type PresenterType;
		public string DisplayUrl;
		public MethodInfo Method;
	}
	public class PresenterEventInfo
	{
		public string Url;
		public Type PresenterType;
		public string DisplayUrl;
		public FieldInfo Field;
	}

	public static class PresenterUtil
	{
		static Dictionary<string, PresenterActionInfo> _actionMapping = new Dictionary<string, PresenterActionInfo>();
		static Dictionary<string, PresenterEventInfo> _eventMapping = new Dictionary<string, PresenterEventInfo>();

		static PresenterUtil()
		{
			FindAllPresenterAction();
			FindAllPresenterEvents();
		}

		static void FindAllPresenterAction()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					if (type.GetCustomAttributes(typeof(PresenterAttribute), true).Length == 0)
						continue;

					var presenterName = type.Name.EndsWith("Presenter") ?
						type.Name.Substring(0, type.Name.LastIndexOf("Presenter")) : type.Name;
					var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (var method in methods)
					{
						var actionAttrs = method.GetCustomAttributes(typeof(PresenterActionAttribute), true);
						if (actionAttrs.Length == 0)
							continue;
						
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
							var actionInfo = new PresenterActionInfo()
							{
								Url = url,
								DisplayUrl = ((PresenterActionAttribute)(actionAttrs[0])).DisplayName,
								PresenterType = type,
								Method = method
							};
							_actionMapping.Add(url, actionInfo);
						}
					}
				}
			}
		}

		static void FindAllPresenterEvents()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					if (type.GetCustomAttributes(typeof(PresenterAttribute), true).Length == 0)
						continue;

					var presenterName = type.Name.EndsWith("Presenter") ?
						type.Name.Substring(0, type.Name.LastIndexOf("Presenter")) : type.Name;
					var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

					foreach (var field in fields)
					{
						if (field.FieldType == typeof(PresenterEvent) || field.FieldType.IsSubclassOf(typeof(PresenterEvent)))
						{
							var eventAttrs = field.GetCustomAttributes(typeof(PresenterEventAttribute), true);
							if (eventAttrs.Length == 0)
								continue;
							
							var url = presenterName + "/" + field.Name;
							if (_actionMapping.ContainsKey(url))
							{
								var existed = _actionMapping[url];
								Debug.LogErrorFormat("Url duplicate {0}.\n{1}, {2}.{3} \n{4}, {5}.{6}", url,
									existed.PresenterType.Assembly.FullName, existed.PresenterType.FullName, existed.Method.Name,
									assembly.FullName, type.FullName, field.Name);
							}
							else
							{
								var eventInfo = new PresenterEventInfo()
								{
									Url = url,
									DisplayUrl = ((PresenterEventAttribute)(eventAttrs[0])).DisplayName,
									PresenterType = type,
									Field = field
								};
								_eventMapping.Add(url, eventInfo);
							}
						}
					}
				}
			}
		}

		public static Dictionary<string, PresenterActionInfo> GetAllPresenterAction()
		{
			return _actionMapping;
		}
		public static Dictionary<string, PresenterEventInfo> GetAllPresetnerEvents()
		{
			return _eventMapping;
		}
	}
}
