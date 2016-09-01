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
    internal class Road : MonoBehaviour
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
            public RoadSettings Path;
            public RoadSettings MinorRoad;
            public RoadSettings MajorRoad;
            public RoadSettings Highway;

            public RoadSettings GetSettingsFor(RoadType type)
            {
                if (type == RoadType.Path)
                    return Path;
                if (type == RoadType.MajorRoad)
                    return MajorRoad;
                if (type == RoadType.MinorRoad)
                    return MinorRoad;
                if (type == RoadType.Highway)
                    return Highway;

                return Default;
            }
        }

        [Serializable]
        public class RoadSettings
        {
            public bool Enabled;
            public Material Material;
            public float Width;
        }
    }
}
