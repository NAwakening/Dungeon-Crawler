using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    #region Enums

    public enum States
    {
        //IDLE
        IDLE_DOWN,
        IDLE_UP,
        IDLE_RIGHT,
        IDLE_LEFT,
        //MOVING
        MOVING_DOWN,
        MOVING_UP,
        MOVING_RIGHT,
        MOVING_LEFT,
        //ATTACKING
        ATTACKING_UP,
        ATTACKING_DOWN,
        ATTACKING_LEFT,
        ATTACKING_RIGHT,
        //
        SPRINTING_UP,
        SPRINTING_DOWN,
        SPRINTING_LEFT,
        SPRINTING_RIGHT,
        DEATH
    }

    public enum StateMechanics
    {
        //STOP
        STOP,
        //MOVE
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        //ATTACK
        ATTACK,
        SPRINT_UP,
        SPRINT_DOWN,
        SPRINT_LEFT,
        SPRINT_RIGHT,
        DIE //TODO: Complete the code to administate this new state
    }

    #endregion

    #region Structs


    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        #region Knobs


        #endregion

        #region References

        [SerializeField,HideInInspector] protected Animator _animator;
        [SerializeField,HideInInspector] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Agent _agent;
        [SerializeField] protected AnimationClip _deathClip;

        #endregion

        #region RuntimeVariables

        [SerializeField] protected States _state;
        [SerializeField] protected Vector2 _movementDirection;
        [SerializeField] protected float _movementSpeed;
        protected Coroutine _deathCoroutine;

        #endregion

        #region RuntimeMethods

        protected void InitializeState()
        {
            switch (_state)
            {
                case States.DEATH:
                    _deathCoroutine = StartCoroutine(DeathCooldown());
                    break;
                case States.ATTACKING_UP:
                    if (_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                        ((PlayersAvatar)_agent).GetHitBox.gameObject.transform.position = _agent.GetHitBoxPositions[1].position;
                    }
                    else
                    {
                        ((EnemyNPC)_agent).GetProjectile.SetActive(true);
                        ((EnemyNPC)_agent).GetProjectile.transform.position = _agent.GetHitBoxPositions[1].position;
                        ((EnemyNPC)_agent).SetPlayerPosition();
                    }
                    break;
                case States.ATTACKING_LEFT:
                    if (_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                        ((PlayersAvatar)_agent).GetHitBox.gameObject.transform.position = _agent.GetHitBoxPositions[3].position;
                    }
                    else
                    {
                        ((EnemyNPC)_agent).GetProjectile.SetActive(true);
                        ((EnemyNPC)_agent).GetProjectile.transform.position = _agent.GetHitBoxPositions[3].position;
                        ((EnemyNPC)_agent).SetPlayerPosition();
                    }
                    break;
                case States.ATTACKING_RIGHT:
                    if (_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                        ((PlayersAvatar)_agent).GetHitBox.gameObject.transform.position = _agent.GetHitBoxPositions[2].position;
                    }
                    else
                    {
                        ((EnemyNPC)_agent).GetProjectile.SetActive(true);
                        ((EnemyNPC)_agent).SetPlayerPosition();
                        ((EnemyNPC)_agent).GetProjectile.transform.position = _agent.GetHitBoxPositions[2].position;
                    }
                    break;
                case States.ATTACKING_DOWN:
                    if (_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                        ((PlayersAvatar)_agent).GetHitBox.gameObject.transform.position = _agent.GetHitBoxPositions[0].position;
                    }
                    else
                    {
                        ((EnemyNPC)_agent).GetProjectile.SetActive(true);
                        ((EnemyNPC)_agent).SetPlayerPosition();
                        ((EnemyNPC)_agent).GetProjectile.transform.position = _agent.GetHitBoxPositions[0].position;
                    }
                    break;
            }
        }

        protected void InitializeFiniteStateMachine()
        {
            if (_agent == null)
            {
                _agent = GetComponent<Agent>();
            }
        }

        protected void CleanAnimatorFlags()
        {
            foreach (StateMechanics stateMechanic in Enum.GetValues(typeof(StateMechanics)))
            {
                _animator.SetBool(stateMechanic.ToString(), false);
            }
        }

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            InitializeFiniteStateMachine();
            #endif
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _movementDirection * _movementSpeed;
        }

        #endregion

        #region PublicMethods

        //Action
        public void StateMechanic(StateMechanics value)
        {
            _animator.SetBool(value.ToString(), true);
        }

        public void SetState(States value)
        {
            CleanAnimatorFlags();
            _state = value;
            //TODO: Pending to develop
            InitializeState();
        }

        public void ResetAnimator()
        {
            _animator.Play("IDLE_DOWN");
        }

        #endregion

        #region Corrutines

        protected IEnumerator DeathCooldown()
        {
            if ( _agent as EnemyNPC)
            {
                _agent.KillEnemy();
            }
            _movementDirection = Vector2.zero; 
            _movementSpeed = 0;
            yield return new WaitForSeconds(_deathClip.length);
            Debug.Log("died");
            if(_agent as PlayersAvatar)
            {
                ((PlayersAvatar)_agent).KillPlayer();
            }
            else if (_agent as DestroyableObjects)
            {
                ((DestroyableObjects)_agent).Destroy();
            }
            gameObject.SetActive(false);
            StopCoroutine(_deathCoroutine);
        }

        #endregion

        #region GettersSetters

        public Vector2 GetMovementDirection
        {
            get { return _movementDirection; }
        }

        public Vector2 SetMovementDirection
        {
            set { _movementDirection = value; }
        }

        public float SetMovementSpeed
        {
            set { _movementSpeed = value; }
        }

        public States GetCurrentState
        {
            get { return _state; }
        }

        #endregion
    }
}