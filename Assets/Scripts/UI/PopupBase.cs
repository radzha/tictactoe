using System.Collections;
using Services;
using UnityEngine;
namespace UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class PopupBase : MonoBehaviour
	{
		[SerializeField]
		protected RectTransform _panel;

		private CanvasGroup _cg;
		private bool _visible;

		protected virtual void Awake()
		{
			_cg = GetComponent<CanvasGroup>();
			SetImmediate(false);
		}

		public virtual void Show()
		{
			if (_visible)
				return;
			_visible = true;
			gameObject.SetActive(true);
			AudioService.Instance.PlayPopup();
			StopAllCoroutines();
			StartCoroutine(AnimateShow());
		}

		public virtual void Hide()
		{
			if (!_visible)
				return;
			_visible = false;
			StopAllCoroutines();
			StartCoroutine(AnimateHide());
		}

		private IEnumerator AnimateShow()
		{
			var t = 0f;
			_panel.localScale = Vector3.one * 0.7f;
			_cg.alpha = 0f;
			_cg.blocksRaycasts = true;
			_cg.interactable = true;

			while (t < 1f)
			{
				t += Time.unscaledDeltaTime * 5f;
				var p = Mathf.SmoothStep(0, 1, t);
				_cg.alpha = p;
				_panel.localScale = Vector3.Lerp(Vector3.one * 0.7f, Vector3.one, p);
				yield return null;
			}

			_cg.alpha = 1f;
			_panel.localScale = Vector3.one;
		}

		private IEnumerator AnimateHide()
		{
			var t = 0f;
			while (t < 1f)
			{
				t += Time.unscaledDeltaTime * 6f;
				var p = Mathf.SmoothStep(0, 1, t);
				_cg.alpha = 1f - p;
				_panel.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.85f, p);
				yield return null;
			}

			SetImmediate(false);
		}

		private void SetImmediate(bool visible)
		{
			_cg ??= GetComponent<CanvasGroup>();
			_cg.alpha = visible ? 1f : 0f;
			_cg.blocksRaycasts = visible;
			_cg.interactable = visible;
			if (!visible)
				gameObject.SetActive(false);
		}
	}
}
