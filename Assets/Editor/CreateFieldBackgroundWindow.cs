using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace SuperBricks.Editor
{
    public class CreateFieldBackgroundWindow : EditorWindow
    {
      
        private  ObjectField _mainGameSettingsField;
        private ObjectField _spawnPointField;
        private ColorField _cellColorField;
        private ObjectField _cellPrefabField;
        private FloatField _xOffsetField;
        private FloatField _yOffsetField;
        private ObjectField _parentField;
        private ColorField _backgroundColorField;
        private ObjectField _backgroundPrefabField;

        [MenuItem("Window/CreateFieldBackground")]
        private static void ShowWindow()
        {
            var window = GetWindow<CreateFieldBackgroundWindow>();
            window.titleContent = new GUIContent("CreateFieldBackgroundWindow");
            window.Show();
        }

        private void OnEnable()
        {
            var root = rootVisualElement;
                
            var UXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CreateFieldBackgroundWindow.uxml");
            UXML.CloneTree(root);
            var button =root.Query<Button>("CreateButton").First();
            _mainGameSettingsField = root.Query<ObjectField>("MainGameSettings").First();
            _xOffsetField = root.Query<FloatField>("X_OffsetField").First();
            _yOffsetField = root.Query<FloatField>("Y_OffsetField").First();
            _spawnPointField = root.Query<ObjectField>("StartPointField").First();
            _parentField = root.Query<ObjectField>("ParentField").First();
            _cellColorField = root.Query<ColorField>("CellColorField").First();
            _backgroundColorField = root.Query<ColorField>("BackgroundColorField").First();
            _cellPrefabField= root.Query<ObjectField>("SpritePrefabField").First();
            _backgroundPrefabField= root.Query<ObjectField>("BackgroundPrefabField").First();
            button.clicked += CreateBackground;



        }

        private void CreateBackground()
        {
            IMainGameSettings _mainGameSettings = (MainGameSettings)_mainGameSettingsField.value;
            Transform _spawnPoint = (Transform) _spawnPointField.value;
            Transform _parent = (Transform) _parentField.value;
            SpriteRenderer _cellPrefab = (SpriteRenderer) _cellPrefabField.value;
            SpriteRenderer _backgroundPrefab = (SpriteRenderer)_backgroundPrefabField.value;
            float xOffset = _xOffsetField.value;
            float yOffset = _yOffsetField.value;
            Color cellColor = _cellColorField.value;
            Color backgroundColor = _backgroundColorField.value;
            Color originCellColor = _cellPrefab.color;
            
            _backgroundPrefab.color = backgroundColor;
            float columns =  _mainGameSettings.ColumnAmount; 
            float rows =  _mainGameSettings.RowAmount; 
            float width = _backgroundPrefab.sprite.bounds.size.x;
            float  height = _backgroundPrefab.sprite.bounds.size.y;
            float requiredWidth = (_cellPrefab.sprite.bounds.size.x + xOffset) * columns - xOffset;
            float requiredHeight = (_cellPrefab.sprite.bounds.size.y + yOffset) * rows - yOffset;
            Vector3 backgroundSpawnPoint = _spawnPoint.localPosition;
            backgroundSpawnPoint.x -= _cellPrefab.sprite.bounds.size.x / 2f;
            backgroundSpawnPoint.y += _cellPrefab.sprite.bounds.size.y / 2f;
            backgroundSpawnPoint.x += requiredWidth / 2f;
            backgroundSpawnPoint.y -= requiredHeight / 2f;

            
            float scaleX = requiredWidth / width ;
            float scaleY = requiredHeight / height;
            float scaleZ = 1;
            _backgroundPrefab.transform.localScale = new Vector3(scaleX,scaleY,scaleZ);
            
            Instantiate(_backgroundPrefab,backgroundSpawnPoint, Quaternion.identity,_parent);
            _backgroundPrefab.transform.localScale = Vector3.up;
            

            _cellPrefab.color = cellColor;
            _cellPrefab.name = "BackgroundCell"; 
            Vector3 spawnPosition = _spawnPoint.localPosition;
            for (int row = 0; row < _mainGameSettings.RowAmount; row++)
            {
                for (int column = 0; column < _mainGameSettings.ColumnAmount; column++)
                {
                    Instantiate(_cellPrefab, spawnPosition, Quaternion.identity,_parent);
                    spawnPosition.x += xOffset + _cellPrefab.sprite.bounds.size.x;
                }

                spawnPosition.x = _spawnPoint.localPosition.x;
                spawnPosition.y -= yOffset + _cellPrefab.sprite.bounds.size.y;
            }

            _cellPrefab.color = originCellColor;

        }
        

    }
}