using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using System;
using System.Collections.Generic;
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
		[ModelProperty]
		public List<float> Numbers { set; get; }
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
			info.Numbers = new List<float>() { 23, 44, 5, 6 };
			return info;
		}

		[PresenterAction(DisplayName = "Examples/UserInfo.GetUserInfoAsync")]
		public static void GetUserInfoAsync(string uid, AsyncReturn<UserInfoModel> userInfo)
		{
			var info = new UserInfoModel();
			info.UserId = "uid : " + uid;
			info.NickName = "John Smith";
			info.Portrait = Resources.Load<Sprite>("UnityMVP_Example_Portrait");
			info.Numbers = new List<float>() { 23, 44, 5, 6 };

			System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				System.Threading.Thread.Sleep(2000);
				userInfo.Return(info);
			}));

			thread.Start();
		}
	}
}
