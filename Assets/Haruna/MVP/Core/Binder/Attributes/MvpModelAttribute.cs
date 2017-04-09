using System;

namespace Haruna.UnityMVP.Model
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public class MvpModelAttribute : Attribute
	{
	}
}