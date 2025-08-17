using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
 
   
    public Vector2 mouseScreenPos;
    public Vector2 mouseWorldPos;

    public Image cursorImage;
    public Transform cursorCanvas;
    public GameObject cursorSliderGameObject;
    public Slider cursorSlider;
    public Image cursorSliderFillImage;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnMousePositionPerformed(InputAction.CallbackContext ctx)
    {
        cursorImage.enabled = true;
        mouseScreenPos = ctx.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
    private void OnMousePositionCanceled(InputAction.CallbackContext ctx)
    {
        cursorImage.enabled = false;
        mouseScreenPos = Vector2.zero;
        mouseWorldPos = Vector2.zero;
    }
    public void SetSliderValue(float v)
    {
        cursorSlider.value = v;
    }
    public void SetCursorSliderActive(bool t)
    {
        cursorSliderGameObject.SetActive(t);
    }
    private void Start()
    {
        SetCursorSliderActive(false);
        Cursor.visible = false;

        InputSystem.GetCurrent().actions.Player.MousePosition.performed += OnMousePositionPerformed;
        InputSystem.GetCurrent().actions.Player.MousePosition.canceled += OnMousePositionCanceled;
    }
    private void OnDestroy()
    {
        InputSystem.GetCurrent().actions.Player.MousePosition.performed -= OnMousePositionPerformed;
        InputSystem.GetCurrent().actions.Player.MousePosition.canceled -= OnMousePositionCanceled;
    }
    private void Update()
    {
        cursorCanvas.position = mouseWorldPos;
    }
}
