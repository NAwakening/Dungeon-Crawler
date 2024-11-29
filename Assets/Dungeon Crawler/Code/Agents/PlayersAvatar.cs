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
        [SerializeField] protected GameObject _sphere;
        [SerializeField] protected HurtBox _hurtBox;
        [SerializeField] protected UIManager _uiManager;
        [SerializeField] protected LevelManager _levelManager;

        #endregion

        #region RuntimeVariables

        protected Vector2 _movementInputVector;
        protected bool _isCarrying, _canTeleport = true, _exitFirstTeleport, _canOpenChest, _canPlaceSphere, _avatarActivated;
        protected Chest _chest;
        protected Pedestal _pedestal;

        #endregion

        #region LocalMethods

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _movementInputVector = Vector2.zero;
            if (_hitBox == null)
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

        private void OnEnable()
        {
            _uiManager.ActivatePanel(playerIndex);
            _levelManager.AddPlayer();
        }

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Teleport"))
            {
                if (_canTeleport)
                {
                    other.GetComponent<Teleport>().PlayerTeleport(transform);
                    _canTeleport = false;
                    StartCoroutine(ResetTeleport());
                }
            }
            if (other.CompareTag("Heart"))
            {
                if (_hurtBox.GetCurrentHealthPoint < 5)
                {
                    _uiManager.GainHeart(playerIndex, _hurtBox.GetCurrentHealthPoint);
                    _hurtBox.GainLife();
                }
                other.gameObject.SetActive(false);
            }
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
                    _fsm.SetMovementDirection = _movementInputVector;
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
                            _fsm.SetMovementSpeed = 3.0f;
                            CalculateMoveStateMechanicDirection();
                            _fsm.StateMechanic(_movementStateMechanic);
                            break;
                        case States.SPRINTING_DOWN:
                        case States.SPRINTING_UP:
                        case States.SPRINTING_LEFT:
                        case States.SPRINTING_RIGHT:
                            CalculateSprintStateMechanicDirection();
                            _fsm.StateMechanic(_movementStateMechanic);
                            break;
                        default:
                            break;

                    }
                }
                else if (value.canceled)
                {
                    _movementInputVector = Vector2.zero;
                    _fsm.SetMovementDirection = Vector2.zero;
                    _fsm.SetMovementSpeed = 0.0f;
                    _fsm.StateMechanic(StateMechanics.STOP);
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
                    case States.SPRINTING_LEFT:
                    case States.SPRINTING_RIGHT:
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
                _levelManager.PauseGame();
            }
        }

        public void OnINTERACT(InputAction.CallbackContext value)
        {
            if (!IsDead)
            {
                if (_isCarrying)
                {
                    _isCarrying = false;
                    _sphere.SetActive(false);
                    if (_canPlaceSphere)
                    {
                        if (!_pedestal.SpherePlaced)
                        {
                            _pedestal.PlaceSphere();
                        }
                        else
                        {
                            LooseSphere();
                        }
                    }
                    else
                    {
                        LooseSphere();
                    }
                }

                else
                {
                    if(_canOpenChest)
                    {
                        _sphere.SetActive(true);
                        _isCarrying = true;
                        _chest.OpenChest();
                    }
                }
            }
        }

        public void LooseSphere()
        {
            _sphere.SetActive(false);
            _isCarrying = false;
            _chest.CloseChest();
            _chest = null;
        }

        public void LooseHeart()
        {
            _uiManager.LooseHeart(playerIndex, _hurtBox.GetCurrentHealthPoint);
        }

        public void KillPlayer()
        {
            _uiManager.DeactivatePanel(playerIndex);
            _levelManager.RemovePlayer();
        }



        #endregion

        #region Corrutinas

        IEnumerator ResetTeleport()
        {
            yield return new WaitForSeconds(0.3f);
            _canTeleport = true;
        }

        #endregion

        #region GettersSetters

        public HitBox GetHitBox
        {
            get { return _hitBox; }
        }

        public bool IsCarrying
        {
            set { _isCarrying = value; }
            get { return _isCarrying; }
        }

        public bool CanOpenChest
        {
            set { _canOpenChest = value;}
        }

        public Chest SetChest
        {
            set { _chest = value; }
        }

        public bool CanPlaceSphere
        {
            set { _canPlaceSphere = value;}
        }

        public Pedestal SetPedestal
        {
            set { _pedestal = value; }
        }

        public bool AvatarActivated
        {
            set { _avatarActivated = value; }
            get { return _avatarActivated; }
        }

        #endregion
    }
}