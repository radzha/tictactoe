using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class StatsPopup : PopupBase
	{
		[SerializeField]
		private TMP_Text _totalGamesText;

		[SerializeField]
		private TMP_Text _p1WinsText;

		[SerializeField]
		private TMP_Text _p2WinsText;

		[SerializeField]
		private TMP_Text _drawsText;

		[SerializeField]
		private TMP_Text _avgDurationText;

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private Button _resetButton;

		protected override void Awake()
		{
			base.Awake();
			_closeButton.onClick.AddListener(Hide);
			if (_resetButton)
				_resetButton.onClick.AddListener(OnReset);
		}

		private void OnDestroy()
		{
			_closeButton.onClick.RemoveAllListeners();
			if (_resetButton)
				_resetButton.onClick.RemoveAllListeners();
		}

		public override void Show()
		{
			Refresh();
			base.Show();
		}

		private void Refresh()
		{
			var d = StatsService.Data;
			_totalGamesText.text = d.TotalGames.ToString();
			_p1WinsText.text = d.Player1Wins.ToString();
			_p2WinsText.text = d.Player2Wins.ToString();
			_drawsText.text = d.Draws.ToString();
			_avgDurationText.text = FormatTime(d.AverageDuration);
		}

		private void OnReset()
		{
			AudioService.Instance.PlayClick();
			StatsService.Reset();
			Refresh();
		}

		private static string FormatTime(float seconds)
		{
			var m = Mathf.FloorToInt(seconds / 60f);
			var s = Mathf.FloorToInt(seconds % 60f);
			return $"{m:00}:{s:00}";
		}
	}
}
