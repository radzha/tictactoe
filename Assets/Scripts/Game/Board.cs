using UnityEngine;
namespace Game
{
	public class Board : MonoBehaviour
	{
		[SerializeField]
		private Cell _cellPrefab;

		[SerializeField]
		private Transform _cellContainer;

		[SerializeField]
		private StrikeLine _strikeLine;

		public Cell[] Cells { get; private set; }

		public void Init(GameController controller)
		{
			if (_cellContainer == null)
				_cellContainer = transform;

			if (Cells != null)
			{
				for (var i = 0; i < Cells.Length; i++)
				{
					if (Cells[i] != null)
						Destroy(Cells[i].gameObject);
				}
			}

			Cells = new Cell[9];
			for (var i = 0; i < 9; i++)
			{
				var cell = Instantiate(_cellPrefab, _cellContainer);
				cell.gameObject.name = $"Cell_{i}";
				cell.Setup(i, controller);
				Cells[i] = cell;
			}

			if (_strikeLine != null)
				_strikeLine.transform.SetAsLastSibling();
		}

		public Vector2 CellPosition(int index)
		{
			return Cells[index].Rect.TransformPoint(Vector3.zero);
		}

		public void Clear()
		{
			if (Cells != null)
			{
				foreach (var c in Cells)
					c.Clear();
			}

			if (_strikeLine != null)
				_strikeLine.Hide();
		}

		public void SetLocked(bool locked)
		{
			if (Cells == null)
				return;

			for (var i = 0; i < Cells.Length; i++)
			{
				var cell = Cells[i];
				if (cell != null)
					cell.SetInteractable(!locked);
			}
		}
	}
}
