using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class SettingsPopup : PopupBase
	{
		[SerializeField]
		private Toggle _bgmToggle;

		[SerializeField]
		private Toggle _sfxToggle;

		[SerializeField]
		private Button _closeButton;

		protected override void Awake()
		{
			base.Awake();

			_closeButton.onClick.AddListener(Hide);
			_bgmToggle.onValueChanged.AddListener(AudioService.Instance.SetBGMEnabled);
			_sfxToggle.onValueChanged.AddListener(AudioService.Instance.SetSfxEnabled);
		}

		private void OnDestroy()
		{
			_closeButton.onClick.RemoveAllListeners();
			_bgmToggle.onValueChanged.RemoveAllListeners();
			_sfxToggle.onValueChanged.RemoveAllListeners();
		}

		public override void Show()
		{
			_bgmToggle.SetIsOnWithoutNotify(AudioService.Instance.BGMEnabled);
			_sfxToggle.SetIsOnWithoutNotify(AudioService.Instance.SfxEnabled);
			base.Show();
		}
	}
}
