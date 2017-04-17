using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using System;
using UnityEngine;

namespace Haruna.UnityMVP.Examples
{
	[MvpModel(DisplayName = "Examples/UserInfoModel")]
	public class UserInfoModel
	{
		[ModelProperty]
		public string UserId { set; get; }
		[ModelProperty]
		public string NickName { set; get; }
		[ModelProperty]
		public Sprite Portrait { set; get; } 
	}

	[Presenter]
	public static class TestUserInfoPresenter
	{
		[PresenterAction(DisplayName = "Examples/UserInfo.GetUserInfo")]
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
