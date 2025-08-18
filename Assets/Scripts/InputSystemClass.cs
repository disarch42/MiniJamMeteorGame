using UnityEngine;

public class InputSystemClass : MonoBehaviour
{
    //bu singleton yeni input sisteminin instanceını tutuyor
    static private InputSystemClass _current;
    static public InputSystemClass GetCurrent() { return _current; }
    
    public InputSystem_Actions actions;
    private void Awake()
    {
        if (_current != null)
        {
            Destroy(gameObject);
            return;
        }
        _current = this;
        actions = new InputSystem_Actions();
        actions.Enable();
        actions.Player.Enable();
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        if (_current==this)
        {
            _current = null;
        }
        if (actions != null)
        {
            actions.Player.Disable();
            actions.Dispose(); // ensures no memory leaks
            actions = null;
        }
    }
}
