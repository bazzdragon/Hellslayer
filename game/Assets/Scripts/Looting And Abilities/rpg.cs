using UnityEngine;

public class RPG : MonoBehaviour
{
    public string itemName = "rpg";

    [SerializeField] private PlayerInputManager getInput;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform rocketSpawnPoint;
    [SerializeField] private float rocketSpeed = 50f;
    [SerializeField] private Transform playerCamera;

    void Update()
    {
        if (getInput.AttackInput.WasPressedThisFrame())
        {
            FireRocket();
        }
    }

    private void FireRocket()
    {
        Debug.Log("Rocket Launched");

        // Raycast from player camera to find target point
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Vector3 targetPoint;
        Transform lockOnTarget = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;

            // Look for enemy tag on the root of the hit object
            Transform rootTarget = hit.collider.transform.root;
            if (rootTarget.CompareTag("Enemy"))
            {
                lockOnTarget = rootTarget;
            }
        }
        else
        {
            targetPoint = playerCamera.position + playerCamera.forward * 100f;
        }

        // Calculate direction from rocket spawn point to target point
        Vector3 direction = (targetPoint - rocketSpawnPoint.position).normalized;
        rocketSpawnPoint.rotation = Quaternion.LookRotation(direction);

        // Instantiate rocket and set velocity
        GameObject rocketInstance = Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation);

        Rigidbody rb = rocketInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearDamping = 0f;
            rb.linearVelocity = direction * rocketSpeed;
        }

        // Assign lock-on target to rocket
        Rocket rocketScript = rocketInstance.GetComponent<Rocket>();
        if (rocketScript != null && lockOnTarget != null)
        {
            rocketScript.AssignTarget(lockOnTarget);
        }
    }
}
