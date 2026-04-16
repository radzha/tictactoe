using UnityEngine;

namespace Core
{
	[CreateAssetMenu(fileName = "AppConfig", menuName = "TicTacToe/App Config")]
	public sealed class AppConfig : ScriptableObject
	{
		[Header("Themes")]
		[SerializeField]
		private ThemeConfig[] _themes;

		[Header("Audio")]
		[SerializeField]
		private AudioClip _clickClip;

		[SerializeField]
		private AudioClip _placeClip;

		[SerializeField]
		private AudioClip _popupClip;

		[SerializeField]
		private AudioClip _winClip;

		[SerializeField]
		private AudioClip _musicClip;

		[SerializeField, Range(0f, 1f)]
		private float _musicVolume = 0.4f;

		public ThemeConfig[] Themes => _themes;

		public AudioClip ClickClip => _clickClip;
		public AudioClip PlaceClip => _placeClip;
		public AudioClip PopupClip => _popupClip;
		public AudioClip WinClip => _winClip;
		public AudioClip MusicClip => _musicClip;
		public float MusicVolume => _musicVolume;
	}
}

