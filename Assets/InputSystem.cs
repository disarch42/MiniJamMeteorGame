using UnityEngine;

public class InputSystem : MonoBehaviour
{
    //bu singleton yeni input sisteminin instanceını tutuyor
    static private InputSystem _current;
    static public InputSystem GetCurrent() { return _current; }
    
    public InputSystem_Actions actions;
    private void Awake()
    {
        if (_current == null)
        {
            _current = this;
            actions = new InputSystem_Actions();
            actions.Enable();
            actions.Player.Enable();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
