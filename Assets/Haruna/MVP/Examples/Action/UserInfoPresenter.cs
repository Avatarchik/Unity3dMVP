using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using UnityEngine;

namespace Haruna.UnityMVP.Examples
{
	[MvpModel]
	public class ToDisplayUserInfo
	{
		[ModelProperty]
		public int UserId { set; get; }
		[ModelProperty]
		public string NickName { set; get; }
		[ModelProperty]
		public Sprite Portrait { set; get; } 
	}

	[PresenterAction]
	public static class UserInfoPresenter
	{
		public static ToDisplayUserInfo GetUserInfo()
		{
			var info = new ToDisplayUserInfo();
			info.UserId = 100;
			info.NickName = "John Smith";
			info.Portrait = Resources.Load<Sprite>("");

			return info;
		}
	}
}
