using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    // This method is called when the game object this script is attached to is awakened
    private void Awake()
    {
        // Call the ResetStaticData method on various classes to reset their static data
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        CharacterSelectionSingleUI.ResetStaticData();
        NewPlayerSingleUI.ResetStaticData();
        LobbyNavigationUI.ResetStaticData();
        PlayerLoader.ResetStaticData();
        GameControlsManager.ResetStaticData();
    }
}
