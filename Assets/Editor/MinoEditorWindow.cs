using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SuperBricks.Editor
{
    public class MinoEditorWindow : EditorWindow
    {
        private const int WINDOW_WIDTH = 270;
        private const int WINDOW_HEIGHT = 1000;
        private List<Vector2Int> _coordinates = new List<Vector2Int>();

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
           // var cell = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CellMinoInEditor.uxml");
            
            minoEditorUXML.CloneTree(root);
            var grid = root.Query<VisualElement>("MinoGrid").First();
          
            for (int row = 0; row < 20; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    var cell = new Button();
                    cell.style.minHeight = 20;
                    cell.style.minWidth = 20;
                    var cellData = new CellData();
                    cellData.coordinates = new Vector2Int(column,row);
                    cellData.isSelected = false;
                    cell.userData = cellData;
                    cell.style.backgroundColor = new StyleColor(Color.white);
                    cell.clickable.clickedWithEventInfo += SelectedCell;
                    
                    grid.Add(cell);
                }

            }
            




        }

        private void SelectedCell(EventBase eventData)
        {
            
            var element = (VisualElement) eventData.target;
            var cellData = (CellData) element.userData;
            if (cellData.isSelected)
            {
                element.style.backgroundColor = new StyleColor(Color.white);
            }
            else
            {
                var coordinates = cellData.coordinates;
                _coordinates .Add(coordinates);
                cellData.isSelected = true;
                element.style.backgroundColor = new StyleColor(Color.black);
            }

          
        }


    }

    

}