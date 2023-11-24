// Required using directive for Unity classes and data structures
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a ScriptableObject that holds a list of RecipeSO instances
//[CreateAssetMenu()] // This attribute is commented out, meaning instances of RecipeListSO won't appear in the Unity Editor's Create menu
public class RecipeListSO : ScriptableObject
{

    // List of RecipeSO instances representing different recipes
    public List<RecipeSO> recipeSOList;
}
