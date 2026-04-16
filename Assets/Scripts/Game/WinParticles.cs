using Core;
using UnityEngine;

namespace Game
{
	public class WinParticles : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _burstParticles;

		[SerializeField]
		private GameController _gameController;

		private void Start()
		{
			if (_gameController == null)
				_gameController = GameContext.Current;

			if (_gameController != null)
			{
				_gameController.GameOver += OnGameOver;
				_gameController.GameRestarted += OnRestart;
			}
			else
			{
				Debug.LogError("WinParticles: GameController reference is missing.");
			}
		}

		private void OnDestroy()
		{
			if (_gameController != null)
			{
				_gameController.GameOver -= OnGameOver;
				_gameController.GameRestarted -= OnRestart;
			}
		}

		private void OnGameOver(GameResult result, WinLine? line)
		{
			if (result == GameResult.Draw || _burstParticles == null)
				return;
			_burstParticles.Play();
		}

		private void OnRestart()
		{
			if (_burstParticles != null)
				_burstParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}
}
