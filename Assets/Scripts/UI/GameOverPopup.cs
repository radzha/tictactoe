using Core;
using Game;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class GameOverPopup : PopupBase
	{
		[SerializeField]
		private GameController _gameController;

		[SerializeField]
		private TMP_Text _resultText;

		[SerializeField]
		private TMP_Text _durationText;

		[SerializeField]
		private Button _retryButton;

		[SerializeField]
		private Button _exitButton;

		protected override void Awake()
		{
			base.Awake();
			_retryButton.onClick.AddListener(OnRetry);
			_exitButton.onClick.AddListener(OnExit);
		}

		private void OnDestroy()
		{
			_retryButton.onClick.RemoveAllListeners();
			_exitButton.onClick.RemoveAllListeners();
		}

		public void Show(GameResult result, float duration)
		{
			_resultText.text = result switch
			{
				GameResult.XWins => "Player 1 Wins!",
				GameResult.OWins => "Player 2 Wins!",
				_ => "Draw!"
			};

			var m = Mathf.FloorToInt(duration / 60f);
			var s = Mathf.FloorToInt(duration % 60f);
			_durationText.text = $"Duration: {m:00}:{s:00}";

			base.Show();
		}

		private void OnRetry()
		{
			AudioService.Instance.PlayClick();
			Hide();
			if (_gameController == null)
				_gameController = GameContext.Current;
			if (_gameController)
				_gameController.RestartMatch();
		}

		private void OnExit()
		{
			AudioService.Instance.PlayClick();
			if (_gameController == null)
				_gameController = GameContext.Current;
			if (_gameController)
				_gameController.ReturnToMenu();
		}
	}
}
