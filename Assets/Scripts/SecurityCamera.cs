using DG.Tweening;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _lightOnMaterial;
    [SerializeField] private Material _lightOffMaterial;

    private void Start()
    {
        Sequence blinkAnim = DOTween.Sequence();
        blinkAnim.AppendCallback(() => {
            Material[] matArray = _meshRenderer.materials;
            matArray[1] = _lightOnMaterial;
            _meshRenderer.materials = matArray;
            });
        blinkAnim.AppendInterval(3f);
        blinkAnim.AppendCallback(() => {
            Material[] matArray = _meshRenderer.materials;
            matArray[1] = _lightOffMaterial;
            _meshRenderer.materials = matArray;
        }); blinkAnim.AppendInterval(1f);
        blinkAnim.SetLoops(-1, LoopType.Restart);
        blinkAnim.Play();
    }

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
