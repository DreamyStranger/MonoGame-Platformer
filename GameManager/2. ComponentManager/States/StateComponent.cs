using Microsoft.Xna.Framework;

namespace EC_Framework
{
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

        // Flags for movement restrictions
        private bool _canMoveLeft, _canMoveRight;

        // Horizontal direction
        private int _horizontalDirection = 1;

        /// <summary>
        /// Action ID used for identifying the current animation
        /// </summary>
        public string AnimationID { get; private set; }

        /// <summary>
        /// Default SuperState that entity should enter after certain actions (appearence, etc)
        /// </summary>
        public SuperState DefaultSuperState { get; private set; }
        
        /// <summary>
        /// Default State that entity should enter after certain actions (appearence, etc)
        /// </summary>
        public State DefaultState { get; private set; }

        /// <summary>
        /// Jumps performed by given entity
        /// </summary>
        public int JumpsPerformed = 0;

        /// <summary>
        /// Initializes a new instance of the StateComponent class with the default state and super state.
        /// </summary>
        public StateComponent(State defaultState = State.Idle, SuperState defaultSuperState = SuperState.IsFalling, State currentState = State.Idle, SuperState currentSuperState = SuperState.IsAppearing)
        {
            _currentState = currentState;
            _currentSuperState = currentSuperState;
            DefaultSuperState = defaultSuperState;
            DefaultState = defaultState;
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
                    AnimationID = "idle";
                    if (_currentState == State.WalkLeft || _currentState == State.WalkRight)
                    {
                        AnimationID = "walking";
                    }
                    break;

                case SuperState.IsFalling:
                    AnimationID = "fall";
                    if (_currentState == State.Slide)
                    {
                        AnimationID = "slide";
                    }
                    break;

                case SuperState.IsJumping:
                    AnimationID = "jump";
                    break;

                case SuperState.IsDoubleJumping:
                    AnimationID = "double_jump";
                    break;

                case SuperState.IsDead:
                    AnimationID = "death";
                    break;

                case SuperState.IsAppearing:
                    AnimationID = "appear";
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
