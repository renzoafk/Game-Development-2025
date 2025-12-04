using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakTile : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;                // assign in Inspector
    [SerializeField] private string breakableLayer = "Breakable";

    private int breakableLayerIndex;

    private void Awake()
    {
        breakableLayerIndex = LayerMask.NameToLayer(breakableLayer);
    }

    // This object must have a Collider2D marked as IsTrigger = true
    // and a Rigidbody2D on either this object or the tilemap object.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // We only care about hitting the breakable tilemap
        if (other.gameObject.layer != breakableLayerIndex)
            return;

        // Find a point where our trigger touched the tilemap
        Vector3 hitPos = other.ClosestPoint(transform.position);

        // Convert that world position to a cell on the tilemap
        Vector3Int cellPos = tilemap.WorldToCell(hitPos);

        if (tilemap.HasTile(cellPos))
        {
            tilemap.SetTile(cellPos, null);      // remove the tile
            Debug.Log("Broke tile at " + cellPos);
        }
    }
}
