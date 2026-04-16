using System;
using Core;

namespace Game
{
	public sealed class GameSession
	{
		private readonly TicTacToeModel _model;

		public bool IsActive { get; private set; }
		public Mark CurrentTurn { get; private set; } = Mark.X;
		public int XMoves { get; private set; }
		public int OMoves { get; private set; }

		public event Action<int, Mark> CellPlaced;
		public event Action<Mark> TurnChanged;
		public event Action<GameResult, WinLine?> GameEnded;
		public event Action Restarted;

		public GameSession(TicTacToeModel model)
		{
			_model = model ?? throw new ArgumentNullException(nameof(model));
		}

		public void StartMatch()
		{
			_model.Reset();
			CurrentTurn = Mark.X;
			XMoves = 0;
			OMoves = 0;
			IsActive = true;
			TurnChanged?.Invoke(CurrentTurn);
			Restarted?.Invoke();
		}

		public PlaceOutcome TryMakeMove(int index)
		{
			if (!IsActive)
				return PlaceOutcome.Invalid();

			var outcome = _model.TryPlace(index, CurrentTurn);
			if (!outcome.Placed)
				return outcome;

			if (CurrentTurn == Mark.X)
				XMoves++;
			else
				OMoves++;

			CellPlaced?.Invoke(index, CurrentTurn);

			if (outcome.GameOver)
			{
				IsActive = false;
				GameEnded?.Invoke(outcome.Result, outcome.WinLine);
				return outcome;
			}

			CurrentTurn = CurrentTurn == Mark.X ? Mark.O : Mark.X;
			TurnChanged?.Invoke(CurrentTurn);
			return outcome;
		}
	}
}

