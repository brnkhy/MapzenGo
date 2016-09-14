using UnityEngine;
using System.Collections.Generic;
using Assets.MapzenGo.Models.Enums;
using MapzenGo.Models.Enums;
using UnityEditor;

public class LayerSettingWindows : EditorWindow
{

    const string PATH_SAVE_SCRIPTABLE_OBJECT = "Assets/MapzenGo/Resources/Settings/LayerSettings.asset";
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
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            settingElement = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SettingsLayers)) as SettingsLayers;
        }
        if (settingElement == null)
        {
            Debug.Log("CREATE ASSET");
            CreateNewItemList();
        }
        SetupSettings();
    }

    private void SetupSettings()
    {
        UsingType = new List<string>();
        UsingType.Add("bt_" + settingElement.DefaultBuilding.Type.ToString());
        settingElement.SettingsBuildings.ForEach(settings =>
        {
            UsingType.Add("bt_" + settings.Type.ToString());
            settings.showContent = false;
        });

        UsingType.Add("rt_" + settingElement.DefaultRoad.Type.ToString());
        settingElement.SettingsRoad.ForEach(settings =>
        {
            UsingType.Add("rt_" + settings.Type.ToString());
            settings.showContent = false;
        });

        UsingType.Add("lt_" + settingElement.DefaultLanduse.Type.ToString());
        settingElement.SettingsLanduse.ForEach(settings =>
        {
            UsingType.Add("lt_" + settings.Type.ToString());
            settings.showContent = false;
        });

        UsingType.Add("wt_" + settingElement.DefaultWater.Type.ToString());
        settingElement.SettingsWater.ForEach(settings =>
        {
            UsingType.Add("wt_" + settings.Type.ToString());
            settings.showContent = false;
        });

        UsingType.Add("bt_" + settingElement.DefaultBoundary.Type.ToString());
        settingElement.SettingsBoundary.ForEach(settings =>
        {
            UsingType.Add("bt_" + settings.Type.ToString());
            settings.showContent = false;
        });
    }


    void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new string[] { "BUILDING", "ROAD", "LANDUSE", "WATER", "BOUNDARY" });
        switch (tab)
        {
            case 0:
                {
                    //BUILDING
                    EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                    {
                        GUILayout.Label("DEFAULT BUILDING TYPE", GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
                        ShowBuildingElement(settingElement.DefaultBuilding);
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
                        ShowRoadElement(settingElement.DefaultRoad);
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
                        ShowLanduseElement(settingElement.DefaultLanduse);
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
                        ShowWaterElement(settingElement.DefaultWater);
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
                        ShowBoundaryElement(settingElement.DefaultBoundary);
                    }
                    EditorGUILayout.EndVertical();
                    GUILayout.Space(20);
                    ShowBoundaryListLanduse();
                    break;
                }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(settingElement);
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
                settingElement.SettingsBuildings.Add(new SettingsLayers.BuildingSettings()
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
                for (int ind = 0; ind < settingElement.SettingsBuildings.Count; ind++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    settingElement.SettingsBuildings[ind].showContent = EditorGUILayout.Foldout(settingElement.SettingsBuildings[ind].showContent,
                        "BUILDING TYPE - " + settingElement.SettingsBuildings[ind].Type.ToString());

                    #region CHECK DUBLE TYPE END ERROR
                    GUI.backgroundColor = Color.red;
                    if (UsingType.FindAll(s => s == "bt_" + settingElement.SettingsBuildings[ind].Type.ToString()).Count > 1)
                    {
                        if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                        {
                            settingElement.SettingsBuildings[ind].showContent = true;
                        }
                    }

                    if (settingElement.SettingsBuildings[ind].Material == null)
                    {
                        GUI.backgroundColor = Color.magenta;
                        if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                        {
                            settingElement.SettingsBuildings[ind].showContent = true;
                        }
                    }
                    GUI.backgroundColor = Color.white;
                    #endregion

                    #region BUTTON MOVE & REMOVE 
                    if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        settingElement.SettingsBuildings.Move(ind, ind - 1);
                    }
                    if (ind < settingElement.SettingsBuildings.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        settingElement.SettingsBuildings.Move(ind, ind + 1);
                    }
                    GUI.contentColor = Color.red;
                    if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                        {
                            if (UsingType.Contains("bt_" + settingElement.SettingsBuildings[ind].Type.ToString()))
                            {
                                UsingType.Remove("bt_" + settingElement.SettingsBuildings[ind].Type.ToString());
                            }
                            settingElement.SettingsBuildings.RemoveAt(ind);
                        }
                    }

                    GUI.contentColor = Color.white;
                    #endregion

                    EditorGUILayout.EndHorizontal();
                    if (settingElement.SettingsBuildings.Count > ind && settingElement.SettingsBuildings[ind].showContent)
                    {
                        ShowBuildingElement(settingElement.SettingsBuildings[ind]);
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
                settingElement.SettingsRoad.Add(new SettingsLayers.RoadSettings()
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
                for (int ind = 0; ind < settingElement.SettingsRoad.Count; ind++)
                {

                    EditorGUILayout.BeginHorizontal("box");
                    settingElement.SettingsRoad[ind].showContent =
                        EditorGUILayout.Foldout(settingElement.SettingsRoad[ind].showContent,
                            "ROAD TYPE - " + settingElement.SettingsRoad[ind].Type.ToString());

                    #region CHECK DUBLE TYPE END ERROR
                    GUI.backgroundColor = Color.red;
                    if (UsingType.FindAll(s => s == "rt_" + settingElement.SettingsRoad[ind].Type.ToString()).Count > 1)
                    {
                        if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                        {
                            settingElement.SettingsRoad[ind].showContent = true;
                        }
                    }

                    if (settingElement.SettingsRoad[ind].Material == null)
                    {
                        GUI.backgroundColor = Color.magenta;
                        if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                        {
                            settingElement.SettingsRoad[ind].showContent = true;
                        }
                    }
                    GUI.backgroundColor = Color.white;
                    #endregion

                    #region BUTTON MOVE & REMOVE 

                    if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind > 0);
                        settingElement.SettingsRoad.Move(ind, ind - 1);
                    }
                    if (ind < settingElement.SettingsRoad.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind < settingElement.SettingsRoad.Count);
                        settingElement.SettingsRoad.Move(ind, ind + 1);
                    }
                    GUI.contentColor = Color.red;
                    if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                        {
                            if (UsingType.Contains("rt_" + settingElement.SettingsRoad[ind].Type.ToString()))
                            {
                                UsingType.Remove("rt_" +
                                                                settingElement.SettingsRoad[ind].Type.ToString());
                            }
                            settingElement.SettingsRoad.RemoveAt(ind);
                        }
                    }
                    GUI.contentColor = Color.white;
                    #endregion

                    EditorGUILayout.EndHorizontal();

                    if (settingElement.SettingsRoad.Count > ind && settingElement.SettingsRoad[ind].showContent) ShowRoadElement(settingElement.SettingsRoad[ind]);

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
                settingElement.SettingsLanduse.Add(new SettingsLayers.LanduseSettings()
                {
                    Type = LanduseKind.Park,
                    Material = null,
                });
                UsingType.Add("lt_" + LanduseKind.Park.ToString());
            }
            GUILayout.Space(10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                for (int ind = 0; ind < settingElement.SettingsLanduse.Count; ind++)
                {

                    EditorGUILayout.BeginHorizontal("box");
                    settingElement.SettingsLanduse[ind].showContent =
                        EditorGUILayout.Foldout(settingElement.SettingsLanduse[ind].showContent,
                            "LANDUSE TYPE - " + settingElement.SettingsLanduse[ind].Type.ToString());

                    #region CHECK DUBLE TYPE END ERROR
                    GUI.backgroundColor = Color.red;

                    if (UsingType.FindAll(s => s == "lt_" + settingElement.SettingsLanduse[ind].Type.ToString()).Count > 1)
                    {
                        if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                        {
                            settingElement.SettingsLanduse[ind].showContent = true;
                        }
                    }

                    if (settingElement.SettingsLanduse[ind].Material == null)
                    {
                        GUI.backgroundColor = Color.magenta;
                        if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                        {
                            settingElement.SettingsLanduse[ind].showContent = true;
                        }
                    }

                    #endregion

                    #region BUTTON MOVE & REMOVE 
                    GUI.backgroundColor = Color.white;
                    if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind > 0);
                        settingElement.SettingsLanduse.Move(ind, ind - 1);
                    }
                    if (ind < settingElement.SettingsLanduse.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind < settingElement.SettingsLanduse.Count);
                        settingElement.SettingsLanduse.Move(ind, ind + 1);
                    }
                    GUI.contentColor = Color.red;
                    if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                        {
                            if (UsingType.Contains("lt_" + settingElement.SettingsLanduse[ind].Type.ToString()))
                            {
                                UsingType.Remove("lt_" +
                                                                settingElement.SettingsLanduse[ind].Type.ToString());
                            }
                            settingElement.SettingsLanduse.RemoveAt(ind);
                        }
                    }
                    GUI.contentColor = Color.white;
                    #endregion

                    EditorGUILayout.EndHorizontal();


                    if (settingElement.SettingsLanduse.Count > ind && settingElement.SettingsLanduse[ind].showContent)
                    {
                        ShowLanduseElement(settingElement.SettingsLanduse[ind]);
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
                settingElement.SettingsWater.Add(new SettingsLayers.WaterSettings()
                {
                    Type = WaterType.Water,
                    Material = null,
                });
                UsingType.Add("wt_" + WaterType.Water.ToString());
            }
            GUILayout.Space(10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                for (int ind = 0; ind < settingElement.SettingsWater.Count; ind++)
                {

                    EditorGUILayout.BeginHorizontal("box");
                    settingElement.SettingsWater[ind].showContent =
                        EditorGUILayout.Foldout(settingElement.SettingsWater[ind].showContent,
                            "WATER TYPE - " + settingElement.SettingsWater[ind].Type.ToString());

                    #region CHECK DUBLE TYPE END ERROR
                    GUI.backgroundColor = Color.red;

                    if (UsingType.FindAll(s => s == "wt_" + settingElement.SettingsWater[ind].Type.ToString()).Count > 1)
                    {
                        if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                        {
                            settingElement.SettingsWater[ind].showContent = true;
                        }
                    }

                    if (settingElement.SettingsWater[ind].Material == null)
                    {
                        GUI.backgroundColor = Color.magenta;
                        if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                        {
                            settingElement.SettingsWater[ind].showContent = true;
                        }
                    }
                    #endregion

                    #region BUTTON MOVE & REMOVE 
                    GUI.backgroundColor = Color.white;
                    if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind > 0);
                        settingElement.SettingsWater.Move(ind, ind - 1);
                    }
                    if (ind < settingElement.SettingsWater.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind < settingElement.SettingsWater.Count);
                        settingElement.SettingsWater.Move(ind, ind + 1);
                    }
                    GUI.contentColor = Color.red;
                    if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                        {
                            if (UsingType.Contains("wt_" + settingElement.SettingsWater[ind].Type.ToString()))
                            {
                                UsingType.Remove("wt_" +
                                                                settingElement.SettingsWater[ind].Type.ToString());
                            }
                            settingElement.SettingsWater.RemoveAt(ind);
                        }
                    }
                    GUI.contentColor = Color.white;
                    #endregion

                    EditorGUILayout.EndHorizontal();
                    if (settingElement.SettingsWater.Count > ind && settingElement.SettingsWater[ind].showContent)
                    {
                        ShowWaterElement(settingElement.SettingsWater[ind]);
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
                settingElement.SettingsBoundary.Add(new SettingsLayers.BoundarySettings()
                {
                    Type = BoundaryType.Unknown,
                    Material = null,
                });
                UsingType.Add("bt_" + BoundaryType.Unknown.ToString());
            }
            GUILayout.Space(10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                for (int ind = 0; ind < settingElement.SettingsBoundary.Count; ind++)
                {

                    EditorGUILayout.BeginHorizontal("box");
                    settingElement.SettingsBoundary[ind].showContent =
                        EditorGUILayout.Foldout(settingElement.SettingsBoundary[ind].showContent,
                            "Boundary TYPE - " + settingElement.SettingsBoundary[ind].Type.ToString());

                    #region CHECK DUBLE TYPE END ERROR
                    GUI.backgroundColor = Color.red;

                    if (UsingType.FindAll(s => s == "bt_" + settingElement.SettingsBoundary[ind].Type.ToString()).Count > 1)
                    {
                        if (GUILayout.Button("Type Exist", "CN CountBadge", GUILayout.Width(75)))
                        {
                            settingElement.SettingsBoundary[ind].showContent = true;
                        }
                    }

                    if (settingElement.SettingsBoundary[ind].Material == null)
                    {
                        GUI.backgroundColor = Color.magenta;
                        if (GUILayout.Button("Mat is not set", "CN CountBadge", GUILayout.Width(95)))
                        {
                            settingElement.SettingsBoundary[ind].showContent = true;
                        }
                    }
                    #endregion

                    #region BUTTON MOVE & REMOVE 
                    GUI.backgroundColor = Color.white;
                    if (ind > 0 && GUILayout.Button(" \u25B2", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind > 0);
                        settingElement.SettingsBoundary.Move(ind, ind - 1);
                    }
                    if (ind < settingElement.SettingsBoundary.Count - 1 && GUILayout.Button("\u25BC", "CN CountBadge", GUILayout.MaxWidth(30)))
                    {
                        Debug.Log(ind < settingElement.SettingsBoundary.Count);
                        settingElement.SettingsBoundary.Move(ind, ind + 1);
                    }
                    GUI.contentColor = Color.red;
                    if (GUILayout.Button("\u2718", "CN CountBadge", GUILayout.MaxWidth(25)))
                    {
                        if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete this type?", "Yes", "No"))
                        {
                            if (UsingType.Contains("bt_" + settingElement.SettingsBoundary[ind].Type.ToString()))
                            {
                                UsingType.Remove("bt_" +
                                                                settingElement.SettingsBoundary[ind].Type.ToString());
                            }
                            settingElement.SettingsBoundary.RemoveAt(ind);
                        }
                    }
                    GUI.contentColor = Color.white;
                    #endregion

                    EditorGUILayout.EndHorizontal();
                    if (settingElement.SettingsBoundary.Count > ind && settingElement.SettingsBoundary[ind].showContent)
                    {
                        ShowBoundaryElement(settingElement.SettingsBoundary[ind]);
                    }
                    EditorGUILayout.Separator();
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void ShowBuildingElement(SettingsLayers.BuildingSettings element)
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
    private void ShowRoadElement(SettingsLayers.RoadSettings element)
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
    private void ShowLanduseElement(SettingsLayers.LanduseSettings element)
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
    private void ShowWaterElement(SettingsLayers.WaterSettings element)
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
    private void ShowBoundaryElement(SettingsLayers.BoundarySettings element)
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

    void CreateNewItemList()
    {
        viewIndex = 1;
        settingElement = Create();
        if (settingElement)
        {
            settingElement.SettingsBuildings = new List<SettingsLayers.BuildingSettings>();
            settingElement.SettingsLanduse = new List<SettingsLayers.LanduseSettings>();
            settingElement.SettingsRoad = new List<SettingsLayers.RoadSettings>();
            settingElement.SettingsWater = new List<SettingsLayers.WaterSettings>();
            settingElement.SettingsBoundary = new List<SettingsLayers.BoundarySettings>();

            string relPath = AssetDatabase.GetAssetPath(settingElement);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
    public SettingsLayers Create()
    {
        SettingsLayers asset = ScriptableObject.CreateInstance<SettingsLayers>();

        AssetDatabase.CreateAsset(asset, PATH_SAVE_SCRIPTABLE_OBJECT);
        AssetDatabase.SaveAssets();
        return asset;
    }

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
