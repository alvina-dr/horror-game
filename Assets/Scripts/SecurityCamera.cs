using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private void Update()
    {
        Vector3 playerCameraVector = (GameManager.Instance.PlayerInputManager.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, playerCameraVector, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.name == GameManager.Instance.PlayerInputManager.name)
            {
               transform.forward = Vector3.RotateTowards(transform.forward, playerCameraVector, 10 * Time.deltaTime, 0);
            }
        }
    }
}
