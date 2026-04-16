using Core;

namespace Game
{
	public readonly struct PlaceOutcome
	{
		public bool Placed { get; }
		public bool GameOver { get; }
		public GameResult Result { get; }
		public WinLine? WinLine { get; }

		public PlaceOutcome(bool placed, bool gameOver, GameResult result, WinLine? winLine)
		{
			Placed = placed;
			GameOver = gameOver;
			Result = result;
			WinLine = winLine;
		}

		public static PlaceOutcome Invalid() => new (false, false, default, null);
	}

	public sealed class TicTacToeModel
	{
		private static readonly int[][] WinPatterns =
		{
			new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 },
			new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 },
			new[] { 0, 4, 8 }, new[] { 2, 4, 6 }
		};

		private readonly Mark[] _grid = new Mark[9];

		public int MoveCount { get; private set; }
		public bool IsLocked { get; private set; }

		public Mark GetMark(int index) => _grid[index];

		public void Reset()
		{
			for (var i = 0; i < 9; i++)
				_grid[i] = Mark.None;
			MoveCount = 0;
			IsLocked = false;
		}

		public PlaceOutcome TryPlace(int index, Mark mark)
		{
			if (IsLocked || index < 0 || index > 8 || mark == Mark.None || _grid[index] != Mark.None)
				return PlaceOutcome.Invalid();

			_grid[index] = mark;
			MoveCount++;

			if (CheckWin(mark, out var line))
			{
				IsLocked = true;
				var result = mark == Mark.X ? GameResult.XWins : GameResult.OWins;
				return new PlaceOutcome(true, true, result, line);
			}

			if (MoveCount >= 9)
			{
				IsLocked = true;
				return new PlaceOutcome(true, true, GameResult.Draw, null);
			}

			return new PlaceOutcome(true, false, default, null);
		}

		private bool CheckWin(Mark mark, out WinLine line)
		{
			foreach (var p in WinPatterns)
			{
				if (_grid[p[0]] == mark && _grid[p[1]] == mark && _grid[p[2]] == mark)
				{
					line = new WinLine(p[0], p[1], p[2]);
					return true;
				}
			}

			line = default;
			return false;
		}
	}
}

