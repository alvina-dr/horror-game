using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public PlayerInputManager PlayerInputManager;
    public UIManager UIManager;

    [Header("Global Volumes")]
    public VolumeProfile NormalVolumeProfile;
    public VolumeProfile CameraVolumeProfile;
    public Volume SceneVolume;
}
