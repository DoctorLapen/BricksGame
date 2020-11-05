using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModestTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;


namespace SuperBricks.Editor
{
    public class MinoEditorWindow : EditorWindow
    {
        private const int WINDOW_WIDTH = 270;
        private const int WINDOW_HEIGHT = 1000;
        private List<Vector2Int> _selectedCells = new List<Vector2Int>();
        private List<VisualElement> _selectedCellElements = new List<VisualElement>();

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
            


           var createButton = root.Query<Button>("CreateButton").First();
           createButton.clicked += CreateMino;
           var clearButton = root.Query<Button>("ClearGrid").First();
           clearButton.clicked += ClearGrid;


        }

        private void SelectedCell(EventBase eventData)
        {
            
            var element = (VisualElement) eventData.target;
            var cellData = (CellData) element.userData;
            if (cellData.isSelected)
            {
                ClearCell(cellData, element);
            }
            else
            {
                var coordinates = cellData.coordinates;
                _selectedCells.Add(coordinates);
                _selectedCellElements.Add(element);
                cellData.isSelected = true;
                element.style.backgroundColor = new StyleColor(Color.black);
            }

          
        }

        private void ClearCell(CellData cellData, VisualElement element)
        {
            _selectedCells.Remove(cellData.coordinates);
            element.style.backgroundColor = new StyleColor(Color.white);
            _selectedCellElements.Remove(element);
            cellData.isSelected = false;
        }

        private void CreateMino()
        {
            Mino mino = ScriptableObject.CreateInstance<Mino>();
            
            List<Vector2Int> localCoordinates = CalculateLocalCoordinates();
            //BlocksLocalCoordinates
            Vector2IntList vectorList = new Vector2IntList();
            vectorList.List.AddRange(localCoordinates);
            mino.BlocksLocalCoordinates.Add(MinoSide.Bottom,vectorList);
            List<Vector2Int> rotateCoordinates = new List<Vector2Int>(localCoordinates);
            ShowCollectiction(rotateCoordinates);
            Debug.Log(Enum.GetNames(typeof(MinoSide)).Length);
            
            for (int index = 1; index < Enum.GetNames(typeof(MinoSide)).Length; index++)
            {
                Debug.Log("rotate");
                MinoSide key = (MinoSide)index;
                rotateCoordinates  = CalculateRotateDirectionCoordinates(rotateCoordinates );
                vectorList = new Vector2IntList();
                vectorList.List.AddRange(rotateCoordinates);
                mino.BlocksLocalCoordinates.Add(key,vectorList);
                ShowCollectiction(rotateCoordinates);
            }
            //Border Bottom
            IntList bordersIndexes =new IntList();
            bordersIndexes.List.AddRange(CalculateBottomBordersIndexes(localCoordinates));
            mino.BorderIndexes.Add(MinoSide.Bottom,bordersIndexes);
            //Border Left
             bordersIndexes =new IntList();
            bordersIndexes.List.AddRange(CalculateLeftBordersIndexes(localCoordinates));
            mino.BorderIndexes.Add(MinoSide.Left,bordersIndexes);
            //Border Top
            bordersIndexes =new IntList();
            bordersIndexes.List.AddRange(CalculateTopBordersIndexes(localCoordinates));
            mino.BorderIndexes.Add(MinoSide.Top,bordersIndexes);
            //Border right
            bordersIndexes =new IntList();
            bordersIndexes.List.AddRange(CalculateRightBordersIndexes(localCoordinates));
            mino.BorderIndexes.Add(MinoSide.Right,bordersIndexes);
            
            SaveMinoAsset(mino);
        }

        private List<Vector2Int> CalculateLocalCoordinates()
        {
            Mino mino;
            var sortedCellCoordinates = _selectedCells.OrderBy(v => v.y).ThenBy(v => v.x).ToList();
            Vector2Int zeroBlock = sortedCellCoordinates[0];
            List<Vector2Int> localCoordinates = new List<Vector2Int>();
            foreach (Vector2Int block in sortedCellCoordinates)
            {
                var localCoordinate = block - zeroBlock;
                localCoordinates.Add(localCoordinate);
                
            }

            return localCoordinates;
        }

        private static List<Vector2Int> CalculateRotateDirectionCoordinates(List<Vector2Int> localCoordinates)
        {
            List<Vector2Int> rotateCoordinates = new List<Vector2Int>();
            foreach (Vector2Int coordinate in localCoordinates)
            {
                Vector2Int rotateCoordinate = new Vector2Int(-coordinate.y,coordinate.x);
                rotateCoordinates.Add(rotateCoordinate);
            }

            return rotateCoordinates;
        }

        private static List<int> CalculateLeftBordersIndexes(List<Vector2Int> localCoordinates)
        {
            Func<Vector2Int,int> func1 = v => v.y;
            Func<IGrouping<int, Vector2Int>, int> func2 = g => g.Min(v => v.x);
            Func<Vector2Int, VectorsSupportItem, bool> func3 = (v, item) => v.x == item.maxItemB;
            
            return  CalculateBordersIndexes(localCoordinates,func1,func2,func3);
        }
        private static List<int> CalculateRightBordersIndexes(List<Vector2Int> localCoordinates)
        {
            Func<Vector2Int,int> func1 = v => v.y;
            Func<IGrouping<int, Vector2Int>, int> func2 = g => g.Max(v => v.x);
            Func<Vector2Int, VectorsSupportItem, bool> func3 = (v, item) => v.x == item.maxItemB;
            
            return  CalculateBordersIndexes(localCoordinates,func1,func2,func3);
        }

        private static List<int> CalculateTopBordersIndexes(List<Vector2Int> localCoordinates)
        {
            Func<Vector2Int,int> func1 = v => v.x;
            Func<IGrouping<int, Vector2Int>, int> func2 = g => g.Min(v => v.y);
            Func<Vector2Int, VectorsSupportItem, bool> func3 = (v, item) => v.y == item.maxItemB;
            
            return  CalculateBordersIndexes(localCoordinates,func1,func2,func3);
        }
        private static List<int> CalculateBottomBordersIndexes(List<Vector2Int> localCoordinates)
        {
            Func<Vector2Int,int> func1 = v => v.x;
            Func<IGrouping<int, Vector2Int>, int> func2 = g => g.Max(v => v.y);
            Func<Vector2Int, VectorsSupportItem, bool> func3 = (v, item) => v.y == item.maxItemB;
            
           return  CalculateBordersIndexes(localCoordinates,func1,func2,func3);
        }

        private static List<int> CalculateBordersIndexes(List<Vector2Int> localCoordinates,Func<Vector2Int,int> func1,Func<IGrouping<int,Vector2Int>,int>  func2,Func <Vector2Int,VectorsSupportItem,bool>func3)
        {
            var groups = localCoordinates.GroupBy(func1);
            var maxBsItems = groups.Select(func2);
            var borders = groups.Zip(maxBsItems, (g, maxB) =>
                    new  VectorsSupportItem{vectors = g.Select(v => v), maxItemB = maxB})
                .Select(item => item.vectors.First(v =>func3(v,item)));
            List<int> bordersIndexes= borders.Select(vector => localCoordinates.IndexOf(vector)).ToList();
            
            return bordersIndexes;
        }

        private static void SaveMinoAsset(Mino mino)
        {
            string path = EditorUtility.SaveFilePanel("Save Mino", "Assets", "Mino", "asset");

            if (path.Length != 0)
            {
                int index = path.IndexOf("/Assets/") + 1;
                path = path.Substring(index);
                AssetDatabase.CreateAsset(mino, path);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();

                Selection.activeObject = mino;
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < _selectedCells.Count; i++)
            { 
                var element = _selectedCellElements[i];
                element.style.backgroundColor = new StyleColor(Color.white);
                var cellData = (CellData) element.userData;
                cellData.isSelected = false;
            }
            _selectedCellElements.Clear();
            _selectedCells.Clear();
        }

        private static void ShowCollectiction <T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Debug.Log(item);
                
            }
        }


        private class VectorsSupportItem
        {
            public IEnumerable< Vector2Int> vectors;
            public int maxItemB;
        }



    }

    

}