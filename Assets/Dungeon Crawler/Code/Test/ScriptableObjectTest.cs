using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Structs

[System.Serializable]
public struct CharacterProperties
{
    [Header("Base Character Definition")]
    [SerializeField] public string name;
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public Vector2 spawnPosition;
    [Header("Referenced Assets")]
    [SerializeField] public Sprite characterSprite;
    [SerializeField] public GameObject prefabAgent; //Prefab
}

#endregion

[CreateAssetMenu(menuName = "Scriptable Object Test/New SOT")]
public class ScriptableObjectTest : ScriptableObject
{
    [SerializeField] CharacterProperties characterProperties;
}
