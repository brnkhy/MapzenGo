using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Building : MonoBehaviour
    {
        public string Id;
        public string Type;
        public string Kind;
        public string Name;

        private string _landuseKind;
        public string LanduseKind
        {
            get { return _landuseKind; }
            set
            {
                _landuseKind = value;
                GetComponent<MeshRenderer>().material = Resources.Load<Material>(LanduseKind);
            }
        }

        public int SortKey;
        
        [Serializable]
        public class Settings
        {
            public int MinimumBuildingHeight = 2;
            public int MaximumBuildingHeight = 5;
            public bool IsVolumetric = true;
        }
    }

}
