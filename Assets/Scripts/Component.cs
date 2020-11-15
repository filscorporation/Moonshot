using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Component : MonoBehaviour
{
    #region Properties

    public Component Up
    {
        get => up;
        set
        {
            if (up != null)
                throw new InvalidOperationException();
            
            up = value;
            value.down = this;
            Attach(value);
        }
    }
    public Component Down
    {
        get => down;
        set
        {
            if (down != null)
                throw new InvalidOperationException();
            
            down = value;
            value.up = this;
            Attach(value);
        }
    }
    public Component Right
    {
        get => right;
        set
        {
            if (right != null)
                throw new InvalidOperationException();
            
            right = value;
            value.left = this;
            Attach(value);
        }
    }
    public Component Left
    {
        get => left;
        set
        {
            if (left != null)
                throw new InvalidOperationException();
            
            left = value;
            value.right = this;
            Attach(value);
        }
    }
    
    public List<Component> Neighbours => new List<Component> { up, down, right, left };

    public List<Vector2Int> FreeNeghbours
    {
        get
        {
            List<Vector2Int> result = new List<Vector2Int>();
            if (up == null) result.Add(new Vector2Int(X, Y + 1));
            if (down == null) result.Add(new Vector2Int(X, Y - 1));
            if (right == null) result.Add(new Vector2Int(X + 1, Y));
            if (left == null) result.Add(new Vector2Int(X - 1, Y));

            return result;
        }
    }
    
    public int X { get; set; }
    public int Y { get; set; }
    
    protected bool IsEnabled { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D Collider { get; private set; }
    
    public virtual int Toughness { get; }

    #endregion

    #region Attributes

    private Component up;
    private Component down;
    private Component right;
    private Component left;

    #endregion

    public virtual void Initialize()
    {
        Collider = GetComponent<Collider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Attach(Component component)
    {
        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = component.Rigidbody;
    }
    
    public void Disable()
    {
        IsEnabled = false;
        Collider.enabled = false;
        Rigidbody.simulated = false;
    }
    
    public void Enable()
    {
        IsEnabled = true;
        Collider.enabled = true;
        Rigidbody.simulated = true;
    }
}