using System.Collections;
using UnityEngine;

public abstract class ControlledComponent : ShipComponent
{
    private bool justPlaced = false;
    
    public KeyCode ControlKey { get; set; }

    private void OnMouseOver()
    {
        if (IsPlaced && !justPlaced && Input.GetMouseButtonDown(0))
        {
            ControlKeysManager.Instance.SelectComponent(this);
        }
    }

    public override void OnPlaced()
    {
        base.OnPlaced();

        ControlKey = GetDefaultControlKey();
        Debug.Log("New control key " + ControlKey);

        // Skip selecting when component was placed
        justPlaced = true;
        StartCoroutine(ResetJustPlaced());
    }

    private IEnumerator ResetJustPlaced()
    {
        yield return null;
        justPlaced = false;
    }

    protected abstract KeyCode GetDefaultControlKey();
}