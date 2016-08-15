using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : MonoBehaviour
{
    public virtual IEnumerable<MonoBehaviour> Create(Vector2 tileMercPos, JSONObject geo)
    {
        return null;
    }
}
