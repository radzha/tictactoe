using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class StrikeLine : MonoBehaviour
	{
		[SerializeField]
		private Image _lineImage;

		private RectTransform _rt;

		private void Awake()
		{
			_rt = _lineImage.rectTransform;
			_lineImage.enabled = false;
		}

		public void Show(Vector2 fromWorld, Vector2 toWorld, Color color, Action onComplete = null)
		{
			_lineImage.color = color;
			_lineImage.enabled = true;

			var parent = (RectTransform)_rt.parent;
			var from = (Vector2)parent.InverseTransformPoint(fromWorld);
			var to   = (Vector2)parent.InverseTransformPoint(toWorld);

			var mid = (from + to) * 0.5f;
			_rt.anchoredPosition = mid;

			var diff = to - from;
			var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			_rt.localRotation = Quaternion.Euler(0, 0, angle);

			var length = diff.magnitude + 60f;
			_rt.sizeDelta = new Vector2(0, 8f);

			StartCoroutine(Animate(length, onComplete));
		}

		private IEnumerator Animate(float targetWidth, Action onComplete)
		{
			var t = 0f;
			var duration = 0.35f;
			while (t < duration)
			{
				t += Time.deltaTime;
				var p = Mathf.SmoothStep(0, 1, t / duration);
				_rt.sizeDelta = new Vector2(targetWidth * p, 8f);
				yield return null;
			}

			_rt.sizeDelta = new Vector2(targetWidth, 8f);
			onComplete?.Invoke();
		}

		public void Hide()
		{
			StopAllCoroutines();
			_lineImage.enabled = false;
		}
	}
}
