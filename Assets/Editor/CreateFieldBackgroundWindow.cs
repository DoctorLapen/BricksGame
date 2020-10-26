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
            _spawnPointField = root.Query<ObjectField>("TransformField").First();
            _cellColorField = root.Query<ColorField>("CellColorField").First();
            _cellPrefabField= root.Query<ObjectField>("SpritePrefabField").First();
            button.clicked += GetAssembleName;



        }

        private void GetAssembleName()
        {
            IMainGameSettings _mainGameSettings = (MainGameSettings)_mainGameSettingsField.value;
            Transform spawnPoint = (Transform) _spawnPointField.value;
            
            Debug.Log(_mainGameSettings.RowAmount);
        }
        

    }
}