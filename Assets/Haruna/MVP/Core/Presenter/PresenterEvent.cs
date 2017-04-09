using Haruna.UnityMVP.Model;

namespace Haruna.UnityMVP.Presenter
{
	public class PresenterEvent
	{
		public string Url { set; get; }
	}

	public class PresenterEvent<T> : PresenterEvent
	{
		public void Broadcast(T arg, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg }, needReceiver);
		}
	}
	public class PresenterEvent<T1, T2> : PresenterEvent
	{
		public void Broadcast(T1 arg1, T2 arg2, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg1, arg2 }, needReceiver);
		}
	}
	public class PresenterEvent<T1, T2, T3> : PresenterEvent
	{
		public void Broadcast(T1 arg1, T2 arg2, T3 arg3, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg1, arg2, arg3 }, needReceiver);
		}
	}

	public class PresenterEvent<T1, T2, T3, T4> : PresenterEvent
	{
		public void Broadcast(T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool needReceiver = false)
		{
			PresenterDispatcher.GetInstance().BroadcastEvent(Url, new object[] { arg1, arg2, arg3, arg4 }, needReceiver);
		}
	}
}