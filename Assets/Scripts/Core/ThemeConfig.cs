using UnityEngine;
using UnityEngine.Serialization;
namespace Core
{
	[CreateAssetMenu(fileName = "Theme", menuName = "TicTacToe/Theme")]
	public class ThemeConfig : ScriptableObject
	{
		[SerializeField, FormerlySerializedAs("_themeName")]
		private string _themeName = "Default";

		[SerializeField, FormerlySerializedAs("_xSprite")]
		private Sprite _xSprite;

		[SerializeField, FormerlySerializedAs("_oSprite")]
		private Sprite _oSprite;

		[SerializeField, FormerlySerializedAs("_xColor")]
		private Color _xColor = new (0.22f, 0.56f, 0.89f);

		[SerializeField, FormerlySerializedAs("_oColor")]
		private Color _oColor = new (0.89f, 0.32f, 0.28f);

		[SerializeField, FormerlySerializedAs("_strikeColor")]
		private Color _strikeColor = Color.white;

		public string ThemeName => _themeName;
		public Sprite XSprite => _xSprite;
		public Sprite OSprite => _oSprite;
		public Color XColor => _xColor;
		public Color OColor => _oColor;
		public Color StrikeColor => _strikeColor;
	}
}
