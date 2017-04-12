using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Haruna.Unity.View
{
	public class BaseView : MonoBehaviour
	{
		[SerializeField]
		UnityEvent _initializeEvent;
		public event UnityAction EvOnInitialize
		{
			add { _initializeEvent.AddListener(value); }
			remove { _initializeEvent.RemoveListener(value); }
		}
		[SerializeField]
		UnityEvent _showEvent;
		public event UnityAction EvOnShow
		{
			add { _showEvent.AddListener(value); }
			remove { _showEvent.RemoveListener(value); }
		}
		[SerializeField]
		UnityEvent _hideEvent;
		public event UnityAction EvOnHide
		{
			add { _hideEvent.AddListener(value); }
			remove { _hideEvent.RemoveListener(value); }
		}

		//[SerializeField]
		//PanelLayerEnum _sortingLayer = PanelLayerEnum.Default;
		//public PanelLayerEnum SortingLayer { get { return _sortingLayer; } }
		[SerializeField]
		bool _setActiveOnShow;
		[SerializeField]
		bool _setDeactiveOnHide;

		public enum ActionOnInitializeEnum
		{
			DoNothing,
			ShowOnInitializing,
			HideOnInitializing
		}
		[SerializeField]
		ActionOnInitializeEnum _actionOnInializing = ActionOnInitializeEnum.DoNothing;

		public virtual void Init()
		{
			if (_initializeEvent != null)
				_initializeEvent.Invoke();

			if (_actionOnInializing == ActionOnInitializeEnum.ShowOnInitializing)
				Show();
			else if (_actionOnInializing == ActionOnInitializeEnum.HideOnInitializing)
				Hide();
		}

		protected virtual void Start()
		{
		}

		public virtual void Show()
		{
			if (_setActiveOnShow)
				gameObject.SetActive(true);

			if (_showEvent != null)
				_showEvent.Invoke();
		}

		public virtual void Hide()
		{
			if (_setDeactiveOnHide)
				gameObject.SetActive(false);

			if (_hideEvent != null)
				_hideEvent.Invoke();
		}

	}
}