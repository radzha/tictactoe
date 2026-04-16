using System.Collections;
using System;
using Core;
using Services;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Game
{
	public class GameController : MonoBehaviour
	{
		[SerializeField]
		private Board _board;

		[SerializeField]
		private StrikeLine _strikeLine;

		[SerializeField]
		private GameOverPopup _gameOverPopup;

		private float _matchDuration;
		private float _matchStart;
		private GameSession _session;
		private readonly TicTacToeModel _model = new ();

		public float MatchDuration => (_session != null && _session.IsActive) ? Time.time - _matchStart : _matchDuration;
		public int XMoves => _session?.XMoves ?? 0;
		public int OMoves => _session?.OMoves ?? 0;

		public ThemeConfig Theme { get; private set; }

		public event Action<int, Mark> CellMarked;
		public event Action<Mark> TurnChanged;
		public event Action<GameResult, WinLine?> GameOver;
		public event Action GameRestarted;

		private void OnEnable()
		{
			GameContext.Current = this;
		}

		private void OnDisable()
		{
			if (GameContext.Current == this)
				GameContext.Current = null;
		}

		private void Start()
		{
			var config = AppConfigProvider.Get();
			var themes = config != null ? config.Themes : null;
			if (themes is { Length: > 0 })
			{
				var idx = PlayerPrefs.GetInt(Prefs.ThemeIndex, 0);
				Theme = themes[Mathf.Abs(idx) % themes.Length];
			}

			_board.Init(this);
			if (_strikeLine == null && _board != null)
				_strikeLine = _board.GetComponentInChildren<StrikeLine>(true);
			if (_gameOverPopup == null)
				_gameOverPopup = FindAnyObjectByType<GameOverPopup>(FindObjectsInactive.Include);

			_session = new GameSession(_model);
			_session.CellPlaced += (i, m) => CellMarked?.Invoke(i, m);
			_session.TurnChanged += m => TurnChanged?.Invoke(m);
			_session.GameEnded += (r, l) => HandleGameOver(r, l);
			_session.Restarted += () => GameRestarted?.Invoke();

			BeginMatch();
		}

		private void BeginMatch()
		{
			_board.Clear();
			_board.SetLocked(false);

			_matchStart = Time.time;
			_matchDuration = 0f;

			_session?.StartMatch();
		}

		public void OnCellClicked(int index)
		{
			if (_session is not { IsActive: true })
				return;

			var mark = _session.CurrentTurn;
			var outcome = _session.TryMakeMove(index);
			if (!outcome.Placed)
				return;

			_board.Cells[index].ShowMark(mark, Theme);
			AudioService.Instance.PlayPlace();
		}

		private void HandleGameOver(GameResult result, WinLine? line)
		{
			_matchDuration = Time.time - _matchStart;
			_board.SetLocked(true);

			StatsService.RecordGame(result, _matchDuration);
			GameOver?.Invoke(result, line);

			if (line.HasValue)
			{
				var from = _board.CellPosition(line.Value.A);
				var to = _board.CellPosition(line.Value.C);
				var color = Theme != null ? Theme.StrikeColor : Color.white;

				AudioService.Instance.PlayWin();
				if (_strikeLine != null)
				{
					_strikeLine.Show(from, to, color, () => { StartCoroutine(ShowResultDelayed(result)); });
				}
				else
				{
					StartCoroutine(ShowResultDelayed(result));
				}
			}
			else
			{
				StartCoroutine(ShowResultDelayed(result));
			}
		}

		private IEnumerator ShowResultDelayed(GameResult result)
		{
			yield return new WaitForSeconds(0.5f);
			if (_gameOverPopup != null)
				_gameOverPopup.Show(result, _matchDuration);
		}

		public void RestartMatch()
		{
			BeginMatch();
		}

		public void ReturnToMenu()
		{
			SceneManager.LoadScene(SceneIds.Play);
		}
	}
}
