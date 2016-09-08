using MapzenGo.Helpers.VectorD;
using UnityEngine;

namespace MapzenGo.Models
{
    public class Tile : MonoBehaviour
    {
        public JSONObject Data { get; set; }

        [SerializeField]
        public RectD Rect;
        public int Zoom { get; set; }
        public Vector2d TileTms { get; set; }
        public Vector3d TileCenter { get; set; }
        public bool UseLayers { get; set; }
        public Material Material { get; set; }
    }
}
