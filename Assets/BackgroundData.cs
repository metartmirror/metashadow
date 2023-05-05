using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Background", menuName = "ScriptableObjects/Background", order = 1)]
public class BackgroundData : ScriptableObject
{
    public string backgroundName;

    public string description;
    public Material material;
}
