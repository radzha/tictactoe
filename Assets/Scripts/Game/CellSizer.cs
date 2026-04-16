using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	[RequireComponent(typeof(GridLayoutGroup))]
	public class CellSizer : MonoBehaviour
	{
		private GridLayoutGroup _grid;
		private Vector2 _lastSize;
		private RectTransform _rt;

		private void Awake()
		{
			_grid = GetComponent<GridLayoutGroup>();
			_rt = GetComponent<RectTransform>();
		}

		private void OnEnable()
		{
			RecalculateIfNeeded();
		}

		private void OnRectTransformDimensionsChange()
		{
			RecalculateIfNeeded();
		}

		private void RecalculateIfNeeded()
		{
			if (_rt == null || _grid == null)
				return;

			var size = _rt.rect.size;
			if (size == _lastSize)
				return;
			_lastSize = size;
			Recalculate(size);
		}

		private void Recalculate(Vector2 area)
		{
			var spacing = _grid.spacing.x;
			var available = Mathf.Min(area.x, area.y) - spacing * 2f;
			var cellSize = Mathf.Floor(available / 3f);
			_grid.cellSize = new Vector2(cellSize, cellSize);

			var totalGrid = cellSize * 3 + spacing * 2;
			var padX = Mathf.Max(0, (area.x - totalGrid) * 0.5f);
			var padY = Mathf.Max(0, (area.y - totalGrid) * 0.5f);
			_grid.padding = new RectOffset((int) padX, (int) padX, (int) padY, (int) padY);
		}
	}
}
