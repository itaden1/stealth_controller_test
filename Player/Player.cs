using Godot;
using System;

public class Player : KinematicBody
{
    [Export]
    public int Speed = 15;
    [Export]
    public float Acceleration = 10;


    private CameraPivot CameraPivot;
    
    private Vector3 _velocity = new Vector3();
    private Vector3 _direction = new Vector3();

    public override void _Ready()
    {
        CameraPivot = (CameraPivot)GetNode("CameraPivot");
    }
    public override void _Input(InputEvent inputEvent)
    {
       

    }
    public override void _PhysicsProcess(float delta)
    {
        if (!IsOnWall())
        {
            CameraPivot.NuetralizeRotation();
        }
        _direction = new Vector3();
        if (Input.IsActionPressed("MoveUp"))
        {
            if (IsOnWall()) 
            {
                CameraPivot.FaceNorth();
            }
            _direction.z -= 1;
        }
        if (Input.IsActionPressed("MoveDown"))
        {
            if (IsOnWall()) CameraPivot.FaceSouth();
            _direction.z += 1;
        }
        if (Input.IsActionPressed("MoveRight"))
        {
            if (IsOnWall()) CameraPivot.FaceEast();
            _direction.x += 1;
        }
        if (Input.IsActionPressed("MoveLeft"))
        {
            if (IsOnWall()) CameraPivot.FaceWest();
            _direction.x -= 1;
        }
        _direction = _direction.Normalized();
        _velocity = _velocity.LinearInterpolate(_direction * Speed, Acceleration * delta);
        _velocity = MoveAndSlide(_velocity, Vector3.Up);
    }
}
