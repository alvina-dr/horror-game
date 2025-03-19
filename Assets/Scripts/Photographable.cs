using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Photographable : MonoBehaviour
{
    public UnityEvent OnPhotographed;
    public bool WasPhotographed;
    [SerializeField] private AudioSource _moveSound;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _pictureMaterial;
    private Material _basicMaterial;
    public float MinimumDistance;
    [SerializeField] private float _blinkingDelay;

    private void Awake()
    {
        _basicMaterial = _meshRenderer.material;
    }

    public void Photograph()
    {
        if (WasPhotographed) return;

        WasPhotographed = true;
        _meshRenderer.material = _pictureMaterial;
        DOVirtual.DelayedCall(_blinkingDelay, () => _meshRenderer.material = _basicMaterial);
        OnPhotographed?.Invoke();
    }

    public void AddMug()
    {
        GameManager.Instance.PlayerInputManager.AddMug();
    }

    public void Jumpscare()
    {
        GameManager.Instance.PlayerInputManager.Jumpscare.Play();
    }

    public void MoveMug()
    {
        _moveSound.Play();
    }

}
