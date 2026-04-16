using UnityEngine;

namespace Core
{
	public static class AppConfigProvider
	{
		// Keep this one string in one place; everything else is referenced from the asset.
		private const string ResourcePath = "AppConfig";
		private static AppConfig _cached;

		public static AppConfig Get()
		{
			if (_cached != null)
				return _cached;

			_cached = Resources.Load<AppConfig>(ResourcePath);
			if (_cached == null)
			{
				// Fallback: keep the build functional even if the asset wasn't created yet.
				// This should be replaced by a real AppConfig asset in Resources.
				Debug.LogWarning($"AppConfig not found at Resources/{ResourcePath}.asset. Using fallback Resources loading.");
				_cached = ScriptableObject.CreateInstance<AppConfig>();
				FallbackPopulate(_cached);
			}
			return _cached;
		}

		private static void FallbackPopulate(AppConfig config)
		{
			// Populate via SerializedObject-like reflection is overkill at runtime; use Resources and leave clips null if missing.
			// Themes are already ScriptableObjects so we can load them directly.
			var themes = Resources.LoadAll<ThemeConfig>("Themes");
			SetPrivate(config, "_themes", themes);

			SetPrivate(config, "_clickClip", Resources.Load<AudioClip>("Audio/click1"));
			SetPrivate(config, "_placeClip", Resources.Load<AudioClip>("Audio/click2"));
			SetPrivate(config, "_winClip", Resources.Load<AudioClip>("Audio/woosh"));
			SetPrivate(config, "_popupClip", Resources.Load<AudioClip>("Audio/pop"));
			SetPrivate(config, "_musicClip", Resources.Load<AudioClip>("Audio/music"));
		}

		private static void SetPrivate<T>(AppConfig config, string fieldName, T value)
		{
			var f = typeof(AppConfig).GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			f?.SetValue(config, value);
		}
	}
}

