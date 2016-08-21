using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Water : MonoBehaviour
    {
        public string Id;
        public string Type;
        public string Kind;
        public string Name;

        public void Init()
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Water");
        }

        [Serializable]
        public class Settings
        {

        }
    }
}
