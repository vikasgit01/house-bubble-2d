using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Traps_SO", menuName = "ScriptableObject/Traps")]
public class Traps_SO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public Sprite rangeSprite;

    public int range;
}
