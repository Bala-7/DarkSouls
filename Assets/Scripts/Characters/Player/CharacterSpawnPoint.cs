using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnPoint : MonoBehaviour
{
    public Color gizmoColor;
    public GameCharacter character;

    private void Awake()
    {
        //character.gameObject.SetActive(false);
    }

    public void SpawnCharacter()
    {
        character.gameObject.SetActive(true);
        character.OnCharacterSpawn();
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position + Vector3.up, new Vector3(1, 2, 1));
    }
}
