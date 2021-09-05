using Godot;
using System;

public class CameraPivot : Spatial
{
    [Export]
    public float CameraSpeed = 4.5F;
    private Transform _cameraPivotNuetral;
    private Transform _cameraRotationNuetral;
    private Transform _cameraPivotNorth;
    private Transform _cameraRotationNorth;
    private Transform _cameraPivotSouth;
    private Transform _cameraRotationSouth;
    private Transform _cameraPivotEast;
    private Transform _cameraRotationEast;
    private Transform _cameraPivotWest;
    private Transform _cameraRotationWest;
    private Transform _targetCameraPivot;
    private Transform _targetCameraRotation;


    private Camera _camera;
    private Vector3 _rotationAxis = new Vector3(Vector3.Up);
    private float _targetRotation;

    public override void _Ready()
    {
        _camera = (Camera)GetNode("SpringArm/Camera");

        // Do not inherit players rotation, we will need to reset the origin each frame
        SetAsToplevel(true);
        
        _cameraPivotNuetral = Transform;
        _cameraRotationNuetral = _camera.Transform;

        _cameraPivotNorth = _cameraPivotNuetral.Rotated(new Vector3(1,0,0), (float)(90 * Math.PI / 180));
        _cameraRotationNorth = _cameraRotationNuetral;

        _cameraPivotSouth = _cameraPivotNuetral.Rotated(new Vector3(1,0,0), (float)(-70 * Math.PI / 180));
        _cameraRotationSouth = _cameraRotationNuetral.Rotated(new Vector3(0,0,1), (float)(180 * Math.PI / 180));

        _cameraPivotEast = _cameraPivotNuetral.Rotated(new Vector3(0,0,1), (float)(70 * Math.PI / 180));
        _cameraRotationEast = _cameraRotationNuetral.Rotated(new Vector3(0,0,1), (float)(-90 * Math.PI / 180));

        _cameraPivotWest = _cameraPivotNuetral.Rotated(new Vector3(0,0,1), (float)(-70 * Math.PI / 180));
        _cameraRotationWest = _cameraRotationNuetral.Rotated(new Vector3(0,0,1), (float)(90 * Math.PI / 180));

        NuetralizeRotation();
    }
    public override void _PhysicsProcess(float delta)
    {
        // reset global position to that of the parent
        var parentNode = (Spatial)GetParent();
        Vector3 targetPos = parentNode.GlobalTransform.origin;
        
        Transform targetTransform = new Transform(Basis.Identity, targetPos);
        targetTransform = targetTransform.Rotated(_rotationAxis, _targetRotation);
        GlobalTransform.InterpolateWith(targetTransform , 5 * delta);
        GlobalTransform = new Transform(Basis.Identity, targetPos);
        // LookAtFromPosition(pos, targetPos, new Vector3(1,0,0));
        // GlobalTransform = GlobalTransform.InterpolateWith(_targetCameraPivot, 0 * delta);
        // _camera.Transform = _camera.Transform.InterpolateWith(_targetCameraRotation, CameraSpeed * delta);
    }
    public void NuetralizeRotation()
    {
        _targetCameraPivot = _cameraPivotNuetral;
        _targetCameraRotation = _cameraRotationNuetral;
    }
    public void FaceNorth()
    {
        _targetCameraPivot = _cameraPivotNorth;
        _targetCameraRotation = _cameraRotationNorth;
    }
    public void FaceSouth()
    {
        _targetCameraPivot = _cameraPivotSouth;
        _targetCameraRotation = _cameraRotationSouth;
    }

    internal void FaceEast()
    {
        _targetCameraPivot = _cameraPivotEast;
        _targetCameraRotation = _cameraRotationEast;
    }

    internal void FaceWest()
    {
        _targetCameraPivot = _cameraPivotWest;
        _targetCameraRotation = _cameraRotationWest;
    }
}
