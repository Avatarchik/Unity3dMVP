using Haruna.UnityMVP.Model;
using System;
using UnityEngine;

namespace Haruna.UnityMVP.Presenter
{
	public class AsyncReturn
	{
		//public string LinkerName { set; get; }
		//public Component Linker { set; get; }
		public Action<object[]> CallbackMethod { set; get; }
	}
	
	public class AsyncReturn<T> : AsyncReturn
	{
		public void Return(T arg)
		{
			CallbackMethod(new object[] { arg });
		}
	}
	public class AsyncReturn<T1, T2> : AsyncReturn
	{
		public void Return(T1 arg1, T2 arg2)
		{
			CallbackMethod(new object[] { arg1, arg2 });
		}
	}
	public class AsyncReturn<T1, T2, T3> : AsyncReturn
	{
		public void Return(T1 arg1, T2 arg2, T3 arg3)
		{
			CallbackMethod(new object[] { arg1, arg2, arg3 });
		}
	}
}