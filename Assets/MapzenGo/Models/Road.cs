using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Road : MonoBehaviour
    {
        public string Id;
        public string Kind;
        public string Type;
        public string Name;
        public int SortKey;

        public void Initialize(JSONObject geo, List<Vector3> list, Settings settings)
        {
            //GetComponent<MeshRenderer>().material = Resources.Load<Material>("Road");
            try
            {
                Id = geo["properties"]["id"].ToString();
                Type = geo["type"].str;
                Kind = geo["properties"]["kind"].str;
                SortKey = (int)geo["properties"]["sort_key"].f;
                if (geo["properties"].HasField("name"))
                    Name = geo["properties"]["name"].str;
                //road.Type = geo["properties"]["kind"].str;
                transform.localScale = Vector3.one;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        
        [Serializable]
        public class Settings
        {
            public RoadSettings Default;
            public List<RoadSettings> AllSettings;

            public RoadSettings GetSettingsFor(RoadType type)
            {
                var f = AllSettings.FirstOrDefault(x => x.Type == type);
                return f ?? Default;
            }

            public bool HasSettingsFor(RoadType type)
            {
                return AllSettings.Any(x => x.Type == type);
            }
        }

        [Serializable]
        public class RoadSettings
        {
            public RoadType Type;
            public Material Material;
            public float Width = 6;
        }
    }
}
