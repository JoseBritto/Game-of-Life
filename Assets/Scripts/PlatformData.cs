using UnityEngine;

public class PlatformData : MonoBehaviour
{
    public static PlatformData Instance;

    [SerializeField]
    private float platformScrollMultiplier = 1;

    public float PlatformScrollMultiplier => platformScrollMultiplier;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;
        this.transform.SetParent(null);
        DontDestroyOnLoad(this);
    }

}
