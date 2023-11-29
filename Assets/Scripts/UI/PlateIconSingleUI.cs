using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;

    // Set the KitchenObjectSO for this icon, updating the displayed sprite.
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        // Set the sprite of the associated Image component to the sprite of the provided KitchenObjectSO.
        image.sprite = kitchenObjectSO.sprite;
    }
}
