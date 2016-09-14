using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SettingsLayers))]

public class LayerSettingEditor : Editor
{
    private SettingsLayers objectSetting;

    public override void OnInspectorGUI()
    {
        objectSetting = target as  SettingsLayers;

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.LabelField("BUILDING TYPE",
                LayerSettingWindows.GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
            GUILayout.Space(10);
            foreach (SettingsLayers.BuildingSettings building in objectSetting.SettingsBuildings)
            {
                EditorGUILayout.LabelField("", building.Type.ToString().ToUpper(), "CN CountBadge",
                    GUILayout.MaxWidth(200));
                GUILayout.Space(2);
            }
            if (objectSetting.SettingsBuildings.Count == 0)
            {
                EditorGUILayout.LabelField("NONE BUILDING TYPE", LayerSettingWindows.GuiTitleSize(12, TextAnchor.MiddleCenter, Color.white));
            }
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.LabelField("ROAD TYPE",LayerSettingWindows.GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
            GUILayout.Space(10);
            foreach (SettingsLayers.RoadSettings road in objectSetting.SettingsRoad)
            {
                EditorGUILayout.LabelField("", road.Type.ToString().ToUpper(), "CN CountBadge", GUILayout.MaxWidth(200));
            }
            if (objectSetting.SettingsRoad.Count == 0)
            {
                EditorGUILayout.LabelField("NONE ROAD TYPE", LayerSettingWindows.GuiTitleSize(12, TextAnchor.MiddleCenter, Color.white));
            }
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(target);

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.LabelField("LANDUSE TYPE", LayerSettingWindows.GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
            GUILayout.Space(10);
            foreach (SettingsLayers.LanduseSettings road in objectSetting.SettingsLanduse)
            {
                EditorGUILayout.LabelField("", road.Type.ToString().ToUpper(), "CN CountBadge", GUILayout.MaxWidth(200));
            }
            if (objectSetting.SettingsLanduse.Count == 0)
            {
                EditorGUILayout.LabelField("NONE LANDUSE TYPE", LayerSettingWindows.GuiTitleSize(12, TextAnchor.MiddleCenter, Color.white));
            }
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.LabelField("WATER TYPE", LayerSettingWindows.GuiTitleSize(14, TextAnchor.MiddleLeft, Color.black));
            GUILayout.Space(10);
            foreach (SettingsLayers.WaterSettings road in objectSetting.SettingsWater)
            {
                EditorGUILayout.LabelField("", road.Type.ToString().ToUpper(), "CN CountBadge", GUILayout.MaxWidth(200));
            }
            if (objectSetting.SettingsWater.Count == 0)
            {
                EditorGUILayout.LabelField("NONE WATER TYPE", LayerSettingWindows.GuiTitleSize(12, TextAnchor.MiddleCenter, Color.white));
            }
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndVertical();
        GUILayout.Space(20);
        if (GUILayout.Button("OPEN THE SETTINGS WINDOW TYPES", GUILayout.Height(30)))
        {
            EditorWindow.GetWindow<LayerSettingWindows>();
        }
    }
}
