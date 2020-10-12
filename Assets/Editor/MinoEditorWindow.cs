using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SuperBricks.Editor
{
    public class MinoEditorWindow : EditorWindow
    {
        [MenuItem("Window/MinoEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<MinoEditorWindow>();
            window.titleContent = new GUIContent("MinoEditor");
            window.maxSize = new Vector2(100,100);
            window.Show();
        }
        private void OnEnable()
        {
            var root = rootVisualElement;
                
            var minoEditorUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MinoEditorWindow.uxml");
            var cell = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CellMinoInEditor.uxml");
            minoEditorUXML.CloneTree(root);
            var grid = root.Query<VisualElement>("MinoGrid").First();
          
            for (int i = 0; i < 200; i++)
            {
                cell.CloneTree(grid);
            }




        }

    }

    

}