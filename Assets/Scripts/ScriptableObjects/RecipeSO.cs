// Required using directive for Unity classes and data structures
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This attribute makes it possible to create instances of this scriptable object in the Unity Editor
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{

    // List of KitchenObjectSO representing the ingredients required for the recipe
    public List<KitchenObjectSO> kitchenObjectSOList;

    // Name of the recipe
    public string recipeName;
}
