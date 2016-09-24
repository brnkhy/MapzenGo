using System;
using System.Collections;
using System.Collections.Generic;
using MapzenGo.Helpers;
using UniRx;
using UnityEngine;

namespace MapzenGo.Helpers.Search
{ //[CreateAssetMenu(fileName = "StructSearchData",menuName = "SearchData")]
    public class StructSearchData:ScriptableObject
    {
        public List<SearchData> dataChache;
        public List<SearchData> SaveSearch;

        public StructSearchData()
        {
            dataChache = new List<SearchData>();
            SaveSearch = new List<SearchData>();
        }
        /*public Texture2D texture;

    void LoadPreview()
    {
        var v2 = GM.LatLonToMeters(coordinates.y, coordinates.x);
        var tile = GM.MetersToTile(v2, 13);
        var url2 = url + 13 + "/" + tile.x + "/" + tile.y + ".png";
      //  Debug.Log(url2);
        texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
        ObservableWWW.GetWWW(url2).Subscribe(
            success =>
            {
                success.LoadImageIntoTexture((Texture2D) texture);
            },
            error =>
            {
                Debug.Log(error);
            });

    }*/
    }

    [Serializable]
    public class SearchData
    {
        private string url = "http://b.tile.openstreetmap.org/";
        public Vector2 coordinates;
        public string label;
    }
}