using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Assets.Helpers;
using Assets.Models.Factories;
using UniRx;
using UnityEngine;

namespace Assets.Models
{
    public class DynamicTileManager : TileManager
    {
        private Vector2 _tileSize;
        [SerializeField] private Rect _centerCollider;
        [SerializeField] private Transform _player;
        [SerializeField] private int _removeAfter;

        public override void Init(BuildingFactory buildingFactory, RoadFactory roadFactory, World.Settings settings)
        {
            base.Init(buildingFactory, roadFactory, settings);
            _removeAfter = Math.Max(_removeAfter, Range*2 + 1);
            _tileSize = Tiles.Values.First().Rect.size;
            _centerCollider = new Rect(Vector2.zero - Tiles.Values.First().Rect.size/2, Tiles.Values.First().Rect.size);

            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x => { UpdateTiles(); });
        }

        private void UpdateTiles()
        {
            if (!_centerCollider.Contains(_player.transform.position.ToVector2xz(), true))
            {
                //player movement in TMS tiles
                var tileDif = GetMovementVector();
                //move locals
                Centralize(tileDif);
                //create new tiles
                LoadTiles(CenterTms, CenterInMercator);
                UnloadTiles(CenterTms);
            }
        }

        private void Centralize(Vector2 tileDif)
        {
            //move everything to keep current tile at 0,0
            foreach (var tile in Tiles.Values)
            {
                tile.transform.position -= new Vector3(tileDif.x * _tileSize.x, 0, tileDif.y * _tileSize.y);
            }
            CenterTms += tileDif;
            CenterInMercator = GM.TileBounds(CenterTms, Zoom).center;
            var difInUnity = new Vector3(tileDif.x*_tileSize.x, 0, tileDif.y*_tileSize.y);
            _player.position -= difInUnity;
            Camera.main.transform.position -= difInUnity;
        }
        
        private void UnloadTiles(Vector2 currentTms)
        {
            var rem = new List<Vector2>();
            foreach (var key in Tiles.Keys.Where(x => x.ManhattanTo(currentTms) > _removeAfter))
            {
                rem.Add(key);
                Destroy(Tiles[key].gameObject);
            }
            foreach (var v in rem)
            {
                Tiles.Remove(v);
            }
        }

        private Vector2 GetMovementVector()
        {
            var dif = _player.transform.position.ToVector2xz();
            var tileDif = Vector2.zero;
            if (dif.x < Math.Min(_centerCollider.xMin, _centerCollider.xMax))
                tileDif.x = -1;
            else if (dif.x > Math.Max(_centerCollider.xMin, _centerCollider.xMax))
                tileDif.x = 1;

            if (dif.y < Math.Min(_centerCollider.yMin, _centerCollider.yMax))
                tileDif.y = 1;
            else if (dif.y > Math.Max(_centerCollider.yMin, _centerCollider.yMax))
                tileDif.y = -1; //invert axis  TMS vs unity
            return tileDif;
        }

        public void Update()
        {
            Debug.DrawLine(_centerCollider.min.ToVector3xz(), _centerCollider.min.ToVector3xz() + new Vector3(0,100,0), Color.red);
            Debug.DrawLine(_centerCollider.max.ToVector3xz(), _centerCollider.max.ToVector3xz() + new Vector3(0, 100, 0), Color.red);
        }
    }
}
