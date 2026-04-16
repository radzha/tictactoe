using Core;
using UnityEngine;

namespace Services
{
	public class AudioService : Singleton<AudioService>
	{
		private AudioSource _bgm;

		private AudioClip _clipClick;
		private AudioClip _clipPlace;
		private AudioClip _clipPopup;
		private AudioClip _clipWin;
		private AudioSource _sfx;

		public bool BGMEnabled { get; private set; } = true;

		public bool SfxEnabled { get; private set; } = true;

		protected override void Awake()
		{
			base.Awake();
			Init();
		}

		private void Init()
		{
			var config = AppConfigProvider.Get();

			_bgm = gameObject.AddComponent<AudioSource>();
			_bgm.loop = true;
			_bgm.playOnAwake = false;
			_bgm.volume = config != null ? config.MusicVolume : 0.4f;

			_sfx = gameObject.AddComponent<AudioSource>();
			_sfx.playOnAwake = false;

			if (config != null)
			{
				_clipClick = config.ClickClip;
				_clipPlace = config.PlaceClip;
				_clipWin = config.WinClip;
				_clipPopup = config.PopupClip;
			}

			if (config != null && config.MusicClip != null)
				_bgm.clip = config.MusicClip;

			BGMEnabled = PlayerPrefs.GetInt(Prefs.BgmEnabled, 1) == 1;
			SfxEnabled = PlayerPrefs.GetInt(Prefs.SfxEnabled, 1) == 1;

			if (BGMEnabled && _bgm.clip != null)
				_bgm.Play();
		}

		public void SetBGMEnabled(bool on)
		{
			BGMEnabled = on;
			PlayerPrefs.SetInt(Prefs.BgmEnabled, on ? 1 : 0);

			if (on && !_bgm.isPlaying && _bgm.clip != null)
				_bgm.Play();
			else if (!on)
				_bgm.Pause();
		}

		public void SetSfxEnabled(bool on)
		{
			SfxEnabled = on;
			PlayerPrefs.SetInt(Prefs.SfxEnabled, on ? 1 : 0);
		}

		public void PlayClick()
		{
			if (SfxEnabled && _clipClick)
				_sfx.PlayOneShot(_clipClick);
		}

		public void PlayPlace()
		{
			if (SfxEnabled && _clipPlace)
				_sfx.PlayOneShot(_clipPlace);
		}

		public void PlayWin()
		{
			if (SfxEnabled && _clipWin)
				_sfx.PlayOneShot(_clipWin);
		}

		public void PlayPopup()
		{
			if (SfxEnabled && _clipPopup)
				_sfx.PlayOneShot(_clipPopup);
		}
	}
}
