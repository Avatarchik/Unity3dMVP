using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Haruna.Unity.View
{
	public class ViewManager : MonoBehaviour
	{
		static ViewManager _instance;
		public static ViewManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<ViewManager>();
					//DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}

		List<BaseView> _viewList = new List<BaseView>();

		[SerializeField]
		bool _initializeAtAwake;

		void Awake()
		{
			if (_initializeAtAwake)
				Init();
		}

		public void Init()
		{
			_viewList.AddRange(GetComponentsInChildren<BaseView>(true));
			foreach(var view in _viewList)
			{
				view.Init();
			}
		}

		public BaseView GetViewByName(string viewName)
		{
			return _viewList.Find(view => view.name == viewName);
		}

		public void ShowByName(string viewName)
		{
			var view = GetViewByName(viewName);
			if (view == null)
				Debug.LogErrorFormat("Can not get view by name {0}", viewName);
			else
				view.Show();
		}
		public void HideByName(string viewName)
		{
			var view = GetViewByName(viewName);
			if (view == null)
				Debug.LogErrorFormat("Can not get view by name {0}", viewName);
			else
				view.Hide();
		}
	}
}