using Microsoft.Xna.Framework;

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
    }

    /// <summary>
    /// Component that stores the current state and super state of an object, as well as its state ID, jump counter, and horizontal direction.
    /// </summary>
    public class StateComponent : Component
    {
        // Object state and previous state
        private State _currentState;
        public State previousState { get; private set; }

        // Super state and previous super state
        private SuperState _currentSuperState;
        public SuperState previousSuperState { get; private set; }

        // State ID used for identifying the current animation
        public string stateID { get; private set; }

        // Jump Counter
        public int JumpsPerformed = 0;

        // Flags for movement restrictions
        private bool _canMoveLeft, _canMoveRight;

        // Horizontal direction
        private int _horizontalDirection = 1;

        /// <summary>
        /// Initializes a new instance of the StateComponent class with the default state and super state.
        /// </summary>
        public StateComponent(State currentState = State.Idle, SuperState currentSuperState = SuperState.IsFalling)
        {
            _currentState = currentState;
            _currentSuperState = currentSuperState;
            UpdateStateID();
            _canMoveRight = true;
            _canMoveLeft = true;
        }

        /// <summary>
        /// Updates the state ID based on the current state and super state.
        /// Used for identifying the current animation.
        /// </summary>
        public void UpdateStateID()
        {
            switch (_currentSuperState)
            {
                case SuperState.IsOnGround:
                    stateID = "idle";
                    if (_currentState == State.WalkLeft || _currentState == State.WalkRight)
                    {
                        stateID = "walking";
                    }
                    break;

                case SuperState.IsFalling:
                    stateID = "fall";
                    if (_currentState == State.Slide)
                    {
                        stateID = "slide";
                    }
                    break;

                case SuperState.IsJumping:
                    stateID = "jump";
                    break;

                case SuperState.IsDoubleJumping:
                    stateID = "double_jump";
                    break;
                case SuperState.IsDead:
                    stateID = "death";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the entity can move to the left.
        /// </summary>
        public bool CanMoveLeft
        {
            get { return _canMoveLeft; }
            set
            {
                _canMoveLeft = value;
                if (!_canMoveLeft)
                {
                    CurrentState = State.Slide;
                    JumpsPerformed = 2;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the entity can move to the right.
        /// </summary>
        public bool CanMoveRight
        {
            get { return _canMoveRight; }
            set
            {
                _canMoveRight = value;
                if (!_canMoveRight)
                {
                    CurrentState = State.Slide;
                    JumpsPerformed = 2;
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal direction of the entity.
        /// </summary>
        public int HorizontalDirection { get => _horizontalDirection; set => _horizontalDirection = value; }

        // State
        public State CurrentState
        {
            get => _currentState;
            set
            {
                previousState = _currentState;
                _currentState = value;
                UpdateStateID();
            }
        }

        // SuperState
        public SuperState CurrentSuperState
        {
            get => _currentSuperState;
            set
            {
                previousSuperState = _currentSuperState;
                _currentSuperState = value;
                UpdateStateID();
            }
        }
    }
}

