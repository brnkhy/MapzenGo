using MapzenGo.Helpers;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MapzenGo.Models
{
    //float lat, float lon
    public class GPS_CDTM : CachedDynamicTileManager
    {
        private bool WeGotAMap = false;

        public override void Start()
        {
            CacheFolderPath = Path.Combine(Application.persistentDataPath, RelativeCachePath);
            CacheFolderPath = CacheFolderPath.Format(Zoom);
            if (!Directory.Exists(CacheFolderPath))
                Directory.CreateDirectory(CacheFolderPath);
        }

        public override void Update()
        {
            if (this.WeGotAMap)
                base.Update();
        }

        // Use this for initialization
        public void Begin()
        {
            //base.Latitude = lat;
            //base.Longitude = lon;
            Destroy(GameObject.Find("Tiles"));

            Latitude = float.Parse(GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text);
            Longitude = float.Parse(GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text);
            this.WeGotAMap = true;
            base.Start();
        }
    }
}