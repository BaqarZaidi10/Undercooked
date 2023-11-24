// Required using directive for Unity classes and data structures
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a ScriptableObject that defines properties for a kitchen object
[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{

    // The prefab associated with this kitchen object, to be instantiated in the game
    public Transform prefab;

    // The sprite representing the visual appearance of this kitchen object
    public Sprite sprite;

    // The name or identifier for this kitchen object
    public string objectName;
}
