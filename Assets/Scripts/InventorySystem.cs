using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public BoxCollider2D spawnArea;
    
    public void AddItem(string item)
    {
        var i = Resources.Load<PuzzlePiece>(item);
        if (i == null)
        {
            Debug.LogError("Item not found: " + item);
            return;
        }
        
        // randomize position in spawn area
        var bounds = spawnArea.bounds;
        Vector2 randomPos = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
        Instantiate(i, randomPos, Quaternion.identity);
    }
    
}
