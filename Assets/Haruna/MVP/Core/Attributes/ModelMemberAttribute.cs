using System;

namespace Haruna.UnityMVP.Model
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class ModelPropertyAttribute : Attribute
	{
	}
}