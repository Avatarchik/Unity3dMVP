using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using System;
using UnityEngine;

namespace Haruna.UnityMVP.Examples
{
	[Presenter]
	public static class DialogPresenter
	{
		[MvpModel]
		public class DialogSettings
		{
			[ModelProperty]
			public bool ShowButton;
			[ModelProperty]
			public bool ShowTitle;
			[ModelProperty]
			public string Title;
			[ModelProperty]
			public string Content;
			[ModelProperty]
			public string ConfirmButtonStr;
			[ModelProperty]
			public string CancelButtonStr;
			[ModelProperty]
			public Action OnConfirm;
			[ModelProperty]
			public Action OnCancel;
			[ModelProperty]
			public bool CloseOnMaskClick;
			[ModelProperty]
			public Action OnMaskClick;
		}

		[PresenterEvent]
		static PresenterEvent<DialogSettings> ShowDialogEvent = new PresenterEvent<DialogSettings>();
		
		public static void ShowMessage(string content, Action onClose = null)
		{
			ShowDialogEvent.Broadcast(new DialogSettings()
			{
				ShowButton = false,
				Content = content,
				CloseOnMaskClick = true,
				OnMaskClick = onClose
			});

		}
		public static void ShowDialog(string title, string content, 
			string confirmButtonStr, string cancelButtonStr, Action onConfirm, Action onCancel = null)
		{
			ShowDialogEvent.Broadcast(new DialogSettings()
			{
				ShowButton = true,
				Content = content,
				ConfirmButtonStr = confirmButtonStr,
				CancelButtonStr = cancelButtonStr,
				OnConfirm = onConfirm,
				OnCancel = onCancel,
				CloseOnMaskClick = false,
				OnMaskClick = null
			});
		}
	}
}
