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
}