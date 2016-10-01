using System.Collections.Generic;
using System.Linq;
using MapzenGo.Helpers;
using MapzenGo.Models.Factories;
using MapzenGo.Models.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace MapzenGo.Models
{
    public class PlacesFactory : Factory
    {
        [SerializeField] private GameObject _labelPrefab;
        [SerializeField] private GameObject _container;
        public override string XmlTag { get { return "places"; } }
        [SerializeField]
        protected PlacesFactorySettings FactorySettings;
        public override void Start()
        {
            base.Start();
            Query = (geo) => geo["geometry"]["type"].str == "Point";
        }

        public override void Create(Tile tile)
        {
            if (!(tile.Data.HasField(XmlTag) && tile.Data[XmlTag].HasField("features")))
                return;


            var ql = tile.Data[XmlTag]["features"].list.Where(x => Query(x));
            foreach (var entity in ql.SelectMany(geo => Create(tile, geo)))
            {
                if (entity != null)
                {
                    entity.transform.SetParent(_container.transform, true);
                }
            }
        }

        protected override IEnumerable<MonoBehaviour> Create(Tile tile, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToPlaceType();
            if (!FactorySettings.HasSettingsFor(kind))
                yield break;

            var typeSettings = FactorySettings.GetSettingsFor<PlaceSettings>(kind);

            var go = Instantiate(_labelPrefab);
            var place = go.AddComponent<Place>();
            go.GetComponentInChildren<Outline>().effectColor = typeSettings.OutlineColor;
            var text = go.GetComponentInChildren<Text>();
            text.fontSize = typeSettings.FontSize;
            text.color = typeSettings.Color;
            text.font = typeSettings.Font;

            place.transform.SetParent(_container.transform, true);

            if (geo["properties"].HasField("name"))
                place.GetComponentInChildren<Text>().text = geo["properties"]["name"].str;

            var c = geo["geometry"]["coordinates"];
            var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
            var localMercPos = dotMerc - tile.Rect.Center;
            go.transform.position = new Vector3((float)localMercPos.x, (float)localMercPos.y);

            var target = new GameObject("placeTarget");
            target.transform.position = localMercPos.ToVector3();
            target.transform.SetParent(tile.transform, false);
            place.Stick(target.transform);

            SetProperties(geo, place, typeSettings);

            yield return place;
        }

        private static void SetProperties(JSONObject geo, Place place, PlaceSettings typeSettings)
        {
            place.Id = geo["properties"]["id"].ToString();
            if (geo["properties"].HasField("name"))
                place.Name = geo["properties"]["name"].str;
            place.Type = geo["type"].str;
            place.Kind = geo["properties"]["kind"].str;
            place.name = "place";
        }
    }
}
