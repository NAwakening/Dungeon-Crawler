using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    #region Enums


    #endregion

    #region Structs


    #endregion

    //Agent cannot operate without the Rigidbody2D
    [RequireComponent(typeof(Rigidbody2D))]
    public class Agent : MonoBehaviour
    {
        //Configuration parameter of this script
        #region Knobs



        #endregion

        #region References

        [SerializeField] protected HitBox _hitBox;
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected FiniteStateMachine _fsm;
        [SerializeField] protected Transform[] _hitboxPositions;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        #endregion

        #region RuntimeVariables

        protected Vector2 _movementDirection;
        protected StateMechanics _movementStateMechanic;
        protected bool _isInvincibile;
        protected float _invincibilityCronometer;
        protected Color _invincibilityColor;

        #endregion

        #region LocalMethods

        protected virtual void CalculateMoveStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) >= 0.5f)
            {
                _movementStateMechanic = StateMechanics.MOVE_DOWN;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) >= 0.5f)
            {
                _movementStateMechanic = StateMechanics.MOVE_RIGHT;
            }   
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) >= 0.5f)
            {
                _movementStateMechanic = StateMechanics.MOVE_UP;
            }
            else
            {
                _movementStateMechanic = StateMechanics.MOVE_LEFT;
            }
        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            //InitializeAgent();
        }

        //ranges from 24 to 200 FPS
        //(according to the computer)
        void Update()
        {
            
        }

        //private void PhysicsUpdate()
        private void FixedUpdate()
        {
            //when we update the rigid body, we do it
            //during the Physics thread update
            //which is the FixedUpdate()
            //within the PhysX Engine (by NVidia) in Unity
            //_rigidbody.velocity = Vector3.right;
            //_rigidbody.AddForce(Vector2.right);
        }

        #endregion

        #region PublicMethods

        public virtual void InitializeAgent()
        {
            //With the RequireComponent we guarantee
            //this reference will be ALWAYS retreived
            /*
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null ) {
                Debug.LogError("Rigid body has not been assigned to " +
                    gameObject.name);
            }
            */
        }

        public virtual void KillEnemy()
        {

        }

        public void StateMechanic(StateMechanics value)
        {
            _fsm.StateMechanic(value);
        }

        public void InvincibleMode(float time)
        {
            _invincibilityCronometer = time;
            StartCoroutine(InvinibilityCorroutine());
        }

        #endregion

        #region Corroutines

        protected IEnumerator InvinibilityCorroutine() 
        {
            _invincibilityColor = _spriteRenderer.color;
            while (_invincibilityCronometer > 0f)
            {
                Debug.Log("parpadeo");
                _invincibilityColor.a = 0.5f;
                _spriteRenderer.color = _invincibilityColor;
                yield return new WaitForSeconds(0.25f);
                _invincibilityColor.a = 1f;
                _spriteRenderer.color = _invincibilityColor;
                yield return new WaitForSeconds(0.25f);
                _invincibilityCronometer -= 0.5f;
            }
        }

        #endregion

        #region GettersSetters

        public bool IsDead
        {
            get { return _fsm.GetCurrentState == States.DEATH; }
        }

        public Transform[] GetHitBoxPositions
        {
            get { return _hitboxPositions; }
        }

        #endregion
    }
}