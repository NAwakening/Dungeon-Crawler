using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace N_Awakening.DungeonCrawler
{
    #region Enums

    public enum PlayerIndexes
    {
        //PlayerInput starts the first index (of player) with 0
        ONE = 0,
        TWO = 1,
        THREE = 2,
        FOUR = 3,
    }

    #endregion

    #region Structs


    #endregion

    public class PlayersAvatar : Agent
    {
        #region Knobs

        public PlayerIndexes playerIndex;

        #endregion

        #region References

        [SerializeField] protected HitBox _hitBox;

        #endregion

        #region RuntimeVariables

        protected Vector2 _movementInputVector;
        protected bool _isCarrying;

        #endregion

        #region LocalMethods

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _movementInputVector = Vector2.zero;
            if (_hitBox == null )
            {
                _hitBox = transform.GetChild(0).gameObject.GetComponent<HitBox>();
            }
        }

        protected virtual void CalculateSprintStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) >= 0.5f)
            {
                _movementStateMechanic = StateMechanics.SPRINT_DOWN;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) >= 0.5f)
            {
                _movementStateMechanic = StateMechanics.SPRINT_RIGHT;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) >= 0.5f)
            {
                _movementStateMechanic = StateMechanics.SPRINT_UP;
            }
            else
            {
                _movementStateMechanic = StateMechanics.SPRINT_LEFT;
            }
        }

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            InitializeAgent();
            #endif
        }

        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _movementInputVector;
        }

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            _hitBox.ActivateHitBox();
        }

        public void OnMOVE(InputAction.CallbackContext value)
        {
            if (!IsDead) //_fsm.GetCurrentState == States.DEATH
            {
                if (value.performed)
                {
                    _movementInputVector = value.ReadValue<Vector2>();
                }
                else if (value.canceled)
                {
                    _movementInputVector = Vector2.zero;
                    _fsm.SetMovementDirection = Vector2.zero;
                    _fsm.SetMovementSpeed = 0.0f;
                    _fsm.StateMechanic(StateMechanics.STOP);
                }
                switch (_fsm.GetCurrentState)
                {
                    case States.IDLE_DOWN:
                    case States.IDLE_LEFT:
                    case States.IDLE_RIGHT: 
                    case States.IDLE_UP:
                    case States.MOVING_DOWN:
                    case States.MOVING_LEFT: 
                    case States.MOVING_RIGHT:
                    case States.MOVING_UP:
                        _fsm.SetMovementDirection = _movementInputVector;
                        _fsm.SetMovementSpeed = 3.0f;
                        CalculateMoveStateMechanicDirection();
                        _fsm.StateMechanic(_movementStateMechanic);
                        break;
                    case States.SPRINTING_DOWN:
                    case States.SPRINTING_UP:
                    case States.SPRINTIN_LEFT:
                    case States.SPRINTIN_RIGHT:
                        _fsm.SetMovementDirection = _movementInputVector;
                        CalculateSprintStateMechanicDirection();
                        _fsm.StateMechanic(_movementStateMechanic);
                        break;
                    default:
                        break;

                }
            }
            
        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if (!_isCarrying && !_hitBox.IsHitBoxActive && !IsDead)
            {
                if (value.performed)
                {
                    _fsm.StateMechanic(StateMechanics.ATTACK);
                }
                else if (value.canceled)
                {

                }
            }
            
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (!_isCarrying && !IsDead)
            {
                switch (_fsm.GetCurrentState)
                {
                    case States.MOVING_DOWN:
                    case States.MOVING_LEFT:
                    case States.MOVING_RIGHT:
                    case States.MOVING_UP:
                        if (value.performed)
                        {
                            _fsm.SetMovementSpeed = 6.0f;
                            CalculateSprintStateMechanicDirection();
                            _fsm.StateMechanic(_movementStateMechanic);
                        }
                        break;
                    case States.SPRINTING_DOWN:
                    case States.SPRINTING_UP:
                    case States.SPRINTIN_LEFT:
                    case States.SPRINTIN_RIGHT:
                        if (value.canceled)
                        {
                            _fsm.SetMovementSpeed = 3.0f;
                            CalculateMoveStateMechanicDirection();
                            _fsm.StateMechanic(_movementStateMechanic);
                        }
                        break;
                    default: //Remaining cases

                        break;
                }
                
            }
        }

        public void OnPAUSE(InputAction.CallbackContext value)
        {
            if (!IsDead)
            {

            }
        }

        public void OnINTERACT(InputAction.CallbackContext value)
        {
            if (!IsDead)
            {

            }
        }

        #endregion

        #region GettersSetters

        public HitBox GetHitBox
        {
            get { return _hitBox; }
        }

        #endregion
    }
}