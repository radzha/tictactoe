namespace Core
{
	public enum Mark
	{
		None,
		X,
		O
	}

	public enum GameResult
	{
		XWins,
		OWins,
		Draw
	}

	public readonly struct WinLine
	{
		public int A { get; }
		public int B { get; }
		public int C { get; }

		public WinLine(int a, int b, int c)
		{
			A = a;
			B = b;
			C = c;
		}
	}
}
