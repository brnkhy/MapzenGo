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

        public void Initialize(JSONObject geo, List<Vector3> list)
        {
            //GetComponent<MeshRenderer>().material = Resources.Load<Material>("Road");
            try
            {
                
                //road.Type = geo["properties"]["kind"].str;
                transform.localScale = Vector3.one;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}
