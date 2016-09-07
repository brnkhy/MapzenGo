using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapzenGo.Models.Factories
{
    public class Factory : MonoBehaviour
    {
        public bool MergeMeshes;
        public float Order = 1;
        public virtual string XmlTag {get { return ""; } }
        public virtual Func<JSONObject, bool> Query { get; set; }

        public virtual void Start()
        {
            Query = (geo) => true;
        }

        public virtual IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
        {
            return null;
        }

        public virtual GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> toList)
        {
            return null;
        }
    }
}
