using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject representing a recipe
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    // List of kitchen objects that make up the recipe
    public List<KitchenObjectSO> kitchenObjectSOList;

    // Name of the recipe
    public string recipeName;
}
