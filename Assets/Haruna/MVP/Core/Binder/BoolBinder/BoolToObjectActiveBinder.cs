using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Haruna.UnityMVP.Model
{
	[AddComponentMenu("UnityMVP/Binder/BoolToObjectActiveBinder")]
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

		public bool HasEditorError()
		{
			return _gameObjectsToSet.Any(go => go == null)
				|| _gameObjectsToReverseSet.Any(go => go == null);
		}
	}
}