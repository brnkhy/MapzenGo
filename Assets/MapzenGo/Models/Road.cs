using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets
{
    public enum RoadType
    {
        Path,
        Rail,
        MinorRoad,
        MajorRoad,
        Highway,
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Road : MonoBehaviour
    {
        public string Id;
        public string Kind;
        public string Type;
        public string Name;

        public void Initialize(JSONObject geo, List<Vector3> list, Settings settings)
        {
            //GetComponent<MeshRenderer>().material = Resources.Load<Material>("Road");
            try
            {
                Id = geo["properties"]["id"].ToString();
                Type = geo["type"].str;
                Kind = geo["properties"]["kind"].str;
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
        }

        [Serializable]
        public class RoadSettings
        {
            public RoadType Type;
            public bool Enabled = true;
            public Material Material;
            public float Width = 6;
        }
    }
}
