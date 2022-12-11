using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(ThirdPersonControllerMovement))]
public class ThirdPersonControllerMovementEditor : Editor
{
    ThirdPersonControllerMovement cm;
    SerializedProperty walkSpeed;
    SerializedProperty walkBackwardsSpeed;
    SerializedProperty strafeSpeed;
    SerializedProperty rotationSpeed;
    SerializedProperty runSpeedMultiplier;

    SerializedProperty weaponsParent;

    void OnEnable()
    {
        cm = (ThirdPersonControllerMovement)target;
        // Fetch the objects from the GameObject script to display in the inspector
        walkSpeed = serializedObject.FindProperty("walkSpeed");
        walkBackwardsSpeed = serializedObject.FindProperty("backwards_walk_speed");
        strafeSpeed = serializedObject.FindProperty("strafe_speed");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        runSpeedMultiplier = serializedObject.FindProperty("runSpeedMultiplier");

        weaponsParent = serializedObject.FindProperty("weaponsParent");
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("3. Here you can setup the movement speeds and parameters of your controller.", MessageType.Info);
        GUILayout.Space(10);

        EditorGUILayout.Slider(walkSpeed, 1f, 10f, new GUIContent("Walk Speed"));
        EditorGUILayout.Slider(walkBackwardsSpeed, 1f, 10f, new GUIContent("Walk Backwards Speed"));
        EditorGUILayout.Slider(strafeSpeed, 1f, 10f, new GUIContent("Walk Strafe Speed"));
        EditorGUILayout.Slider(rotationSpeed, 0.1f, 1.5f, new GUIContent("Rotation Speed"));
        EditorGUILayout.Slider(runSpeedMultiplier, 1.0f, 2.0f, new GUIContent("Run Speed Multiplier"));

        EditorGUILayout.PropertyField(weaponsParent, new GUIContent("Weapons Parent"));

        // It must be at bottom:
        EditorUtility.SetDirty(cm);
        serializedObject.ApplyModifiedProperties();

    }

}
