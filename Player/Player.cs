using Godot;
using System;

public class Player : KinematicBody
{
    [Export]
    public int Speed = 15;
    [Export]
    public int StealthSpeed = 6;
    [Export]
    public float Acceleration = 10;

    private int _speed;

    private CameraPivot _cameraPivot;
    private RayCast _rayCastNorth;
    private RayCast _rayCastSouth;
    private RayCast _rayCastEast;
    private RayCast _rayCastWest;

    private Vector3 _velocity = new Vector3();
    private Vector3 _direction = new Vector3();
    private float _targetAngle;
    private bool _stealthMode;
    public override void _Ready()
    {
        // save references to child nodes
        _cameraPivot = (CameraPivot)GetNode("CameraPivot");

        _rayCastNorth = (RayCast)GetNode("DirectionRays/RayCastNorth");

        _rayCastSouth = (RayCast)GetNode("DirectionRays/RayCastSouth");

        _rayCastEast = (RayCast)GetNode("DirectionRays/RayCastEast");

        _rayCastWest = (RayCast)GetNode("DirectionRays/RayCastWest");

        _speed = Speed;
    }
    public override void _Input(InputEvent inputEvent)
    {
       if (Input.IsActionPressed("Hide"))
       {
            _stealthMode = true;
            _speed = StealthSpeed;
       }
       else if (Input.IsActionJustReleased("Hide"))
       {
            _cameraPivot.NuetralizeRotation();
           _stealthMode = false;
           _speed = Speed;
       }
    }
    public override void _PhysicsProcess(float delta)
    {
        // clean up transform if required
        Transform = Transform.Orthonormalized();

        // reset the direction each frame
        _direction = new Vector3();

        // Movement states
        if (_stealthMode) HandleStealthState(delta);
        else HandleFreeState(delta);

        
        _direction = _direction.Normalized();
        _velocity = _velocity.LinearInterpolate(_direction * _speed, Acceleration * delta);
        _velocity = MoveAndSlide(_velocity, Vector3.Up);

        float lerpedRotation = Mathf.LerpAngle(Rotation.y, _targetAngle, 5 * delta);
     
        Rotation = new Vector3(0, lerpedRotation, 0);
    }

    private void HandleFreeState(float delta)
    {

        if (Input.IsActionPressed("MoveUp"))
        {
            _targetAngle = 0f;
            _direction.z -= 1;
        }
        if (Input.IsActionPressed("MoveDown"))
        {
            _targetAngle = (float)(180 * Math.PI / 180);
            _direction.z += 1;
        }
        if (Input.IsActionPressed("MoveRight"))
        {
            _targetAngle = (float)(-90 * Math.PI / 180);
            _direction.x += 1;
        }
        if (Input.IsActionPressed("MoveLeft"))
        {
            _targetAngle = (float)(90 * Math.PI / 180);
            _direction.x -= 1;
        }
    }

    private void HandleStealthState(float delta)
    {
        if (_rayCastNorth.IsColliding())
        {
            _cameraPivot.FaceNorth();
            _targetAngle = (float)(180 * Math.PI / 180);

            if (Input.IsActionPressed("MoveLeft"))
            {
                _direction.x -= 1;
            }
            else if (Input.IsActionPressed("MoveRight"))
            {
                _direction.x += 1;
            }
        }
        else if (_rayCastSouth.IsColliding())
        {
            _cameraPivot.FaceSouth();
            _targetAngle = 0f;

            if (Input.IsActionPressed("MoveLeft"))
            {
                _direction.x += 1;
            }
            else if (Input.IsActionPressed("MoveRight"))
            {
                _direction.x -= 1;
            }
        }
        else if (_rayCastEast.IsColliding())
        {
            _cameraPivot.FaceEast();
            _targetAngle = (float)(90 * Math.PI / 180);

            if (Input.IsActionPressed("MoveRight"))
            {
                _direction.z += 1;
            }
            else if (Input.IsActionPressed("MoveLeft"))
            {
                _direction.z -= 1;
            }
        }
        else if (_rayCastWest.IsColliding())
        {
            _cameraPivot.FaceWest();
            _targetAngle = (float)(-90 * Math.PI / 180);

            if (Input.IsActionPressed("MoveRight"))
            {
                _direction.z -= 1;
            }
            else if (Input.IsActionPressed("MoveLeft"))
            {
                _direction.z += 1;
            }
        }
        else
        {
            _cameraPivot.NuetralizeRotation();
            HandleFreeState(delta);
        }
    }
}
