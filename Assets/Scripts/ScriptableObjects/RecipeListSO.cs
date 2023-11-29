using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject representing a list of recipes
//[CreateAssetMenu()] // You might want to uncomment this line if you want to create instances from the Unity editor
public class RecipeListSO : ScriptableObject
{
    // List of individual recipes
    public List<RecipeSO> recipeSOList;
}
