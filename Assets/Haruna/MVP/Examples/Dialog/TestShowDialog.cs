using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using System;
using UnityEngine;

namespace Haruna.UnityMVP.Examples
{
	public class TestShowDialog : MonoBehaviour
	{
#if UNITY_EDITOR
		[UnityEditor.CustomEditor(typeof(TestShowDialog))]
		protected class TestShowDialogEditor : UnityEditor.Editor
		{
			public override void OnInspectorGUI()
			{
				if(GUILayout.Button("Show Message"))
				{
					TestDialogPresenter.ShowMessage("This is a message.", () =>
					{
						Debug.Log("message box close");
					});
				}

				if(GUILayout.Button("Show Dialog"))
				{
					TestDialogPresenter.ShowDialog("Test Dialog", "This is a dialog", "OK", "Deny", () =>
					{
						Debug.Log("user click ok");
					}, () =>
					{
						Debug.Log("user click cancel");
					});
				}
			}
		}
#endif
	}
}
