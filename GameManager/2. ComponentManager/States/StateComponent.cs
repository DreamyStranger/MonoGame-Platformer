using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> that stores information about states of an entity.
    /// </summary>
    public class StateComponent : Component
    {
        // Object state and previous state.
        private State _currentState;
        public State previousState { get; private set; }

        // Super state and previous super state.
        private SuperState _currentSuperState;
        public SuperState previousSuperState { get; private set; }

        // Flags for movement restrictions.
        private bool _canMoveLeft, _canMoveRight;

        // Horizontal direction, 1 is right, -1 is left.
        private int _horizontalDirection = 1;

        /// <summary>
        /// Action ID used for identifying the current animation.
        /// </summary>
        public AnimationID AnimationState { get; private set; }

        /// <summary>
        /// Default SuperState that entity should enter after certain actions (appearence, etc).
        /// </summary>
        public SuperState DefaultSuperState { get; private set; }
        
        /// <summary>
        /// Default State that entity should enter after certain actions (appearence, etc).
        /// </summary>
        public State DefaultState { get; private set; }

        /// <summary>
        /// Default horizontal direction, -1 is left, 1 is right.
        /// </summary>
        public int DefaultHorizontalDirection { get; private set; }

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
            DefaultHorizontalDirection = 1;
            if(defaultState == State.WalkLeft)
            {
                DefaultHorizontalDirection = -1;
            }
            _canMoveRight = true;
            _canMoveLeft = true;

            UpdateStateID();
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
                    AnimationState = AnimationID.Idle;
                    if (_currentState == State.WalkLeft || _currentState == State.WalkRight)
                    {
                        AnimationState = AnimationID.Walk;
                    }
                    break;

                case SuperState.IsFalling:
                    AnimationState = AnimationID.Fall;
                    if (_currentState == State.Slide)
                    {
                        AnimationState = AnimationID.Slide;
                    }
                    break;

                case SuperState.IsJumping:
                    AnimationState = AnimationID.Jump;
                    break;

                case SuperState.IsDoubleJumping:
                    AnimationState = AnimationID.DoubleJump;
                    break;

                case SuperState.IsDead:
                    AnimationState = AnimationID.Death;
                    break;

                case SuperState.IsAppearing:
                    AnimationState = AnimationID.Appear;
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

                //if don't want a slide mechanic, delete this
                if (!_canMoveLeft)
                {
                    CurrentState = State.Slide;
                    HorizontalDirection = -1;
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
                
                //if don't want a slide mechanic, delete this
                if (!_canMoveRight)
                {
                    CurrentState = State.Slide;
                    HorizontalDirection = 1;
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
