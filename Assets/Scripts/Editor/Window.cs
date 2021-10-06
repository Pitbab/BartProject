using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class Window : EditorWindow
{
    private static Window window = null;
    private GUIContent[] m_Items;
    private Vector2 m_ScrollPostion;
    private int m_SelectedItem = 0;
    private string m_Searchstr = "";
    private static Editor GameobjectEditor;

    [MenuItem("Window/EditorTool/AssetsSearching")]
    public static void ShowWindow()
    {
        if(window == null)
        {
            window = GetWindow<Window>();
            window.titleContent = new GUIContent("Assets Search");
        }
    }

    private void OnEnable()
    {
        FindContent();
    }

    private void OnGUI()
    {
        DrawTool1();
    }

    private void FindContent()
    {
        List<GUIContent> ContentList = new List<GUIContent>();

        string[] paths = { "Assets" };
        string[] guids = AssetDatabase.FindAssets("t:Prefab", paths);

        foreach (var item in guids)
        {
            string _path = AssetDatabase.GUIDToAssetPath(item);
            string _filename = Path.GetFileNameWithoutExtension(_path);

            GUIContent _item = new GUIContent();
            _item.text = _filename;

            GameObject _obj = (GameObject)AssetDatabase.LoadAssetAtPath(_path, typeof(GameObject));

            Texture2D _preview = AssetPreview.GetAssetPreview(_obj);
            _item.image = _preview;

            ContentList.Add(_item);
        }

        m_Items = ContentList.ToArray();
    }

    private void DrawTool1()
    {
        EditorGUILayout.LabelField("inside tool #1");

        m_ScrollPostion = GUILayout.BeginScrollView(m_ScrollPostion);

        m_Searchstr = GUILayout.TextField(m_Searchstr);

        m_SelectedItem = GUILayout.SelectionGrid(m_SelectedItem, m_Items.Where(n => n.text.ToUpper().Contains(m_Searchstr.ToUpper())).ToArray(), 3, GetUIStyle());
        GUILayout.EndScrollView();
    }
    private GUIStyle GetUIStyle()
    {
        GUIStyle _style = new GUIStyle(GUI.skin.button);
        _style.fixedWidth = 200.0f;
        _style.fixedHeight = 200.0f;
        _style.alignment = TextAnchor.LowerCenter;
        _style.imagePosition = ImagePosition.ImageAbove;
        return _style;
    }
}
