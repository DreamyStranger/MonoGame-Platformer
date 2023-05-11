using Microsoft.Xna.Framework;

namespace ECS_Framework
{
    /// <summary>
    /// Component that stores the current state and super state of an object, as well as its state ID, jump counter, and horizontal direction.
    /// </summary>
    public class StateComponent : Component
    {
        public SuperState DefaultSuperState { get; private set; }

        // Object state and previous state
        private State _currentState;
        public State previousState { get; private set; }

        // Super state and previous super state
        private SuperState _currentSuperState;
        public SuperState previousSuperState { get; private set; }

        /// <summary>
        /// action ID used for identifying the current animation
        /// </summary>
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
            _currentSuperState = SuperState.IsAppearing;
            DefaultSuperState = currentSuperState;
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

                case SuperState.IsAppearing:
                    stateID = "appear";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Indicates whether the entity can move to the left.
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
        /// Indicates whether the entity can move to the right.
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
        /// Horizontal direction of the entity.
        /// </summary>
        public int HorizontalDirection { get => _horizontalDirection; set => _horizontalDirection = value; }

        /// <summary>
        /// Interabtable state of the entity.
        /// </summary>
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

        /// <summary>
        /// Continous state of the entity.
        /// </summary>
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
