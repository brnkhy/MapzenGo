using UnityEngine;

namespace MapzenGo.Models
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        public RectD Rect;
        public int Zoom { get; set; }
        public Vector2d TileTms { get; set; }
        public Vector3d TileCenter { get; set; }
        public bool UseLayers { get; set; }
        public Material Material { get; set; }
    }
}
