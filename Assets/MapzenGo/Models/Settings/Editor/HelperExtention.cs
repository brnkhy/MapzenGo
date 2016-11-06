using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace MapzenGo.Models.Settings.Editor
{
    public class HelperExtention : MonoBehaviour {


        public static void GetOrCreateSObject<T>(ref T scriptebleObject, string pathSaveScriptableObject,string nameScriptableObject, Action action = null) where T : ScriptableObject
        {
            var path = pathSaveScriptableObject + nameScriptableObject;
            if (!Directory.Exists(pathSaveScriptableObject)) Directory.CreateDirectory(pathSaveScriptableObject);

            if (File.Exists(Path.Combine(Environment.CurrentDirectory, path)))
            {
                scriptebleObject = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
            }
            else if (scriptebleObject == null)
            {
                scriptebleObject = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(scriptebleObject, path);
                AssetDatabase.SaveAssets();
                if (action != null) action();
            }
        }
        public static T GetOrCreateSObjectReturn<T>(ref T scriptebleObject, string pathSaveScriptableObject, Action action = null) where T : ScriptableObject
        {
            if (!Directory.Exists(pathSaveScriptableObject)) Directory.CreateDirectory(pathSaveScriptableObject);

            var path = pathSaveScriptableObject + typeof(T).Name + ".asset";
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, path)))
            {
                // Debug.LogError("\u25B6 " + "GET OBJECT" + path);
                scriptebleObject = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
            }
            else if (scriptebleObject == null)
            {
                // Debug.LogError("\u25B6 "+"CREATE " + path);
                scriptebleObject = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(scriptebleObject, path);
                AssetDatabase.SaveAssets();
                if (action != null) action();
            }
            return scriptebleObject;
        }


        public static Type[] CreateScriptableObject(Type TypeSeach)
        {
            var assembly = GetAssembly();

            // Get all classes derived from ScriptableObject
            var allScriptableObjects = (from t in assembly.GetTypes()
                where t.IsSubclassOf(TypeSeach)
                select t).ToArray();
            return allScriptableObjects;
        }

        private static Assembly GetAssembly()
        {
            return Assembly.Load(new AssemblyName("Assembly-CSharp"));
        }

    }
}
