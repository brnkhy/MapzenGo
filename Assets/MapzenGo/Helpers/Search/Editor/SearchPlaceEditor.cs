using System;
using System.Collections.Generic;
using MapzenGo.Helpers.Search;
using MapzenGo.Models.Settings.Editor;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

//[CanEditMultipleObjects]
namespace MapzenGo.Helpers.Search.Editor
{
    [CustomEditor(typeof(SearchPlace))]
    public class SearchPlaceEditor : UnityEditor.Editor
    {
        const string PATH_SAVE_SCRIPTABLE_OBJECT = "Assets/MapzenGo/Resources/Settings/";
        public bool show = true;
        List<string> mKeys;
        private SearchPlace place;
    
        public Color m_Color;
        public string m_String;
        public int m_Number;

        AnimBool m_ShowExtraFields;
        //private string str;
        void OnEnable()
        {
            place = target as SearchPlace;
            place.DataStructure = HelperExtention.GetOrCreateSObjectReturn<StructSearchData>(ref place.DataStructure,PATH_SAVE_SCRIPTABLE_OBJECT);
            place.namePlaceСache = "";
         //   place.namePlace = "";
            /*if (place.namePlace!=String.Empty)
            {
                place.SearchInMapzen();
            }
            else
            {
                place.DataStructure.dataChache.Clear();
            }*/

        }

        private void ShowSaveLocation()
        {
            if (place.DataStructure.SaveSearch.Count > 0)
            {
                GUILayout.BeginHorizontal("Box");
                show = EditorGUILayout.Foldout(show, "SAVED LOCATION");
                GUILayout.EndHorizontal();
                if (show)
                {
                    GUILayout.BeginVertical("Box");
                    foreach (SearchData seachData in place.DataStructure.SaveSearch)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(seachData.label, "CN CountBadge"))
                        {
                            place.SetupToTileManager(seachData.coordinates[1],
                                seachData.coordinates[0]);
                        }
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            place.DataStructure.SaveSearch.Remove(seachData);
                            return;
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
            }
        }

        private void ShowFoundLocation()
        {
            for (int i = 0; i < place.DataStructure.dataChache.Count; i++)
            {
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.black;
                style.alignment = TextAnchor.MiddleLeft;
                style.fontSize = 14;
                style.padding = new RectOffset(10, 1, 1, 1);

                GUILayout.BeginHorizontal("Box");
                {
                    if (GUILayout.Button(place.DataStructure.dataChache[i].label, style))
                    {
                        place.SetupToTileManager(place.DataStructure.dataChache[i].coordinates[1],
                            place.DataStructure.dataChache[i].coordinates[0]);
                    }
                    if (GUILayout.Button("Save", "CN CountBadge", GUILayout.Width(50)))
                    {
                        place.DataStructure.SaveSearch.Add(place.DataStructure.dataChache[i]);
                        show = true;
                    }

                }
                GUILayout.EndHorizontal();
            }
            if (place.DataStructure.dataChache.Count == 0&& place.namePlace != String.Empty)
            {
                EditorGUILayout.HelpBox("Location is not found", MessageType.Warning);
            }
        }

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();
            place = target as SearchPlace;
            if (place != null)
            {

                GUILayout.BeginHorizontal("Box");
                GUILayout.Label("Enter location name",LayerSettingWindows.GuiTitleSize(14, TextAnchor.MiddleCenter, Color.black));
                place.namePlace = EditorGUILayout.TextField("", place.namePlace,GUILayout.Height(20));
                GUILayout.EndHorizontal();

               // Debug.Log(place.namePlace == String.Empty);
                if (place.namePlace == String.Empty)
                {
                    place.namePlaceСache = "";
                    place.DataStructure.dataChache.Clear();
                    EditorGUILayout.HelpBox("Enter place name", MessageType.Info);
                }
                else
                {
                    place.SearchInMapzen();
                }

                if (place.DataStructure.dataChache != null)
                {
                    EditorGUILayout.Separator();
                    ShowSaveLocation();
                    EditorGUILayout.Separator();
                    ShowFoundLocation();
                }
            }
            Repaint();
        }
    }
}
