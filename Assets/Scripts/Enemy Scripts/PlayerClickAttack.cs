using UnityEngine;

public class PlayerClickAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask enemyLayerMask;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        // Left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("[PlayerClickAttack] Left click detected");
            TryDamageEnemyUnderMouse();
        }
    }

    private void TryDamageEnemyUnderMouse()
    {
        if (mainCam == null)
        {
            Debug.LogError("[PlayerClickAttack] mainCam is NULL – is your camera tagged MainCamera?");
            return;
        }
        // Ray from camera through mouse position
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        // Check if that ray hits any 2D collider on the enemy layer
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, enemyLayerMask);

        if (hit.collider == null)
        {
            Debug.Log("[PlayerClickAttack] Click hit NOTHING on enemy layer");
            return;
        }

        Debug.Log($"[PlayerClickAttack] Click hit {hit.collider.name}");


        // Look for EnemyHealth on the thing we clicked
        EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Debug.Log($"[PlayerClickAttack] Calling TakeDamage({damage}) on {hit.collider.name}");
            enemyHealth.TakeDamage(damage, hit.point);
        }
        else
        {
            Debug.Log("[PlayerClickAttack] No EnemyHealth component found on clicked object");
        }
    }
}
