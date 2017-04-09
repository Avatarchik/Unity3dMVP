using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Haruna.UnityMVP
{
	public static class TypeUtil
	{
		public static List<Type> GetAllTypes(Func<Type, bool> predicate = null)
		{
			var ret = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					if (predicate == null || predicate(type))
						ret.Add(type);
				}
			}

			return ret;
		}

		public static string GetAssemblyTypeString(Type type)
		{
			return type.FullName + ";" + type.Assembly.FullName;
		}

		public static Type GetTypeWithAssemblyTypeString(string assemblyTypeString, UnityEngine.Object caller = null)
		{
			if (string.IsNullOrEmpty(assemblyTypeString))
				return null;

			var temp = assemblyTypeString.Split(';');
			var assembly = Assembly.Load(temp[1]);
			if (assembly == null)
			{
				Debug.LogErrorFormat(caller, "Can not load assembly {0}", temp[0]);
				return null;
			}
			var type = assembly.GetType(temp[0]);
			if (type == null)
			{
				Debug.LogErrorFormat(caller, "Can not get type {0}", assemblyTypeString);
			}
			return type;
		}
		
		public static Type GetFieldOrPropertyType(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType == MemberTypes.Field)
			{
				return ((FieldInfo)memberInfo).FieldType;
			}
			else if (memberInfo.MemberType == MemberTypes.Property)
			{
				return ((PropertyInfo)memberInfo).PropertyType;
			}
			return null;
		}

		public static object GetValueFromFieldOrProperty(MemberInfo memberInfo, object obj)
		{
			if(memberInfo.MemberType == MemberTypes.Field)
			{
				return ((FieldInfo)memberInfo).GetValue(obj);
			}
			else if(memberInfo.MemberType == MemberTypes.Property)
			{
				return ((PropertyInfo)memberInfo).GetValue(obj, null);
			}
			else
			{
				Debug.LogErrorFormat("member is not field or property. {0}.{1} {2}"
					, memberInfo.DeclaringType.Name, memberInfo.Name, memberInfo.MemberType.ToString());
				return null;
			}
		}

		public static void SetValueFromFieldOrProperty(string memberName, object obj, object value)
		{
			var type = obj.GetType();
			var member = type.GetMember(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if(member.Length == 0)
			{
				throw new Exception(string.Format("Can not get member {0} in type {1}", memberName, type.FullName));
			}
			SetValueFromFieldOrProperty(member[0], obj, value);
		}

		public static void SetValueFromFieldOrProperty(MemberInfo memberInfo, object obj, object value)
		{
			if (memberInfo.MemberType == MemberTypes.Field)
			{
				((FieldInfo)memberInfo).SetValue(obj, value);
			}
			else if (memberInfo.MemberType == MemberTypes.Property)
			{
				((PropertyInfo)memberInfo).SetValue(obj, value, null);
			}
			else
			{
				Debug.LogErrorFormat("member is not field or property. {0}.{1} {2}"
					, memberInfo.DeclaringType.Name, memberInfo.Name, memberInfo.MemberType.ToString());
			}
		}
	}
}
