using System;
using Core;
using UnityEngine;
namespace Services
{
	[Serializable]
	public class StatsData
	{
		[SerializeField]
		private int _totalGames;

		[SerializeField]
		private int _p1Wins;

		[SerializeField]
		private int _p2Wins;

		[SerializeField]
		private int _draws;

		[SerializeField]
		private float _totalDuration;

		public int TotalGames => _totalGames;
		public int Player1Wins => _p1Wins;
		public int Player2Wins => _p2Wins;
		public int Draws => _draws;
		public float TotalDuration => _totalDuration;

		public float AverageDuration => _totalGames > 0 ? _totalDuration / _totalGames : 0f;

		public void Reset()
		{
			_totalGames = 0;
			_p1Wins = 0;
			_p2Wins = 0;
			_draws = 0;
			_totalDuration = 0f;
		}

		public void ApplyGameResult(GameResult result, float duration)
		{
			_totalGames++;
			_totalDuration += duration;

			switch (result)
			{
				case GameResult.XWins:
					_p1Wins++;
					break;
				case GameResult.OWins:
					_p2Wins++;
					break;
				case GameResult.Draw:
					_draws++;
					break;
			}
		}
	}

	public static class StatsService
	{
		private static StatsData _data;

		public static StatsData Data
		{
			get
			{
				if (_data == null)
					Load();
				return _data;
			}
		}

		private static void Load()
		{
			var json = PlayerPrefs.GetString(Prefs.Stats, "");
			_data = string.IsNullOrEmpty(json) ? new StatsData() : JsonUtility.FromJson<StatsData>(json);
		}

		private static void Save()
		{
			PlayerPrefs.SetString(Prefs.Stats, JsonUtility.ToJson(_data));
			PlayerPrefs.Save();
		}

		public static void RecordGame(GameResult result, float duration)
		{
			Data.ApplyGameResult(result, duration);

			Save();
		}

		public static void Reset()
		{
			Data.Reset();
			Save();
		}
	}
}
