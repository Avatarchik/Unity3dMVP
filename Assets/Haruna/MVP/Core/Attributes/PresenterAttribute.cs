using System;
using System.Collections.Generic;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class PresenterAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class PresenterActionAttribute : Attribute
	{
		public string DisplayName { set; get; }
	}
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class PresenterEventAttribute : Attribute
	{
		public string DisplayName { set; get; }
	}
}