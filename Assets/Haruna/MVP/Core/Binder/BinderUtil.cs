using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Haruna.UnityMVP.Model
{	
	public class RequireBinderInfo
	{
		public MTokenType TokenType;
		public string ValueTypeName;
		public Type ValueType;
		public Type InterfaceType;
	}

	public class ModelPropertyInfo
	{
		public MemberInfo MemberInfo;
		public RequireBinderInfo BinderInfo;
	}

	public static class BinderUtil
	{
		static List<Type> _implementedBinderValueTypes;
		static BinderUtil()
		{
			HashSet<string> binderValueTypes = new HashSet<string>();
			var allTypes = TypeUtil.GetAllTypes();
			foreach (var t in allTypes)
			{
				var @interface = t.GetInterface(typeof(Model.IMvpCustomTypeBinder<>).FullName);
				if (@interface != null)
				{
					var valueType = @interface.GetGenericArguments()[0];
					if (!valueType.IsPrimitive && valueType != typeof(string))
						binderValueTypes.Add(TypeUtil.GetAssemblyTypeString(valueType));
				}
			}
			_implementedBinderValueTypes = binderValueTypes.Select(s => TypeUtil.GetTypeWithAssemblyTypeString(s)).ToList();
		}

		public static Dictionary<string, ModelPropertyInfo> GetRequireBinderInfoFromModelMembers(Type modelType)
		{
			var ret = new Dictionary<string, ModelPropertyInfo>();
			var members = modelType.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			for (var i = 0; i < members.Length; i++)
			{
				var memberInfo = members[i];
				var attrs = memberInfo.GetCustomAttributes(typeof(ModelPropertyAttribute), true);
				if (attrs.Length == 0)
					continue;

				var temp = new ModelPropertyInfo();
				temp.MemberInfo = memberInfo;
				temp.BinderInfo = GetRequireBinderInfoByValueType(TypeUtil.GetFieldOrPropertyType(memberInfo));
				
				ret.Add(memberInfo.Name, temp);
			}
			return ret;
		}

		public static RequireBinderInfo GetRequireBinderInfoByValueType(Type valueType)
		{
			var temp = new RequireBinderInfo();
			temp.ValueType = valueType;
			if(temp.ValueType == typeof(MToken) || temp.ValueType.IsSubclassOf(typeof(MToken)))
			{
				temp.TokenType = MTokenType.Dynamic;
				temp.ValueTypeName = "dynamic";
			}
			else if (temp.ValueType.IsPrimitive)
			{
				if (temp.ValueType == typeof(bool))
				{
					temp.InterfaceType = typeof(IMvpBoolBinder);
					temp.TokenType = MTokenType.Bool;
					temp.ValueTypeName = "bool";
				}
				else
				{
					temp.InterfaceType = typeof(IMvpFloatBinder);
					temp.TokenType = MTokenType.Float;
					temp.ValueTypeName = "float";
				}
			}
			else if (temp.ValueType == typeof(string))
			{
				temp.InterfaceType = typeof(IMvpStringBinder);
				temp.TokenType = MTokenType.String;
				temp.ValueTypeName = "string";
			}
			else if (temp.ValueType.GetInterface(typeof(IList).FullName) != null)
			{
				temp.InterfaceType = typeof(IMvpArrayBinder);
				temp.TokenType = MTokenType.Array;
				temp.ValueTypeName = "array";
			}
			else if (temp.ValueType.GetCustomAttributes(typeof(MvpModelAttribute), true).Length != 0)
			{
				temp.InterfaceType = typeof(IMvpObjectBinder);
				temp.TokenType = MTokenType.Object;
				temp.ValueTypeName = string.Format("object({0})", temp.ValueType.Name);
			}
			else
			{
				temp.InterfaceType = typeof(IMvpCustomTypeBinder<>).MakeGenericType(temp.ValueType);
				temp.TokenType = MTokenType.Custom;
				temp.ValueTypeName = string.Format("custom({0})", temp.ValueType.Name); ;
			}

			return temp;
		}

		public static void SetValueToBinder(MToken value, UnityEngine.Object binderObject)
		{
			var method = binderObject.GetType().GetMethod("SetData");
			method.Invoke(binderObject, new object[] { value });
		}

		public static MToken GetValueFromBinder(UnityEngine.Object binderObject)
		{
			var method = binderObject.GetType().GetMethod("GetData");
			var value = method.Invoke(binderObject, null);
			return (MToken)value;
		}

		public static List<Type> GetImplementedBinderValueTypes()
		{
			return _implementedBinderValueTypes;
		}

		public static bool IsUnityEventHasError(UnityEngine.Events.UnityEventBase ev)
		{
			if (ev == null)
				return true;

			var count = ev.GetPersistentEventCount();
			for (var i = 0; i < count; i++)
			{
				var target = ev.GetPersistentTarget(i);
				if (target == null)
					return true;

				var methodName = ev.GetPersistentMethodName(i);
				if (string.IsNullOrEmpty(methodName))
					return true;

				var type = target.GetType();
				if (type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
					.All(m => m.Name != methodName))
					return true;
			}
			return false;
		}
	}
}
