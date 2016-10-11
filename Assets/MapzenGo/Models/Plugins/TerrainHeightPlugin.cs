using System;
using System.Collections.Generic;
using MapzenGo.Helpers;
using MapzenGo.Models;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Plugins
{
    public class TerrainHeightPlugin : Plugin
    {
        public enum TileServices
        {
            Default,
            Satellite,
            Terrain,
            Toner,
            Watercolor
        }

        public TileServices TileService = TileServices.Default;
        [SerializeField] protected string _key = "mapzen-cdjccvm";
        private string[] TileServiceUrls = new string[] {
            "http://b.tile.openstreetmap.org/",
            "http://b.tile.openstreetmap.us/usgs_large_scale/",
            "http://tile.stamen.com/terrain-background/",
            "http://a.tile.stamen.com/toner/",
            "https://stamen-tiles.a.ssl.fastly.net/watercolor/"
        };

        public override void Create(Tile tile)
        {
            base.Create(tile);

            var url = "https://tile.mapzen.com/mapzen/terrain/v1/terrarium/" + tile.Zoom + "/" + tile.TileTms.x + "/" +
                      tile.TileTms.y + ".png?api_key=" + _key;
            
            ObservableWWW.GetWWW(url).Subscribe(
                    success =>
                    {
                        CreateMesh(tile, success);
                    },
                    error =>
                    {
                        Debug.Log(url + " - " + error);
                    });
        }

        private void CreateMesh(Tile tile, WWW terrarium)
        {
            var url = TileServiceUrls[(int)TileService] + tile.Zoom + "/" + tile.TileTms.x + "/" + tile.TileTms.y + ".png";
            var sampleCount = 3;
            var tex = new Texture2D(256, 256);
            terrarium.LoadImageIntoTexture(tex);

            ObservableWWW.GetWWW(url).Subscribe(
                success =>
                {
                    var go = new GameObject();
                    var mesh = go.AddComponent<MeshFilter>().mesh;
                    var rend = go.AddComponent<MeshRenderer>();
                    var verts = new List<Vector3>();

                    for (float x = 0; x < sampleCount; x++)
                    {
                        for (float y = 0; y < sampleCount; y++)
                        {
                            var xx = Mathf.Lerp((float)tile.Rect.Min.x, (float)(tile.Rect.Min.x + tile.Rect.Size.x), x / (sampleCount-1));
                            var yy = Mathf.Lerp((float)tile.Rect.Min.y, (float)(tile.Rect.Min.y + tile.Rect.Size.y), y / (sampleCount - 1));
                            
                            verts.Add(new Vector3(
                                (float)(xx - tile.Rect.Center.x), 
                                GetTerrariumHeight(tex.GetPixel((int) Mathf.Clamp((x/ (sampleCount - 1) * 256),0,255), (int)Mathf.Clamp((256 - (y/ (sampleCount - 1) * 256)),0,255))), 
                                (float)(yy - tile.Rect.Center.y)));
                        }
                    }

                    mesh.SetVertices(verts);
                    mesh.triangles = new int[]
                    {
                0,3,4,0,4,1,1,4,5,1,5,2,3,6,7,3,7,4,4,7,8,4,8,5
                    };
                    mesh.SetUVs(0, new List<Vector2>()
                    {
                        new Vector2(0, 1),
                        new Vector2(0, 0.5f),
                        new Vector2(0, 0),
                        new Vector2(0.5f, 1),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0),
                        new Vector2(1, 1),
                        new Vector2(1, 0.5f),
                        new Vector2(1, 0),
                    });
                    mesh.RecalculateNormals();
                    go.transform.SetParent(tile.transform, false);

                    rend.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT5, false);
                    rend.material.color = new Color(1f, 1f, 1f, 1f);
                    success.LoadImageIntoTexture((Texture2D)rend.material.mainTexture);
                },
                error =>
                {
                    Debug.Log(error);
                });
        }

        private float GetTerrariumHeight(Color c)
        {
            return (c.r*256*256 + c.g*256 + c.b) - 32768;
        }
    }
}
