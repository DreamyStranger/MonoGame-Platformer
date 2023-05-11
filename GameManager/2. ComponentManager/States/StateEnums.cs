namespace MonogameExamples
{
    /// <summary>
    /// Possible interaptable States an object can be in.
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
    /// Possible continuous SuperStates an object can be in.
    /// </summary>
    public enum SuperState
    {
        IsOnGround,
        IsFalling,
        IsJumping,
        IsDoubleJumping,
        IsDead,
        IsSliding,
        IsAppearing,
    }

    public enum AnimationID
    {
        Idle,
        Walk,
        Jump,
        DoubleJump,
        Slide,
        Fall,
        Death,
        Appear,
    }
}