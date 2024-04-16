using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public BoxCollider2D spawnAreaOutBound;
    public BoxCollider2D spawnAreaInnerBound;

    public void AddItem(string item)
    {
        var i = Resources.Load<GameObject>(item);
        if (i == null)
        {
            Debug.LogWarning("Item not found: " + item);
            return;
        }

        Vector2 randomPos = GetValidSpawnPosition();
        // random rotation
        float randomRotation = Random.Range(0, 360);
        Instantiate(i, randomPos, Quaternion.Euler(0, 0, randomRotation));
    }

    private Vector2 GetValidSpawnPosition()
    {
        spawnAreaInnerBound.enabled = true;
        spawnAreaOutBound.enabled = true;
        Bounds outerBounds = spawnAreaOutBound.bounds;
        Bounds innerBounds = spawnAreaInnerBound.bounds;
        Vector2 randomPos;

        do
        {
            randomPos = new Vector2(
                Random.Range(outerBounds.min.x, outerBounds.max.x),
                Random.Range(outerBounds.min.y, outerBounds.max.y)
            );
        } while (innerBounds.Contains(randomPos));
        
        spawnAreaInnerBound.enabled = false;
        spawnAreaOutBound.enabled = false;

        return randomPos;
    }
}