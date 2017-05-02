using Haruna.UnityMVP.Model;
using System;
using System.Collections.Generic;

namespace Haruna.UnityMVP.Presenter
{
	public class PresenterEvent
	{
		public string Url { set; get; }
		public virtual Dictionary<string, Type> ConditionTypes { get { return null; } }
		public virtual Dictionary<string, Type> ParameterTypes { get { return null; } }
	}

	public class PresenterEvent<T> : PresenterEvent
	{
		public override Dictionary<string, Type> ParameterTypes
		{
			get { return new Dictionary<string, Type>() { { "arg", typeof(T) } }; }
		}

		public void Broadcast(T arg, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg }, c => true, needReceiver);
		}
	}
	public class PresenterEvent<T0, T1> : PresenterEvent
	{
		public override Dictionary<string, Type> ParameterTypes
		{
			get
			{
				return new Dictionary<string, Type>()
				{
					{ "arg 0", typeof(T0) },
					{ "arg 1", typeof(T1) }
				};
			}
		}

		public void Broadcast(T0 arg0, T1 arg1, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg0, arg1 }, c => true, needReceiver);
		}
	}
	public class PresenterEvent<T0, T1, T2> : PresenterEvent
	{
		public override Dictionary<string, Type> ParameterTypes
		{
			get
			{
				return new Dictionary<string, Type>()
				{
					{ "arg 0", typeof(T0) },
					{ "arg 1", typeof(T1) },
					{ "arg 2", typeof(T2) },
				};
			}
		}
		public void Broadcast(T0 arg1, T1 arg2, T2 arg3, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg1, arg2, arg3 }, c => true, needReceiver);
		}
	}

	public class PresenterEventWithIndex : PresenterEvent
	{
		public override Dictionary<string, Type> ConditionTypes
		{
			get { return new Dictionary<string, Type>() { { "Index", typeof(float) } }; }
		}

		public void Broadcast(int index, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { }, c => ((MFloat)c).Value == index, needReceiver);
		}
	}
	public class PresenterEventWithIndex<T> : PresenterEventWithIndex
	{
		public override Dictionary<string, Type> ParameterTypes
		{
			get { return new Dictionary<string, Type>() { { "arg", typeof(T) } }; }
		}

		public void Broadcast(int index, T arg, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg }, c => ((MFloat)c).Value == index, needReceiver);
		}
	}
	public class PresenterEventWithIdentity : PresenterEvent
	{
		public override Dictionary<string, Type> ConditionTypes
		{
			get { return new Dictionary<string, Type>() { { "ID", typeof(string) } }; }
		}

		public void Broadcast(string id, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { }, c => ((MString)c).Value == id, needReceiver);
		}
	}
	public class PresenterEventWithIdentity<T> : PresenterEventWithIdentity
	{
		public override Dictionary<string, Type> ParameterTypes
		{
			get { return new Dictionary<string, Type>() { { "arg", typeof(T) } }; }
		}

		public void Broadcast(string id, T arg, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg }, c => ((MString)c).Value == id, needReceiver);
		}
	}
}