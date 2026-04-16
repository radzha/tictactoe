using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField]
		private Button _playButton;

		[SerializeField]
		private Button _statsButton;

		[SerializeField]
		private Button _settingsButton;

		[SerializeField]
		private Button _exitButton;

		[SerializeField]
		private ThemePopup _themePopup;

		[SerializeField]
		private StatsPopup _statsPopup;

		[SerializeField]
		private SettingsPopup _settingsPopup;

		[SerializeField]
		private ConfirmExitPopup _exitPopup;

		private void Start()
		{
			_ = AudioService.Instance;

			_playButton.onClick.AddListener(() =>
			{
				AudioService.Instance.PlayClick();
				_themePopup.Show();
			});

			_statsButton.onClick.AddListener(() =>
			{
				AudioService.Instance.PlayClick();
				_statsPopup.Show();
			});

			_settingsButton.onClick.AddListener(() =>
			{
				AudioService.Instance.PlayClick();
				_settingsPopup.Show();
			});

			_exitButton.onClick.AddListener(() =>
			{
				AudioService.Instance.PlayClick();
				_exitPopup.Show();
			});
		}

		private void OnDestroy()
		{
			_playButton.onClick.RemoveAllListeners();
			_statsButton.onClick.RemoveAllListeners();
			_settingsButton.onClick.RemoveAllListeners();
			_exitButton.onClick.RemoveAllListeners();
		}
	}
}
