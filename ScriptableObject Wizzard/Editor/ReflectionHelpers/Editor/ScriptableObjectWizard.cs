using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectWizard : EditorWindow
{
    [Header("Clic on the folder you want create the object")]
    public string path;
    public string assetName = "New";
    private Type[] aviableScriptableObjects = new Type[0];

    private Vector2 scrollPos;

    [MenuItem("RA/ScriptableObjectWizzard")]
    static void Init()
    {
        ScriptableObjectWizard wizard = (ScriptableObjectWizard)GetWindow(typeof(ScriptableObjectWizard));

        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object obj = getActiveFolderPath.Invoke(null, new object[0]);
        string pathToCurrentFolder = obj.ToString();

        wizard.path = pathToCurrentFolder;

        wizard.aviableScriptableObjects = GetAviableClasses();
    }

    private static Type[] GetAviableClasses()
    {
        var types = AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject));
        return AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject));
    }

    private void OnGUI()
    {
        path = EditorGUILayout.TextField("Path", path);
        assetName = EditorGUILayout.TextField("Asset Name", assetName);

        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

        if (!AssetDatabase.IsValidFolder(path) && assetName != "")
        {
            EditorGUILayout.LabelField("Invalid Path!");
            return;
        }

        foreach (Type so in aviableScriptableObjects)
        {
            if (so.Name != typeof(ScriptableObjectWizard).Name)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(so.FullName);

                if (GUILayout.Button("Create"))
                {
                    var obj = CreateInstance(so);

                    AssetDatabase.CreateAsset(obj, path + "/" + assetName + ".asset");
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
