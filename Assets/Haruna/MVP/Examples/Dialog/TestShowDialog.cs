using Haruna.UnityMVP.Model;
using Haruna.UnityMVP.Presenter;
using System;
using UnityEngine;

namespace Haruna.UnityMVP.Examples
{
	[ExecuteInEditMode]
	public class TestShowDialog : MonoBehaviour
	{
#if UNITY_EDITOR
		[UnityEditor.CustomEditor(typeof(TestShowDialog))]
		protected class TestShowDialogEditor : UnityEditor.Editor
		{
			public override void OnInspectorGUI()
			{
				if(GUILayout.Button("Show Message") && Application.isPlaying)
				{
					TestDialogPresenter.ShowMessage("This is a message.", () =>
					{
						Debug.Log("message box close");
					});
				}

				if(GUILayout.Button("Show Dialog") && Application.isPlaying)
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
