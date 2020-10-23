using UnityEditor;
using UnityEngine;

namespace SuperBricks.Editor
{
    public class CreateFieldBackgroundWindow : EditorWindow
    {
        [MenuItem("Window/CreateFieldBackground")]
        private static void ShowWindow()
        {
            var window = GetWindow<CreateFieldBackgroundWindow>();
            window.titleContent = new GUIContent("CreateFieldBackgroundWindow");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}