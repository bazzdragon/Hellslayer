using UnityEngine;

public class EyeLookAtCamera : MonoBehaviour
{
     [SerializeField] private Camera mainCam;

    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    // void Start()
    // {
    //     mainCam = Camera.main;
    // }

    void Update()
    {
        if (mainCam == null) return;

        Vector3 dir = mainCam.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        Quaternion offsetRot = lookRot * Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, offsetRot, Time.deltaTime * 5f);
    }
}
