using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ConfirmExitPopup : PopupBase
	{
		[SerializeField]
		private Button _confirmButton;

		[SerializeField]
		private Button _cancelButton;

		protected override void Awake()
		{
			base.Awake();
			_confirmButton.onClick.AddListener(OnConfirm);
			_cancelButton.onClick.AddListener(OnCancel);
		}

		private void OnDestroy()
		{
			_confirmButton.onClick.RemoveAllListeners();
			_cancelButton.onClick.RemoveAllListeners();
		}

		private void OnConfirm()
		{
			AudioService.Instance.PlayClick();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
			Hide();
#else
			Application.Quit();
#endif
		}

		private void OnCancel()
		{
			AudioService.Instance.PlayClick();
			Hide();
		}
	}
}
