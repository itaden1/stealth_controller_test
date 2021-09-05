using Godot;
using System;

public class DirectionRays : Spatial
{
    private Spatial _parentNode;
    public override void _Ready()
    {
        SetAsToplevel(true);
        _parentNode = (Spatial)GetParent();
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector3 targetPos = _parentNode.GlobalTransform.origin;
        
        Transform targetTransform = new Transform(Basis.Identity, targetPos);

        Transform = targetTransform;
    }
}
