using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// The possible interaptable States an object can be in.
    /// </summary>
    public enum ObjectState
    {
        Idle,
        WalkLeft,
        WalkRight,
        Jump,
        DoubleJump,
        Slide,
    }

    /// <summary>
    /// The possible continious SuperStates an object can be in.
    /// </summary>
    public enum SuperState
    {
        OnGround,
        isFalling,
        isJumping,
        isDoubleJumping,
    }

    /// <summary>
    /// Component that stores the current state and super state of an object, as well as its state ID and jump counter.
    /// </summary>
    public class StateComponent : Component
    {
        //State
        public ObjectState currentState { get; private set; }
        public ObjectState previousState { get; private set; }

        //SuperState
        public SuperState currentSuperState { get; private set; }
        public SuperState previousSuperState { get; private set; }

        //ID
        public string stateID { get; private set; }

        //Jump Counter
        public int JumpsPerformed = 0;

        /// <summary>
        /// Initializes a new instance of the StateComponent class with the default state and super state.
        /// </summary>
        public StateComponent()
        {
            SetState(ObjectState.Idle);
            SetSuperState(SuperState.isFalling);
            UpdateStateID();
        }

        /// <summary>
        /// Sets the current state to the specified state and updates its ID.
        /// </summary>
        /// <param name="newState">The new state to set.</param>
        public void SetState(ObjectState newState)
        {
            previousState = currentState;
            currentState = newState;
            UpdateStateID();
        }

        /// <summary>
        /// Checks if the current state is the specified state.
        /// </summary>
        /// <param name="state">The state to check against.</param>
        /// <returns>True if the current state is the specified state, false otherwise.</returns>
        public bool IsState(ObjectState state)
        {
            return currentState == state;
        }

        /// <summary>
        /// Sets the current super state to the specified super state.
        /// </summary>
        /// <param name="newSuperState">The new super state to set.</param>
        public void SetSuperState(SuperState newSuperState)
        {
            previousSuperState = currentSuperState;
            currentSuperState = newSuperState;
            UpdateStateID();
        }

        /// <summary>
        /// Checks if the current super state is the specified super state.
        /// </summary>
        /// <param name="superState">The super state to check against.</param>
        /// <returns>True if the current super state is the specified super state, false otherwise.</returns>
        public bool IsSuperState(SuperState superState)
        {
            return currentSuperState == superState;
        }

        /// <summary>
        /// Updates the state ID based on the current state and super state. 
        /// Used for identifying current animation
        /// </summary>
        public void UpdateStateID()
        {
            switch (currentSuperState)
            {
                case SuperState.OnGround:
                    if (currentState == ObjectState.WalkLeft || currentState == ObjectState.WalkRight)
                    {
                        stateID = "walking";
                    }
                    else if (currentState == ObjectState.Idle)
                    {
                        stateID = "idle";
                    }
                    break;

                case SuperState.isFalling:
                    stateID = "fall";
                    if (currentState == ObjectState.Slide) stateID = "slide";
                    break;

                case SuperState.isJumping:
                    stateID = "jump";
                    break;

                case SuperState.isDoubleJumping:
                    stateID = "double_jump";
                    break;
            }
        }
    }
}