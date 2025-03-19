using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class PlayerGPCamera : MonoBehaviour
{
    [SerializeField] private GameObject _cameraMesh;
    [SerializeField] private Transform _handTransform;
    [SerializeField] private AudioSource _takePicture;
    [SerializeField] private AudioSource _takePictureValidation;
    [SerializeField] private AudioSource _takePictureValidationPitched;
    [SerializeField] private AudioSource _pickUpCamera;
    public bool IsUsingCamera;
    private List<Photographable> photographableList = new List<Photographable>();
    [SerializeField] private float _dotProductCloseness;
    [SerializeField] private float _minimumDistance;

    private void Awake()
    {
        photographableList = FindObjectsByType<Photographable>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    private void Update()
    {
        if (!IsUsingCamera)
        {
            _cameraMesh.transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            //_cameraMesh.transform.forward = Vector3.RotateTowards(_cameraMesh.transform.forward, Camera.main.transform.forward, 10 * Time.deltaTime, 0);
            _cameraMesh.transform.position = Camera.main.transform.forward * .5f + transform.position + new Vector3(0, -.2f, 0) + Camera.main.transform.right * .3f;
        }
        else
        {
            _cameraMesh.transform.forward = Camera.main.transform.forward;

        }
    }

    public void OpenCamera()
    {
        IsUsingCamera = true;
        _pickUpCamera.Play();

        _cameraMesh.transform.DOMove(Camera.main.transform.forward * .4f + Camera.main.transform.position, .4f).OnComplete(() =>
        {
            _cameraMesh.gameObject.SetActive(false);
            GameManager.Instance.UIManager.CameraOverlay.gameObject.SetActive(true);
            GameManager.Instance.SceneVolume.profile = GameManager.Instance.CameraVolumeProfile;
        });
    }

    public void TakePicture()
    {
        _takePicture.Play();
        DetectPhotographable();
        DOVirtual.DelayedCall(.1f, () =>
        {
            CloseCamera();
        });
    }

    public void CloseCamera()
    {
        _cameraMesh.gameObject.SetActive(true);
        GameManager.Instance.UIManager.CameraOverlay.gameObject.SetActive(false);
        GameManager.Instance.SceneVolume.profile = GameManager.Instance.NormalVolumeProfile;
        _cameraMesh.transform.position = Camera.main.transform.forward * .4f + Camera.main.transform.position;
        _cameraMesh.transform.DORotateQuaternion(Quaternion.FromToRotation(_cameraMesh.transform.forward, Camera.main.transform.forward), .4f);
        _cameraMesh.transform.DOMove(Camera.main.transform.forward * .5f + transform.position + new Vector3(0, -.2f, 0) + Camera.main.transform.right * .3f, .4f).OnComplete(() =>
        {
            IsUsingCamera = false;
        });
    }

    public void DetectPhotographable()
    {
        List<Photographable> toPhotoraphList = new List<Photographable>();
        for (int i = 0; i < photographableList.Count; i++)
        {
            if (photographableList[i].gameObject.activeSelf && !photographableList[i].WasPhotographed)
            {
                Vector3 playerPhotographableVector = photographableList[i].transform.position - transform.position;
                if (Vector3.Dot(Camera.main.transform.forward, playerPhotographableVector.normalized) > _dotProductCloseness)
                {
                    if (Vector3.Distance(transform.position, photographableList[i].transform.position) < photographableList[i].MinimumDistance)
                    {
                        if (Physics.Raycast(transform.position, playerPhotographableVector, out RaycastHit hitInfo, Mathf.Infinity))
                        {
                            if (hitInfo.transform.name == photographableList[i].name)
                            {
                                toPhotoraphList.Add(photographableList[i]);
                            }
                        }
                    }
                }
            }
        }

        if (toPhotoraphList.Count > 0)
        {
            _takePictureValidation.Play();
            _takePictureValidationPitched.Play();
        }

        for (int i = 0; i < toPhotoraphList.Count; i++)
        {
            Debug.Log("PICTURE OF " + toPhotoraphList[i].name);
            toPhotoraphList[i].Photograph();
        }
    }
}
