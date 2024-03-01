using UnityEngine;

public class PlayableCharacterInput : MonoBehaviour
{
    public InputActions InputActions { get; private set; }

    private void Awake()
    {
        InputActions ??= new InputActions();

        foreach (var listener in GetComponentsInChildren<INeedInputActions>())
        {
            listener.InjectInputActions(InputActions);
        }
    }

    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}

public interface INeedInputActions
{
    void InjectInputActions(InputActions inputActions);
}
