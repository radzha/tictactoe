using System.Collections;
using Core;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class Cell : MonoBehaviour
	{
		[SerializeField]
		private Image _icon;

		[SerializeField]
		private Button _button;

		private GameController _controller;

		private int _index;

		public RectTransform Rect => (RectTransform) transform;

		public void Setup(int index, GameController controller)
		{
			_index = index;
			_controller = controller;
			_button.onClick.AddListener(OnClick);
			Clear();
		}

		private void OnClick()
		{
			AudioService.Instance.PlayClick();
			_controller.OnCellClicked(_index);
		}

		public void ShowMark(Mark mark, ThemeConfig theme)
		{
			if (mark == Mark.None)
				return;
			if (theme == null)
				return;

			_icon.sprite = mark == Mark.X ? theme.XSprite : theme.OSprite;
			_icon.color = mark == Mark.X ? theme.XColor : theme.OColor;
			_icon.enabled = true;
			_button.interactable = false;

			StartCoroutine(PopAnim());
		}

		private IEnumerator PopAnim()
		{
			var rt = _icon.rectTransform;
			rt.localScale = Vector3.zero;
			var t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime * 5f;
				var s = Mathf.Sin(t * Mathf.PI * 0.5f);
				rt.localScale = Vector3.one * s;
				yield return null;
			}

			rt.localScale = Vector3.one;
		}

		public void Clear()
		{
			StopAllCoroutines();
			_icon.sprite = null;
			_icon.enabled = false;
			_icon.color = Color.white;
			_icon.rectTransform.localScale = Vector3.one;
			_button.interactable = true;
		}

		public void SetInteractable(bool interactable)
		{
			if (_button)
				_button.interactable = interactable;
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveAllListeners();
		}
	}
}
