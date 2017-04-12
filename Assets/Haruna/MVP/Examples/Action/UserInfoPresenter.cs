using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using System;
using UnityEngine;

namespace Haruna.UnityMVP.Examples
{
	[MvpModel]
	public class UserInfoModel
	{
		[ModelProperty]
		public string UserId { set; get; }
		[ModelProperty]
		public string NickName { set; get; }
		[ModelProperty]
		public Sprite Portrait { set; get; } 
	}

	[PresenterAction]
	public static class UserInfoPresenter
	{
		public static UserInfoModel GetUserInfo(string uid)
		{
			var info = new UserInfoModel();
			info.UserId = "uid : " + uid;
			info.NickName = "John Smith";
			info.Portrait = Resources.Load<Sprite>("UnityMVP_Example_Portrait");
			
			return info;
		}
	}
}
