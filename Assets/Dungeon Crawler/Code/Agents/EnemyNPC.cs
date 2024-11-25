using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    #region Enums

    public enum EnemyBehaviourState
    {
        PATROL,
        PERSECUTION
    }

    #endregion

    #region Structs


    #endregion

    public class EnemyNPC : Agent
    {
        #region Knobs

        public EnemyBehaviours_ScriptableObject scriptBehaviours;

        #endregion

        #region References

        [SerializeField, HideInInspector]protected GameObject _projectile;
        [SerializeField, HideInInspector] protected Proyectile _proyectile;

        #endregion

        #region RuntimeVariables

        protected EnemyBehaviourState _currentEnemyBehaviourState;
        protected EnemyBehaviour _currentEnemyBehaviour;
        protected int _currentEnemyBehaviourIndex;
        protected Transform _avatarsTransform;
        protected StateMechanics _previousMovementStateMechanic;

        #endregion

        #region LocalMethods

        protected void InvokeStateMechanic()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    _fsm.StateMechanic(StateMechanics.STOP);
                    break;
                case EnemyBehaviourType.FIRE:
                    if (CanFire())
                    {
                        _fsm.StateMechanic(StateMechanics.ATTACK);
                    }
                    else
                    {
                        _fsm.StateMechanic(StateMechanics.STOP);
                    }
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    _fsm.StateMechanic(_movementStateMechanic);
                    break;
            }
        }

        protected bool CanFire()
        {
            if (Vector2.Dot(_movementDirection, new Vector2(_avatarsTransform.position.x, _avatarsTransform.position.y)) >= 0.25f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected IEnumerator TimerForEnemyBehaviour()
        {
            yield return new WaitForSeconds(_currentEnemyBehaviour.time);
            FinalizeSubState();
            GoToNextEnemyBehaviour();
        }

        protected void GoToNextEnemyBehaviour()
        {
            _currentEnemyBehaviourIndex++;
            if (_currentEnemyBehaviourState == EnemyBehaviourState.PATROL)
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.patrolBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[_currentEnemyBehaviourIndex];
            }
            else // PERSECUTING
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.persecutionBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[_currentEnemyBehaviourIndex];
            }
            InitializeSubState();
            CalculateMoveStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    InitializeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    InitializeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    InitializePersecuteTheAvatarSubStateMachine();
                    break;
                case EnemyBehaviourType.FIRE:
                    InitializeFireSubStateMachine();
                    break;
            }
        }

        protected void FinalizeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    FinalizeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    FinalizeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    FinalizePersecuteTheAvatarSubStateMachine();
                    break;
                case EnemyBehaviourType.FIRE:
                    FinalizeFireSubStateMachine();
                    break;
            }
        }

        protected void InitializePatrolBehaviour()
        {
            StopAllCoroutines();
            //To initialize the sub-finite state machine
            _currentEnemyBehaviourState = EnemyBehaviourState.PATROL;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.patrolBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[0];
            }
            else
            {
                //Plan if the array is empty for this enemy NPC
                _currentEnemyBehaviour.type = EnemyBehaviourType.STOP;
                _currentEnemyBehaviour.time = -1; //-1 equals an infinity / perpetual state
            }
            //Initialize the proper sub-state
            InitializeSubState();
            CalculateMoveStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializePersecutionBehaviour()
        {
            StopAllCoroutines();
            _currentEnemyBehaviourState = EnemyBehaviourState.PERSECUTION;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.persecutionBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[0];
            }
            else
            {
                _currentEnemyBehaviour.type = EnemyBehaviourType.PERSECUTE_THE_AVATAR;
                _currentEnemyBehaviour.time = -1;
                _currentEnemyBehaviour.speed = 1.0f;
            }
            InitializeSubState();
            CalculateMoveStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeAgent();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        void FixedUpdate()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    ExecutingStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    ExecutingMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    ExecutingPersecuteTheAvatarSubStateMachine();
                    break;
                case EnemyBehaviourType.FIRE:
                    ExecutingFireSubStateMachine();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                _avatarsTransform = other.gameObject.transform;
                InitializePersecutionBehaviour();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                _avatarsTransform = null;
                InitializePatrolBehaviour();
            }
        }

        #endregion

        #region PublicMethods

        public override void InitializeAgent()
        {
            InitializePatrolBehaviour();
        }

        public override void KillEnemy()
        {
            StopAllCoroutines();
            InitializeStopSubStateMachine();
        }

        public void SetPlayerPosition()
        {
            _proyectile.SetPlayerPosition = _avatarsTransform;
        }

        #endregion

        #region GettersSetters

        public GameObject GetProjectile
        {
            get { return _projectile; }
        }

        #endregion

        #region SubStateMachineStates

        #region StopSubStateMachineMethods

        protected void InitializeStopSubStateMachine()
        {
            _fsm.SetMovementSpeed = 0.0f;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        protected void ExecutingStopSubStateMachine()
        {
            //do nothing
        }

        protected void FinalizeStopSubStateMachine()
        {
            //do nothing
        }

        #endregion StopSubStateMachineMethods

        #region MoveToRandomDirectionSubStateMachineMethods

        protected void InitializeMoveToRandomDirectionSubStateMachine()
        {
            do
            {
                _movementDirection = new Vector2(
                                    UnityEngine.Random.Range(-1.0f, 1.0f),
                                    UnityEngine.Random.Range(-1.0f, 1.0f)
                                ).normalized;
            } while (_movementDirection.magnitude == 0.0f);
            _fsm.SetMovementDirection = _movementDirection;
            
            _fsm.SetMovementSpeed = _currentEnemyBehaviour.speed;
        }

        protected void ExecutingMoveToRandomDirectionSubStateMachine()
        {
            //Do nothing
        }

        protected void FinalizeMoveToRandomDirectionSubStateMachine()
        {
            _fsm.SetMovementDirection = Vector2.zero;
            _fsm.SetMovementSpeed = 0.0f;
        }

        #endregion MoveToRandomDirectionSubStateMachineMethods

        #region PersecuteTheAvatarSubStateMachineMethods

        protected void InitializePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
            _fsm.SetMovementSpeed = _currentEnemyBehaviour.speed;
            _previousMovementStateMechanic = _movementStateMechanic;
        }

        protected void ExecutingPersecuteTheAvatarSubStateMachine()
        {
            //as the avatar may move, we have to update the direction towards him / her
            _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
            CalculateMoveStateMechanicDirection();
            if (_previousMovementStateMechanic != _movementStateMechanic)
            {
                InvokeStateMechanic();
                _previousMovementStateMechanic = _movementStateMechanic;
            }
        }

        protected void FinalizePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetMovementSpeed = 0.0f;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        #endregion PersecuteTheAvatarSubStateMachineMethods

        #region StopSubStateMachineMethods

        protected void InitializeFireSubStateMachine()
        {
            _fsm.SetMovementDirection = Vector2.zero;
            _fsm.SetMovementSpeed = 0.0f;
        }

        protected void ExecutingFireSubStateMachine()
        {
            //do nothing
        }

        protected void FinalizeFireSubStateMachine()
        {
            //do nothing
        }

        #endregion StopSubStateMachineMethods

        #endregion SubStateMachineStates

    }
}