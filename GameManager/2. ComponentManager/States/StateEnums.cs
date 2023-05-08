namespace ECS_Framework
{
    /// <summary>
    /// The possible interaptable States an object can be in.
    /// </summary>
    public enum State
    {
        Idle,
        WalkLeft,
        WalkRight,
        Jump,
        DoubleJump,
        Slide,
    }

    /// <summary>
    /// The possible continuous SuperStates an object can be in.
    /// </summary>
    public enum SuperState
    {
        IsOnGround,
        IsFalling,
        IsJumping,
        IsDoubleJumping,
        IsDead,
        IsSliding,
    }
}