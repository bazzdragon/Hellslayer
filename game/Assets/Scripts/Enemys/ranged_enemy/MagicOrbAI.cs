using UnityEngine;
using System.Collections;

public class MagicOrbAI : MonoBehaviour
{
    [Header("Hover Settings")]
    [SerializeField] private float hoverRadius = 8f;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float moveInterval = 3f;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 40f;

    private Transform player;
    private bool canAttack = true;
    private Vector3 targetPosition;

    void Start()
    {
        player = player_manager.instance.player.transform;
        StartCoroutine(UpdateTargetPosition());
    }

    void Update()
    {
        if (player == null) return;

        // Hover movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);

        // Attack logic
        if (canAttack)
        {
            canAttack = false; // Prevent multiple coroutines
            StartCoroutine(Attack());
        }
    }

    private IEnumerator UpdateTargetPosition()
    {
        while (true)
        {
            Vector3 offset = Random.insideUnitSphere * hoverRadius;
            offset.y = Random.Range(1f, 5f); // Allow height variation

            targetPosition = player.position + offset;

            yield return new WaitForSeconds(moveInterval);
        }
    }

    private IEnumerator Attack()
    {
        if (projectilePrefab && firePoint)
        {
            // Aim firePoint at the middle of the player (adjust y offset as needed)
            Vector3 target = player.position + Vector3.up * 1f;
            firePoint.LookAt(target);

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            proj.GetComponent<LaserBolt>().GiveOwner(gameObject); // Set the owner of the projectile
            if (proj.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed; // Fixed property name
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
