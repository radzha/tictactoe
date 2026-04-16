using UnityEngine;

namespace Core
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		private static bool _quitting;

		public static T Instance
		{
			get
			{
				if (_quitting)
					return null;

				if (_instance != null)
					return _instance;

				_instance = FindAnyObjectByType<T>();
				if (_instance != null)
					return _instance;

				var go = new GameObject($"[{typeof(T).Name}]");
				_instance = go.AddComponent<T>();
				DontDestroyOnLoad(go);

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this as T;

			if (transform.parent != null)
			{
				transform.SetParent(null);
			}

			DontDestroyOnLoad(gameObject);
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		protected virtual void OnApplicationQuit()
		{
			_quitting = true;
		}

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			_instance = null;
			_quitting = false;
		}
#endif
	}
}
