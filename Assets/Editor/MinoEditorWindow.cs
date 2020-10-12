using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SuperBricks.Editor
{
    public class MinoEditorWindow : EditorWindow
    {
        private const int WINDOW_WIDTH = 270;
        private const int WINDOW_HEIGHT = 1000;

        [MenuItem("Window/MinoEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<MinoEditorWindow>();
            window.titleContent = new GUIContent("MinoEditor");
            
            window.maxSize = new Vector2(WINDOW_WIDTH,WINDOW_HEIGHT);
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