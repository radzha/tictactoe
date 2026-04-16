using Core;
using Game;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class GameHUD : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _timerText;

		[SerializeField]
		private TMP_Text _xMovesText;

		[SerializeField]
		private TMP_Text _oMovesText;

		[SerializeField]
		private TMP_Text _turnText;

		[SerializeField]
		private Button _settingsButton;

		[SerializeField]
		private SettingsPopup _settingsPopup;

		[SerializeField]
		private GameController _gameController;

		private bool _running = true;
		private int _lastShownSecond = -1;

		private void Start()
		{
			if (_gameController == null)
				_gameController = GameContext.Current;

			_settingsButton.onClick.AddListener(() =>
			{
				AudioService.Instance.PlayClick();
				_settingsPopup.Show();
			});

			if (_gameController != null)
			{
				_gameController.TurnChanged += OnTurnChanged;
				_gameController.CellMarked += OnCellMarked;
				_gameController.GameOver += OnGameOver;
				_gameController.GameRestarted += OnRestart;
			}
			else
			{
				Debug.LogError("GameHUD: GameController reference is missing.");
			}
		}

		private void Update()
		{
			if (!_running || !_gameController)
				return;

			var d = _gameController.MatchDuration;
			var totalSeconds = Mathf.FloorToInt(d);
			if (totalSeconds == _lastShownSecond)
				return;
			_lastShownSecond = totalSeconds;

			var m = totalSeconds / 60;
			var s = totalSeconds % 60;
			_timerText.text = $"{m:00}:{s:00}";
		}

		private void OnDestroy()
		{
			_settingsButton.onClick.RemoveAllListeners();
			if (_gameController != null)
			{
				_gameController.TurnChanged -= OnTurnChanged;
				_gameController.CellMarked -= OnCellMarked;
				_gameController.GameOver -= OnGameOver;
				_gameController.GameRestarted -= OnRestart;
			}
		}

		private void OnTurnChanged(Mark mark)
		{
			_turnText.text = mark == Mark.X ? "Player 1 (X)" : "Player 2 (O)";
		}

		private void OnCellMarked(int idx, Mark mark)
		{
			_xMovesText.text = $"X: {_gameController.XMoves}";
			_oMovesText.text = $"O: {_gameController.OMoves}";
		}

		private void OnGameOver(GameResult r, WinLine? l)
		{
			_running = false;
			_turnText.text = "";
		}

		private void OnRestart()
		{
			_running = true;
			_lastShownSecond = -1;
			_xMovesText.text = "X: 0";
			_oMovesText.text = "O: 0";
		}
	}
}
