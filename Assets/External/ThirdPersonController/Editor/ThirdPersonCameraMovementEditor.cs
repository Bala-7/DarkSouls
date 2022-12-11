using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(ThirdPersonCameraMovement))]
public class ThirdPersonCameraMovementEditor : Editor
{
    ThirdPersonCameraMovement cm;
    SerializedProperty type;
    SerializedProperty sensitivity;
    SerializedProperty invertXAxis;
    SerializedProperty invertYAxis;
    SerializedProperty lookAt;
    SerializedProperty settings;

    void OnEnable()
    {
        cm = (ThirdPersonCameraMovement)target;
        // Fetch the objects from the GameObject script to display in the inspector
        type = serializedObject.FindProperty("type");
        sensitivity = serializedObject.FindProperty("sensitivity");
        invertXAxis = serializedObject.FindProperty("invertXAxis");
        invertYAxis = serializedObject.FindProperty("invertYAxis");
        lookAt = serializedObject.FindProperty("lookAt");
        settings = serializedObject.FindProperty("settings");

    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("2. Once you have set the model, you can change camera settings here.\nJust be sure the 'LookAt' transform is assigned (I recommend dragging there the Spine or Neck bones of your model).", MessageType.Info);
        EditorGUILayout.PropertyField(lookAt, new GUIContent("Custom LookAt Transform"));
        GUILayout.Space(10);
        
        if (lookAt.objectReferenceValue) { 
            EditorGUILayout.PropertyField(type, new GUIContent("Movement Type"));
            EditorGUILayout.Slider(sensitivity, 0.1f, 5f, new GUIContent("Sensitivity"));
            EditorGUILayout.PropertyField(invertXAxis, new GUIContent("Invert X Axis"));
            EditorGUILayout.PropertyField(invertYAxis, new GUIContent("Invert Y Axis"));
            EditorGUILayout.PropertyField(settings, new GUIContent("Camera Settings"));
        
        }
        // It must be at bottom:
        EditorUtility.SetDirty(cm);
        serializedObject.ApplyModifiedProperties();
        
    }

}
