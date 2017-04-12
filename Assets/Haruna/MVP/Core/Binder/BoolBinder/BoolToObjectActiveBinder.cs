using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruna.UnityMVP.Model
{
	public class BoolToObjectActiveBinder : MonoBehaviour, IMvpBoolBinder
	{
		[SerializeField]
		List<GameObject> _gameObjectsToSet;
		[SerializeField]
		List<GameObject> _gameObjectsToReverseSet;

		public void SetData(MBool data)
		{
			_gameObjectsToSet.ForEach(go =>
			{
				go.SetActive(data.Value);
			});
			_gameObjectsToReverseSet.ForEach(go =>
			{
				go.SetActive(!data.Value);
			});
		}

		public MBool GetData()
		{
			return new MBool(false);
		}
	}
}