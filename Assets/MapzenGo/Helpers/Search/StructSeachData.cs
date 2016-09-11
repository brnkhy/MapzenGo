using System;
using UnityEngine;
using System.Collections;
using MapzenGo.Helpers;
using UniRx;

[Serializable]
public class StructSeachData
{
    private string url = "http://b.tile.openstreetmap.org/";
    public Vector2 coordinates;
    
    public string label;
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


