using System.Collections.Generic;
using Assets.MapzenGo.Models.Enums;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEditor;
using UnityEngine;

namespace MapzenGo.Models.Settings.Editor
{
    public class LayerSettingWindows : EditorWindow
    {

        const string PATH_SAVE_SCRIPTABLE_OBJECT = "Assets/MapzenGo/Resources/Settings/";
        private BuildingFactorySettings _factorySettingsBuildingFactory;
        private BuildingFactorySettings BuildingFactorySettings
        {
            get
            {
                return _factorySettingsBuildingFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsBuildingFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }
        private RoadFactorySettings _factorySettingsRoadFactory;
        private RoadFactorySettings RoadFactorySettings
        {
            get
            {
                return _factorySettingsRoadFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsRoadFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }
        private LanduseFactorySettings _factorySettingsLanduseFactory;
        private LanduseFactorySettings LanduseFactorySettings
        {
            get
            {
                return _factorySettingsLanduseFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsLanduseFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }
        private WaterFactorySettings _factorySettingsWaterFactory;
        private WaterFactorySettings WaterFactorySettings
        {
            get
            {
                return _factorySettingsWaterFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsWaterFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }

        private EarthFactorySettings _factorySettingsEarthFactory;
        private EarthFactorySettings EarthFactorySettings
        {
            get
            {
                return _factorySettingsEarthFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsEarthFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }

        private PlacesFactorySettings _factorySettingsPlacesFactory;
        private PlacesFactorySettings PlacesFactorySettings
        {
            get
            {
                return _factorySettingsPlacesFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsPlacesFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }

        private PoiFactorySettings _factorySettingsPoiFactory;
        private PoiFactorySettings PoiFactorySettings
        {
            get
            {
                return _factorySettingsPoiFactory ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _factorySettingsPoiFactory, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }

        private BoundaryFactorySettings _settingBoundary;
        private BoundaryFactorySettings BoundaryFactorySettings
        {
            get
            {
                return _settingBoundary ??
                       HelperExtention.GetOrCreateSObjectReturn(ref _settingBoundary, PATH_SAVE_SCRIPTABLE_OBJECT);
            }
        }

        private SettingsLayers settingElement;

        private int viewIndex = 1;
        private int tab = 0;
        private Vector2 scrollPos = Vector2.zero;

        public List<string> UsingType = new List<string>();

        Material currentObject = null;

        public static GUIStyle GuiTitleSize(int fontSize, TextAnchor anchor, Color color)
        {
            var style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;
            style.fontSize = fontSize;
            style.fixedHeight = 20;
            style.alignment = anchor;
            style.border = new RectOffset();
            return style;
        }

        [MenuItem("Window/MapzenGO/Setting Layer %#e")]
        static void Init()
        {
            var window = EditorWindow.GetWindow(typeof(LayerSettingWindows));
            window.minSize = new Vector2(500, 500);

            //Add name content
            var content = new GUIContent();
            content.text = "Setting Type";
            content.tooltip = "Setting Type";
            window.titleContent = content;
        }
        void OnEnable()
        {
            SetupSettings();
        }

        private void SetupSettings()
        {
            UsingType = new List<string>();
            UsingType.Add("bt_" + BuildingFactorySettings.DefaultBuilding.Type.ToString());
            BuildingFactorySettings.SettingsBuildings.ForEach(settings =>
            {
                UsingType.Add("bt_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("rt_" + RoadFactorySettings.DefaultRoad.Type.ToString());
            RoadFactorySettings.SettingsRoad.ForEach(settings =>
            {
                UsingType.Add("rt_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("lt_" + LanduseFactorySettings.DefaultLanduse.Type.ToString());
            LanduseFactorySettings.SettingsLanduse.ForEach(settings =>
            {
                UsingType.Add("lt_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("wt_" + WaterFactorySettings.DefaultWater.Type.ToString());
            WaterFactorySettings.SettingsWater.ForEach(settings =>
            {
                UsingType.Add("wt_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("et_" + EarthFactorySettings.DefaultEarth.Type.ToString());
            EarthFactorySettings.SettingsEarth.ForEach(settings =>
            {
                UsingType.Add("et_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("bt_" + BoundaryFactorySettings.DefaultBoundary.Type.ToString());
            BoundaryFactorySettings.SettingsBoundary.ForEach(settings =>
            {
                UsingType.Add("bt_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("pt_" + PlacesFactorySettings.DefaultPlace.Type.ToString());
            PlacesFactorySettings.SettingsPlace.ForEach(settings =>
            {
                UsingType.Add("pt_" + settings.Type.ToString());
                settings.showContent = false;
            });

            UsingType.Add("pot_" + PoiFactorySettings.DefaultPoi.Type.ToString());
            PoiFactorySettings.SettingsPoi.ForEach(settings =>
            {
                UsingType.Add("pot_" + settings.Type.ToString());
                settings.showContent = false;
            });
        }


        void OnGUI()
        {
            //return;
            tab = GUILayout.Toolbar(tab, new string[] { "BUILDING", "ROAD", "LANDUSE", "WATER", "BOUNDARY", "EARTH", "PLACES", "POI" });
            switch (tab)
            {
                case 0:
                    {
                        //BUILDING
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT BUILDING TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowBuildingElement(BuildingFactorySettings.DefaultBuilding);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowExistListBuilding();
                        break;
                    }
                case 1:
                    {
                        //ROAD
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT ROAD TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowRoadElement(RoadFactorySettings.DefaultRoad);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowExistListRoad();
                        break;

                    }
                case 2:
                    {
                        //LANDUSE
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT ROAD TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowLanduseElement(LanduseFactorySettings.DefaultLanduse);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowExistListLanduse();
                        break;
                    }

                case 3:
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT ROAD TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowWaterElement(WaterFactorySettings.DefaultWater);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowWaterListLanduse();
                        break;
                    }
                case 4:
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT BOUNDARY TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowBoundaryElement(BoundaryFactorySettings.DefaultBoundary);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowBoundaryListLanduse();
                        break;
                    }
                case 5:
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT EARTH TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowEarthElement(EarthFactorySettings.DefaultEarth);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowEarthListLanduse();
                        break;
                    }
                case 6:
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT PLACE TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowPlaceElement(PlacesFactorySettings.DefaultPlace);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowPlaceListLanduse();
                        break;
                    }
                case 7:
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                        {
                            GUILayout.Label("DEFAULT POI TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                            ShowPoiElement(PoiFactorySettings.DefaultPoi);
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(20);
                        ShowPoiListLanduse();
                        break;
                    }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(BuildingFactorySettings);
                EditorUtility.SetDirty(RoadFactorySettings);
                EditorUtility.SetDirty(LanduseFactorySettings);
            }

        }

        private void ShowExistListBuilding()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("BUILDING TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD BUILDING TYPE"))
                {
                    BuildingFactorySettings.SettingsBuildings.Add(new BuildingSettings()
                    {
                        Type = BuildingType.Unknown,
                        Material = null
                    });
                    UsingType.Add("bt_" + BuildingType.Unknown.ToString());
                    GUILayout.FlexibleSpace();
                }

                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < BuildingFactorySettings.SettingsBuildings.Count; ind++)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        BuildingFactorySettings.SettingsBuildings[ind].showContent = EditorGUILayout.Foldout(BuildingFactorySettings.SettingsBuildings[ind].showContent,
                            "BUILDING TYPE - " + BuildingFactorySettings.SettingsBuildings[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;
                        if (UsingType.FindAll(s => s == "bt_" + BuildingFactorySettings.SettingsBuildings[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                BuildingFactorySettings.SettingsBuildings[ind].showContent = true;
                            }
                        }

                        if (BuildingFactorySettings.SettingsBuildings[ind].Material == null)
                        {
                            GUI.backgroundColor = Color.magenta;
                            if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                            {
                                BuildingFactorySettings.SettingsBuildings[ind].showContent = true;
                            }
                        }
                        GUI.backgroundColor = Color.white;
                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            BuildingFactorySettings.SettingsBuildings.Move(ind, ind - 1);
                        }
                        if (ind < BuildingFactorySettings.SettingsBuildings.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            BuildingFactorySettings.SettingsBuildings.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("bt_" + BuildingFactorySettings.SettingsBuildings[ind].Type.ToString()))
                                {
                                    UsingType.Remove("bt_" + BuildingFactorySettings.SettingsBuildings[ind].Type.ToString());
                                }
                                BuildingFactorySettings.SettingsBuildings.RemoveAt(ind);
                            }
                        }

                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        if (BuildingFactorySettings.SettingsBuildings.Count > ind && BuildingFactorySettings.SettingsBuildings[ind].showContent)
                        {
                            ShowBuildingElement(BuildingFactorySettings.SettingsBuildings[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowExistListRoad()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("ROAD TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD ROAD TYPE"))
                {
                    RoadFactorySettings.SettingsRoad.Add(new RoadSettings()
                    {
                        Type = RoadType.Path,
                        Material = null,
                        Width = 3
                    });
                    UsingType.Add("rt_" + RoadType.Path.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < RoadFactorySettings.SettingsRoad.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        RoadFactorySettings.SettingsRoad[ind].showContent =
                            EditorGUILayout.Foldout(RoadFactorySettings.SettingsRoad[ind].showContent,
                                "ROAD TYPE - " + RoadFactorySettings.SettingsRoad[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;
                        if (UsingType.FindAll(s => s == "rt_" + RoadFactorySettings.SettingsRoad[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                RoadFactorySettings.SettingsRoad[ind].showContent = true;
                            }
                        }

                        if (RoadFactorySettings.SettingsRoad[ind].Material == null)
                        {
                            GUI.backgroundColor = Color.magenta;
                            if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                            {
                                RoadFactorySettings.SettingsRoad[ind].showContent = true;
                            }
                        }
                        GUI.backgroundColor = Color.white;
                        #endregion

                        #region BUTTON MOVE & REMOVE 

                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            RoadFactorySettings.SettingsRoad.Move(ind, ind - 1);
                        }
                        if (ind < RoadFactorySettings.SettingsRoad.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < RoadFactorySettings.SettingsRoad.Count);
                            RoadFactorySettings.SettingsRoad.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("rt_" + RoadFactorySettings.SettingsRoad[ind].Type.ToString()))
                                {
                                    UsingType.Remove("rt_" +
                                                     RoadFactorySettings.SettingsRoad[ind].Type.ToString());
                                }
                                RoadFactorySettings.SettingsRoad.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();

                        if (RoadFactorySettings.SettingsRoad.Count > ind && RoadFactorySettings.SettingsRoad[ind].showContent) ShowRoadElement(RoadFactorySettings.SettingsRoad[ind]);

                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowExistListLanduse()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("LANDUSE TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD LANDUSE TYPE"))
                {
                    LanduseFactorySettings.SettingsLanduse.Add(new LanduseSettings()
                    {
                        Type = LanduseKind.Park,
                        Material = null,
                    });
                    UsingType.Add("lt_" + LanduseKind.Park.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < LanduseFactorySettings.SettingsLanduse.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        LanduseFactorySettings.SettingsLanduse[ind].showContent =
                            EditorGUILayout.Foldout(LanduseFactorySettings.SettingsLanduse[ind].showContent,
                                "LANDUSE TYPE - " + LanduseFactorySettings.SettingsLanduse[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;

                        if (UsingType.FindAll(s => s == "lt_" + LanduseFactorySettings.SettingsLanduse[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                LanduseFactorySettings.SettingsLanduse[ind].showContent = true;
                            }
                        }

                        if (LanduseFactorySettings.SettingsLanduse[ind].Material == null)
                        {
                            GUI.backgroundColor = Color.magenta;
                            if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                            {
                                LanduseFactorySettings.SettingsLanduse[ind].showContent = true;
                            }
                        }

                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        GUI.backgroundColor = Color.white;
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            LanduseFactorySettings.SettingsLanduse.Move(ind, ind - 1);
                        }
                        if (ind < LanduseFactorySettings.SettingsLanduse.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < LanduseFactorySettings.SettingsLanduse.Count);
                            LanduseFactorySettings.SettingsLanduse.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("lt_" + LanduseFactorySettings.SettingsLanduse[ind].Type.ToString()))
                                {
                                    UsingType.Remove("lt_" +
                                                     LanduseFactorySettings.SettingsLanduse[ind].Type.ToString());
                                }
                                LanduseFactorySettings.SettingsLanduse.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();


                        if (LanduseFactorySettings.SettingsLanduse.Count > ind && LanduseFactorySettings.SettingsLanduse[ind].showContent)
                        {
                            ShowLanduseElement(LanduseFactorySettings.SettingsLanduse[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowWaterListLanduse()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("WATER TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD WATER TYPE"))
                {
                    WaterFactorySettings.SettingsWater.Add(new WaterSettings()
                    {
                        Type = WaterType.Water,
                        Material = null,
                    });
                    UsingType.Add("wt_" + WaterType.Water.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < WaterFactorySettings.SettingsWater.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        WaterFactorySettings.SettingsWater[ind].showContent =
                            EditorGUILayout.Foldout(WaterFactorySettings.SettingsWater[ind].showContent,
                                "WATER TYPE - " + WaterFactorySettings.SettingsWater[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;

                        if (UsingType.FindAll(s => s == "wt_" + WaterFactorySettings.SettingsWater[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                WaterFactorySettings.SettingsWater[ind].showContent = true;
                            }
                        }

                        if (WaterFactorySettings.SettingsWater[ind].Material == null)
                        {
                            GUI.backgroundColor = Color.magenta;
                            if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                            {
                                WaterFactorySettings.SettingsWater[ind].showContent = true;
                            }
                        }
                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        GUI.backgroundColor = Color.white;
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            WaterFactorySettings.SettingsWater.Move(ind, ind - 1);
                        }
                        if (ind < WaterFactorySettings.SettingsWater.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < WaterFactorySettings.SettingsWater.Count);
                            WaterFactorySettings.SettingsWater.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("wt_" + WaterFactorySettings.SettingsWater[ind].Type.ToString()))
                                {
                                    UsingType.Remove("wt_" +
                                                     WaterFactorySettings.SettingsWater[ind].Type.ToString());
                                }
                                WaterFactorySettings.SettingsWater.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        if (WaterFactorySettings.SettingsWater.Count > ind && WaterFactorySettings.SettingsWater[ind].showContent)
                        {
                            ShowWaterElement(WaterFactorySettings.SettingsWater[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowEarthListLanduse()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("EARTH TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD EARTH TYPE"))
                {
                    EarthFactorySettings.SettingsEarth.Add(new EarthSettings()
                    {
                        Type = EarthType.Earth,
                        Material = null,
                    });
                    UsingType.Add("et_" + EarthType.Earth.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < EarthFactorySettings.SettingsEarth.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        EarthFactorySettings.SettingsEarth[ind].showContent =
                            EditorGUILayout.Foldout(EarthFactorySettings.SettingsEarth[ind].showContent,
                                "Earth TYPE - " + EarthFactorySettings.SettingsEarth[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;

                        if (UsingType.FindAll(s => s == "wt_" + EarthFactorySettings.SettingsEarth[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                EarthFactorySettings.SettingsEarth[ind].showContent = true;
                            }
                        }

                        if (EarthFactorySettings.SettingsEarth[ind].Material == null)
                        {
                            GUI.backgroundColor = Color.magenta;
                            if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                            {
                                EarthFactorySettings.SettingsEarth[ind].showContent = true;
                            }
                        }
                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        GUI.backgroundColor = Color.white;
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            EarthFactorySettings.SettingsEarth.Move(ind, ind - 1);
                        }
                        if (ind < EarthFactorySettings.SettingsEarth.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < EarthFactorySettings.SettingsEarth.Count);
                            EarthFactorySettings.SettingsEarth.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("et_" + EarthFactorySettings.SettingsEarth[ind].Type.ToString()))
                                {
                                    UsingType.Remove("et_" +
                                                     EarthFactorySettings.SettingsEarth[ind].Type.ToString());
                                }
                                EarthFactorySettings.SettingsEarth.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        if (EarthFactorySettings.SettingsEarth.Count > ind && EarthFactorySettings.SettingsEarth[ind].showContent)
                        {
                            ShowEarthElement(EarthFactorySettings.SettingsEarth[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowBoundaryListLanduse()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("Boundary TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD Boundary TYPE"))
                {
                    BoundaryFactorySettings.SettingsBoundary.Add(new BoundarySettings()
                    {
                        Type = BoundaryType.Unknown,
                        Material = null,
                    });
                    UsingType.Add("bt_" + BoundaryType.Unknown.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < BoundaryFactorySettings.SettingsBoundary.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        BoundaryFactorySettings.SettingsBoundary[ind].showContent =
                            EditorGUILayout.Foldout(BoundaryFactorySettings.SettingsBoundary[ind].showContent,
                                "Boundary TYPE - " + BoundaryFactorySettings.SettingsBoundary[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;

                        if (UsingType.FindAll(s => s == "bt_" + BoundaryFactorySettings.SettingsBoundary[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                BoundaryFactorySettings.SettingsBoundary[ind].showContent = true;
                            }
                        }

                        if (BoundaryFactorySettings.SettingsBoundary[ind].Material == null)
                        {
                            GUI.backgroundColor = Color.magenta;
                            if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                            {
                                BoundaryFactorySettings.SettingsBoundary[ind].showContent = true;
                            }
                        }
                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        GUI.backgroundColor = Color.white;
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            BoundaryFactorySettings.SettingsBoundary.Move(ind, ind - 1);
                        }
                        if (ind < BoundaryFactorySettings.SettingsBoundary.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < BoundaryFactorySettings.SettingsBoundary.Count);
                            BoundaryFactorySettings.SettingsBoundary.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("bt_" + BoundaryFactorySettings.SettingsBoundary[ind].Type.ToString()))
                                {
                                    UsingType.Remove("bt_" +
                                                     BoundaryFactorySettings.SettingsBoundary[ind].Type.ToString());
                                }
                                BoundaryFactorySettings.SettingsBoundary.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        if (BoundaryFactorySettings.SettingsBoundary.Count > ind && BoundaryFactorySettings.SettingsBoundary[ind].showContent)
                        {
                            ShowBoundaryElement(BoundaryFactorySettings.SettingsBoundary[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowPlaceListLanduse()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("Place TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD Place TYPE"))
                {
                    PlacesFactorySettings.SettingsPlace.Add(new PlaceSettings()
                    {
                        Type = PlaceType.Unknown,
                    });
                    UsingType.Add("pt_" + PlaceType.Unknown.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < PlacesFactorySettings.SettingsPlace.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        PlacesFactorySettings.SettingsPlace[ind].showContent =
                            EditorGUILayout.Foldout(PlacesFactorySettings.SettingsPlace[ind].showContent,
                                "Place TYPE - " + PlacesFactorySettings.SettingsPlace[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;

                        if (UsingType.FindAll(s => s == "pt_" + PlacesFactorySettings.SettingsPlace[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                PlacesFactorySettings.SettingsPlace[ind].showContent = true;
                            }
                        }
                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        GUI.backgroundColor = Color.white;
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            PlacesFactorySettings.SettingsPlace.Move(ind, ind - 1);
                        }
                        if (ind < PlacesFactorySettings.SettingsPlace.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < PlacesFactorySettings.SettingsPlace.Count);
                            PlacesFactorySettings.SettingsPlace.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("pt_" + PlacesFactorySettings.SettingsPlace[ind].Type.ToString()))
                                {
                                    UsingType.Remove("pt_" +
                                                     PlacesFactorySettings.SettingsPlace[ind].Type.ToString());
                                }
                                PlacesFactorySettings.SettingsPlace.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        if (PlacesFactorySettings.SettingsPlace.Count > ind && PlacesFactorySettings.SettingsPlace[ind].showContent)
                        {
                            ShowPlaceElement(PlacesFactorySettings.SettingsPlace[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowPoiListLanduse()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("TextArea");
            {
                GUILayout.Label("Poi TYPE FOR LAYER", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                if (GUILayout.Button("ADD POI TYPE"))
                {
                    PoiFactorySettings.SettingsPoi.Add(new PoiSettings()
                    {
                        Type = PoiType.Unknown,
                    });
                    UsingType.Add("pot_" + PoiType.Unknown.ToString());
                }
                GUILayout.Space(10);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    for (int ind = 0; ind < PoiFactorySettings.SettingsPoi.Count; ind++)
                    {

                        EditorGUILayout.BeginHorizontal("box");
                        PoiFactorySettings.SettingsPoi[ind].showContent =
                            EditorGUILayout.Foldout(PoiFactorySettings.SettingsPoi[ind].showContent,
                                "POI TYPE - " + PoiFactorySettings.SettingsPoi[ind].Type.ToString());

                        #region CHECK DUBLE TYPE END ERROR
                        GUI.backgroundColor = Color.red;

                        if (UsingType.FindAll(s => s == "pot_" + PoiFactorySettings.SettingsPoi[ind].Type.ToString()).Count > 1)
                        {
                            if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                            {
                                PoiFactorySettings.SettingsPoi[ind].showContent = true;
                            }
                        }
                        
                        #endregion

                        #region BUTTON MOVE & REMOVE 
                        GUI.backgroundColor = Color.white;
                        if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind > 0);
                            PoiFactorySettings.SettingsPoi.Move(ind, ind - 1);
                        }
                        if (ind < PoiFactorySettings.SettingsPoi.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                        {
                            Debug.Log(ind < PoiFactorySettings.SettingsPoi.Count);
                            PoiFactorySettings.SettingsPoi.Move(ind, ind + 1);
                        }
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                            {
                                if (UsingType.Contains("pot_" + PoiFactorySettings.SettingsPoi[ind].Type.ToString()))
                                {
                                    UsingType.Remove("pot_" +
                                                     PoiFactorySettings.SettingsPoi[ind].Type.ToString());
                                }
                                PoiFactorySettings.SettingsPoi.RemoveAt(ind);
                            }
                        }
                        GUI.contentColor = Color.white;
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        if (PoiFactorySettings.SettingsPoi.Count > ind && PoiFactorySettings.SettingsPoi[ind].showContent)
                        {
                            ShowPoiElement(PoiFactorySettings.SettingsPoi[ind]);
                        }
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        private void ShowBuildingElement(BuildingSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                BuildingType saveKind = element.Type;
                element.Type = (BuildingType)EditorGUILayout.EnumPopup("Type Building:", element.Type);
                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("bt_" + saveKind.ToString())) UsingType.Remove("bt_" + saveKind.ToString());
                    UsingType.Add("bt_" + element.Type.ToString());
                }

                element.Material = (Material)EditorGUILayout.ObjectField("Material", element.Material, typeof(Material));

                if (element.Material == null) DisplayErrorMEssage("Not setting material");


                element.IsVolumetric = EditorGUILayout.Toggle("IsVolumetric", element.IsVolumetric);

                EditorGUILayout.LabelField("BuildingHeight");
                EditorGUILayout.BeginHorizontal("box");
                element.MaximumBuildingHeight = EditorGUILayout.IntField("Maximum", element.MaximumBuildingHeight);
                element.MinimumBuildingHeight = EditorGUILayout.IntField("Minimum", element.MinimumBuildingHeight);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowRoadElement(RoadSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                RoadType saveKind = element.Type;
                element.Type = (RoadType)EditorGUILayout.EnumPopup("Type Road:", element.Type);

                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("rt_" + saveKind.ToString())) UsingType.Remove("rt_" + saveKind.ToString());
                    UsingType.Add("rt_" + element.Type.ToString());
                }

                if (element.Type == RoadType.Rail)
                {
                    element.TypeRail = (RailwayType)EditorGUILayout.EnumPopup("Type Rail:", element.TypeRail);
                }
                element.Material = (Material)EditorGUILayout.ObjectField("Material", element.Material, typeof(Material));

                if (element.Material == null) DisplayErrorMEssage("Not setting material");
                element.Width = EditorGUILayout.FloatField("Road Width", element.Width);
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowLanduseElement(LanduseSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                LanduseKind saveKind = element.Type;
                element.Type = (LanduseKind)EditorGUILayout.EnumPopup("Type Landuse:", element.Type);

                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("lt_" + saveKind.ToString())) UsingType.Remove("lt_" + saveKind.ToString());
                    UsingType.Add("lt_" + element.Type.ToString());
                }

                element.Material = (Material)EditorGUILayout.ObjectField("Material", element.Material, typeof(Material));

                if (element.Material == null) DisplayErrorMEssage("Not setting material");
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowWaterElement(WaterSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                WaterType saveKind = element.Type;
                element.Type = (WaterType)EditorGUILayout.EnumPopup("Type Landuse:", element.Type);
                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("wt_" + saveKind.ToString())) UsingType.Remove("wt_" + saveKind.ToString());
                    UsingType.Add("wt_" + element.Type.ToString());
                }

                element.Material = (Material)EditorGUILayout.ObjectField("Material", element.Material, typeof(Material));

                if (element.Material == null) DisplayErrorMEssage("Not setting material");
            }
            EditorGUILayout.EndVertical();
        }

        private void ShowEarthElement(EarthSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                EarthType saveKind = element.Type;
                element.Type = (EarthType)EditorGUILayout.EnumPopup("Type Earth:", element.Type);
                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("et_" + saveKind.ToString())) UsingType.Remove("et_" + saveKind.ToString());
                    UsingType.Add("et_" + element.Type.ToString());
                }

                element.Material = (Material)EditorGUILayout.ObjectField("Material", element.Material, typeof(Material));

                if (element.Material == null) DisplayErrorMEssage("Not setting material");
            }
            EditorGUILayout.EndVertical();
        }

        private void ShowPlaceElement(PlaceSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                PlaceType saveKind = element.Type;
                element.Type = (PlaceType)EditorGUILayout.EnumPopup("Type Place:", element.Type);
                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("pt_" + saveKind.ToString())) UsingType.Remove("pt_" + saveKind.ToString());
                    UsingType.Add("pt_" + element.Type.ToString());
                }

                element.Color = EditorGUILayout.ColorField("Color", element.Color);
                element.FontSize = EditorGUILayout.IntField("Font Size", element.FontSize);
                element.OutlineColor = EditorGUILayout.ColorField("Outline Color", element.OutlineColor);
                element.Font = (Font)EditorGUILayout.ObjectField(element.Font, typeof(Font), allowSceneObjects: true);
            }
            EditorGUILayout.EndVertical();
        }
        private void ShowPoiElement(PoiSettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                PoiType saveKind = element.Type;
                element.Type = (PoiType)EditorGUILayout.EnumPopup("Type Poi:", element.Type);
                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("pot_" + saveKind.ToString())) UsingType.Remove("pot_" + saveKind.ToString());
                    UsingType.Add("pot_" + element.Type.ToString());
                }

                element.Sprite = (Sprite)EditorGUILayout.ObjectField(element.Sprite, typeof(Sprite), allowSceneObjects: true);
            }
            EditorGUILayout.EndVertical();
        }

        private void ShowBoundaryElement(BoundarySettings element)
        {
            EditorGUILayout.BeginVertical("box");
            {
                BoundaryType saveKind = element.Type;
                element.Type = (BoundaryType)EditorGUILayout.EnumPopup("Type Boundary:", element.Type);
                if (GUI.changed && saveKind != element.Type)
                {
                    if (UsingType.Contains("bt_" + saveKind.ToString())) UsingType.Remove("bt_" + saveKind.ToString());
                    UsingType.Add("bt_" + element.Type.ToString());
                }

                element.Material = (Material)EditorGUILayout.ObjectField("Material", element.Material, typeof(Material));

                if (element.Material == null) DisplayErrorMEssage("Not setting material");
                element.Width = EditorGUILayout.FloatField("Boundary Width", element.Width);
            }
            EditorGUILayout.EndVertical();
        }

        /* void CreateNewItemList()
    {
        viewIndex = 1;
        settingElement = Create();
        if (settingElement)
        {
            BuildingFactorySettings.SettingsBuildings = new List<BuildingSettings>();
            LanduseFactorySettings.SettingsLanduse = new List<LanduseSettings>();
            RoadFactorySettings.SettingsRoad = new List<RoadSettings>();
            WaterFactorySettings.SettingsWater = new List<WaterSettings>();
            BoundaryFactorySettings.SettingsBoundary = new List<BoundarySettings>();

            string relPath = AssetDatabase.GetAssetPath(settingElement);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
    public SettingsLayersLayers Create()
    {
        SettingsLayersLayers asset = ScriptableObject.CreateInstance<SettingsLayersLayers>();

        AssetDatabase.CreateAsset(asset, PATH_SAVE_SCRIPTABLE_OBJECT);
        AssetDatabase.SaveAssets();
        return asset;
    }*/

        public void DisplayErrorMEssage(string message, MessageType type = MessageType.Error)
        {
            EditorGUILayout.HelpBox(message, type);
        }
    }

    static class Ext
    {
        public static void Move<T>(this List<T> list, int i, int j)
        {
            var elem = list[i];
            list.RemoveAt(i);
            list.Insert(j, elem);
        }

        public static void Swap<T>(this List<T> list, int i, int j)
        {
            var elem1 = list[i];
            var elem2 = list[j];

            list[i] = elem2;
            list[j] = elem1;
        }
    }
}