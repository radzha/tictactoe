using System.Collections.Generic;
using Core;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class ThemePopup : PopupBase
	{
		private static readonly Color SelectedColor = new (0.35f, 0.65f, 0.95f, 1f);
		private static readonly Color NormalColor = new (0.25f, 0.25f, 0.30f, 1f);

		[SerializeField]
		private Transform _themeContainer;

		[SerializeField]
		private GameObject _themeButtonPrefab;

		[SerializeField]
		private Button _startButton;

		[SerializeField]
		private Button _closeButton;

		private readonly List<Image> _buttonBackgrounds = new ();
		private int _selected;

		private ThemeConfig[] _themes;

		protected override void Awake()
		{
			base.Awake();

			_themes = (AppConfigProvider.Get() != null ? AppConfigProvider.Get().Themes : null) ?? new ThemeConfig[0];

			_startButton.onClick.AddListener(OnStart);
			_closeButton.onClick.AddListener(Hide);

			PopulateThemes();
		}

		private void OnDestroy()
		{
			_startButton.onClick.RemoveAllListeners();
			_closeButton.onClick.RemoveAllListeners();
		}

		private void PopulateThemes()
		{
			if (_themes.Length == 0)
				return;

			for (var i = 0; i < _themes.Length; i++)
			{
				var idx = i;
				var go = Instantiate(_themeButtonPrefab, _themeContainer);
				go.SetActive(true);

				var label = go.GetComponentInChildren<TMP_Text>();
				if (label)
					label.text = _themes[i] != null ? _themes[i].ThemeName : "Theme";

				var bg = go.GetComponent<Image>();
				if (bg)
					_buttonBackgrounds.Add(bg);

				var btn = go.GetComponent<Button>();
				btn.onClick.AddListener(() => Select(idx));
			}

			var saved = PlayerPrefs.GetInt(Prefs.ThemeIndex, 0);
			Select(Mathf.Clamp(saved, 0, _themes.Length - 1));
		}

		private void Select(int index)
		{
			_selected = index;
			AudioService.Instance.PlayClick();

			for (var i = 0; i < _buttonBackgrounds.Count; i++)
				_buttonBackgrounds[i].color = i == _selected ? SelectedColor : NormalColor;
		}

		private void OnStart()
		{
			AudioService.Instance.PlayClick();
			PlayerPrefs.SetInt(Prefs.ThemeIndex, _selected);
			SceneManager.LoadScene(SceneIds.Game);
		}
	}
}
