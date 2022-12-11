using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(ThirdPersonModelConfig))]
public class ThirdPersonModelConfigEditor : Editor
{
    ThirdPersonModelConfig tpModelConfig;
    SerializedProperty modelObject;
    SerializedProperty animatorController;

    void OnEnable()
    {
        tpModelConfig = (ThirdPersonModelConfig)target;
        // Fetch the objects from the GameObject script to display in the inspector
        modelObject = serializedObject.FindProperty("model");
        animatorController = serializedObject.FindProperty("animatorController");
        
    }

    
    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox("1. Place your humanoid model on the 'Player Model' field and push the green button below!", MessageType.Info);

        EditorGUILayout.PropertyField(modelObject, new GUIContent("Player Model"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(animatorController, new GUIContent("Animator Controller"));

        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.235f, 0.722f, 0.502f);
        if (GUILayout.Button("Set Model!"))
        {
            tpModelConfig.SetNewModel();
        }
        GUI.backgroundColor = oldColor;

        // It must be at bottom:
        serializedObject.ApplyModifiedProperties();
    }

}