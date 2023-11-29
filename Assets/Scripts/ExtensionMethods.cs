using UnityEngine;

public static class ExtensionMethods
{
    private const string HEAD_NAME = "Head";
    private const string BODY_NAME = "Body";

    /// <summary>
    /// Instantiate a player GameObject with specified parameters.
    /// </summary>
    /// <param name="playerPrefab">The player prefab to instantiate.</param>
    /// <param name="position">The position of the instantiated player.</param>
    /// <param name="rotation">The rotation of the instantiated player.</param>
    /// <param name="playerInputActions">PlayerInputActions associated with the player.</param>
    /// <param name="material">Material to set for the player's head and body.</param>
    /// <returns>The instantiated player GameObject.</returns>
    public static GameObject InstantiatePlayer(GameObject playerPrefab, Vector3 position, Quaternion rotation, PlayerInputActions playerInputActions, Material material)
    {
        GameObject player = GameObject.Instantiate(playerPrefab, position, rotation);

        // Set skin
        foreach (MeshRenderer meshRenderer in player.GetComponentsInChildren<MeshRenderer>())
        {
            if (meshRenderer.transform.name == HEAD_NAME || meshRenderer.transform.name == BODY_NAME)
            {
                meshRenderer.material = material;
            }
        }

        // Set PlayerInputActions
        player.GetComponent<PlayerController>().SetPlayerInputActions(playerInputActions);

        return player;
    }
}
