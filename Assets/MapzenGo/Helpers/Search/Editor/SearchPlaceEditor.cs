using System;
using System.Collections.Generic;
using MapzenGo.Helpers.Search;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

//[CanEditMultipleObjects]
namespace MapzenGo.Helpers.Search.Editor
{
    [CustomEditor(typeof(SearchPlace))]
    public class SearchPlaceEditor : UnityEditor.Editor
    {
        List<string> mKeys;
        private SearchPlace place;
    
        public Color m_Color;
        public string m_String;
        public int m_Number;

        AnimBool m_ShowExtraFields;
        //private string str;
        void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();
            place = target as SearchPlace;

            if (place != null)
            {
                place.namePlace = EditorGUILayout.TextField("NamePlace", place.namePlace);
            
                if (place.namePlace == String.Empty)
                {
                    place.namePlaceСache = "";
                    place.dataList.Clear(); 
                    EditorGUILayout.HelpBox("Enter place name", MessageType.Info);
                }
                else
                {
                    place.SearchInMapzen();
                }

                int fl = 0;
          
                if(place.dataList!=null) { for (int i = 0; i < place.dataList.Count; i++)
                {
                    var style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.black;
                    style.alignment = TextAnchor.MiddleLeft;
                    style.fixedWidth = Screen.width - 40;
                    if (GUILayout.Button(place.dataList[i].label,style, GUILayout.Height(20)))
                    {
                        place.SetupToTileManager(place.dataList[i].coordinates[1], place.dataList[i].coordinates[0]);
                    }
                }}
         
            }

            Repaint();
        }
    }
}
