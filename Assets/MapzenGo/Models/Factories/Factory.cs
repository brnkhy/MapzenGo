using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : MonoBehaviour
{
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
