using Godot;
using System;

public class CameraPivot : Spatial
{
    [Export]
    public float CameraSpeed = 4.5F;

    private float _targetAngleY = 0f;
    private float _targetAngleX = 0f;
    private Camera _camera;
    private Spatial _parentNode;
    private Vector3 _rotationAxis = new Vector3(Vector3.Up);
    private float _targetRotation;

    public override void _Ready()
    {
        _camera = (Camera)GetNode("SpringArm/Camera");
        _parentNode = (Spatial)GetParent();

        // Do not inherit players rotation, we will need to reset the origin each frame
        SetAsToplevel(true);
        
        NuetralizeRotation();
    }
    public override void _PhysicsProcess(float delta)
    {
        // reset global position to that of the parent
        Vector3 targetPos = _parentNode.GlobalTransform.origin;
        
        
        Transform targetTransform = new Transform(Basis.Identity, targetPos);

        float lerpedRotationY = Mathf.LerpAngle(Rotation.y, _targetAngleY, 8 * delta);
        float lerpedRotationX = Mathf.LerpAngle(Rotation.x, _targetAngleX, 5 * delta);

        GlobalTransform = targetTransform;
        Rotation = new Vector3(lerpedRotationX, lerpedRotationY, 0);

    }
    public void NuetralizeRotation()
    {
       _targetAngleY = 0f;
        _targetAngleX = 0f;
    }
    public void FaceNorth()
    {

        _targetAngleY = 0f;
        _targetAngleX = (float)(60 * Math.PI / 180);
    }
    public void FaceSouth()
    {
        _targetAngleY = (float)(180 * Math.PI / 180);
        _targetAngleX = (float)(60 * Math.PI / 180);
    }

    internal void FaceEast()
    {
        _targetAngleY = (float)(-90 * Math.PI / 180);
        _targetAngleX = (float)(60 * Math.PI / 180);
    }

    internal void FaceWest()
    {
        _targetAngleY = (float)(90 * Math.PI / 180);
        _targetAngleX = (float)(60 * Math.PI / 180);
    }
}
