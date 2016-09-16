using System;
using MapzenGo.Helpers.VectorD;
using UnityEngine;

namespace MapzenGo.Models
{
    public class Tile : MonoBehaviour
    {
        public delegate void DestroyedEventHandler(Tile sender, EventArgs e);
        public event DestroyedEventHandler Destroyed;

        public JSONObject Data { get; set; }

        [SerializeField]
        public RectD Rect;
        public int Zoom { get; set; }
        public Vector2d TileTms { get; set; }
        public Vector3d TileCenter { get; set; }
        public bool UseLayers { get; set; }
        public Material Material { get; set; }

        public void OnDestroy()
        {
            if (Destroyed != null)
            {
                Destroyed(this, null);
            }
        }



    }
}
