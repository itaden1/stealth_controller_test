using Godot;
using System;

public class Player : KinematicBody
{
    [Export]
    public int Speed = 15;
    [Export]
    public float Acceleration = 10;
    [Export]
    public float CameraSpeed = 4.5F;

    private Spatial CameraPivot;
    
    private Transform _cameraPivotNuetral;
    private Transform _cameraPivotNorth;
    private Transform _cameraPivotSouth;
    private Transform _cameraPivotEast;
    private Transform _cameraPivotWest;
    private Transform _targetCameraPivot;
    private Vector3 _velocity = new Vector3();
    private Vector3 _direction = new Vector3();

    public override void _Ready()
    {
        CameraPivot = (Spatial)GetNode("CameraPivot");
        _cameraPivotNuetral = CameraPivot.Transform.Rotated(new Vector3(1,0,0), 0F);
        _cameraPivotNorth = _cameraPivotNuetral.Rotated(new Vector3(1,0,0), (float)(90 * Math.PI / 180));
        _cameraPivotSouth = _cameraPivotNuetral.Rotated(new Vector3(1,0,0), (float)(-90 * Math.PI / 180));
        _cameraPivotEast = _cameraPivotNuetral.Rotated(new Vector3(0,0,1), (float)(90 * Math.PI / 180));
        _cameraPivotWest = _cameraPivotNuetral.Rotated(new Vector3(0,0,1), (float)(-90 * Math.PI / 180));

        _targetCameraPivot = _cameraPivotNuetral;

    }
    public override void _Input(InputEvent inputEvent)
    {
       

    }
    public override void _PhysicsProcess(float delta)
    {
        if (!IsOnWall())
        {
            _targetCameraPivot = _cameraPivotNuetral;
        }
        _direction = new Vector3();
        if (Input.IsActionPressed("MoveUp"))
        {
            if (IsOnWall())
            {
                GD.Print("on wall");
                _targetCameraPivot = _cameraPivotNorth;
            }

            _direction.z -= 1;
        }
        if (Input.IsActionPressed("MoveDown"))
        {
            _direction.z += 1;
        }
        if (Input.IsActionPressed("MoveRight"))
        {
            _direction.x += 1;
        }
        if (Input.IsActionPressed("MoveLeft"))
        {
            _direction.x -= 1;
        }
        _direction = _direction.Normalized();
        _velocity = _velocity.LinearInterpolate(_direction * Speed, Acceleration * delta);
        _velocity = MoveAndSlide(_velocity, Vector3.Up);

        CameraPivot.Transform = CameraPivot.Transform.InterpolateWith(_targetCameraPivot, CameraSpeed * delta);
    }

}
