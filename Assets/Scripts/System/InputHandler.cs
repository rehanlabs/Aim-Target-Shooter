using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    // Current screen position of input (mouse or touch)
    public Vector3 ScreenPosition { get; private set; }

    // True only on the frame a press starts
    public bool IsPressedDown { get; private set; }

    // True while input is held
    public bool IsPressed { get; private set; }

    // True on the frame input is released
    public bool IsReleased { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // PC / Editor mouse
        ScreenPosition = Input.mousePosition;
        IsPressedDown = Input.GetMouseButtonDown(0);
        IsPressed = Input.GetMouseButton(0);
        IsReleased = Input.GetMouseButtonUp(0);
#else
        // Mobile touch
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            ScreenPosition = t.position;
            IsPressedDown = t.phase == TouchPhase.Began;
            IsPressed = t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary;
            IsReleased = t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled;
        }
        else
        {
            ScreenPosition = Vector3.zero;
            IsPressedDown = false;
            IsPressed = false;
            IsReleased = false;
        }
#endif
    }

    public bool HasValidPosition()
    {
        return !(float.IsNaN(ScreenPosition.x) || float.IsNaN(ScreenPosition.y)
               || float.IsInfinity(ScreenPosition.x) || float.IsInfinity(ScreenPosition.y));
    }
}
