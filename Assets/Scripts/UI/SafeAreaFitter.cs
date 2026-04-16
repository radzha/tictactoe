using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(RectTransform))]
	public class SafeAreaFitter : MonoBehaviour
	{
		private Rect _lastSafeArea;
		private RectTransform _rt;

		private void Awake()
		{
			_rt = GetComponent<RectTransform>();
		}

		private void OnEnable()
		{
			Apply();
		}

		private void OnRectTransformDimensionsChange()
		{
			Apply();
		}

		private void Apply()
		{
			if (_rt == null)
				return;

			if (_lastSafeArea == Screen.safeArea)
				return;
			var sa = Screen.safeArea;
			_lastSafeArea = sa;
			var min = sa.position;
			var max = sa.position + sa.size;

			min.x /= Screen.width;
			min.y /= Screen.height;
			max.x /= Screen.width;
			max.y /= Screen.height;

			_rt.anchorMin = min;
			_rt.anchorMax = max;
		}
	}
}
