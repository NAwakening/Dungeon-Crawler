using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        #region KnobsParameters

        //TODO: Stored in a Sciptable Object for robustness
        public int maxHealthPoints = 3; //HP
        public float cooldownTime = 1f; //Damage (Hit Box) Per Second (Cooldown)

        #endregion

        #region References

        [SerializeField] protected Agent _agent;
        [SerializeField] protected AudioSource _hurtSound;

        #endregion

        #region RuntimeVariables

        protected bool _isInCooldown;
        [SerializeField] protected int _currentHealthPoints;
        

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (_agent == null)
            {
                _agent = transform.parent.gameObject.GetComponent<Agent>();
            }
            #endif
        }

        private void OnEnable()
        {
            _currentHealthPoints = maxHealthPoints;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_isInCooldown && _currentHealthPoints >= 1) //to be immune or not ;)
            {
                //I have the potential to be harmed by a Hit Box :O
                if (other.gameObject.CompareTag("HitBox"))
                {
                    //This Hit Box may hurt me if it is from another type
                    //of entity -> Friendly Fire = false
                    if (other.gameObject.layer != gameObject.layer)
                    {
                        if (_agent as DestroyableObjects && other.gameObject.layer != 6)
                        {
                            return;
                        }
                        else if (_agent as PlayersAvatar)
                        {
                            if (((PlayersAvatar)_agent).IsCarrying)
                            {
                                ((PlayersAvatar)_agent).LooseSphere();
                            }
                            ((PlayersAvatar)_agent).LooseHeart();
                        }
                        else if (_agent as EnemyNPC)
                        {
                            ((EnemyNPC)_agent).StopAfterHit();
                        }
                        _currentHealthPoints -= 1;
                        _hurtSound.Play();
                        if (_currentHealthPoints <= 0)
                        {
                            _agent.StateMechanic(StateMechanics.DIE);
                        }
                        else
                        {
                            StartCoroutine(Cooldown());
                        }
                    }
                }
            }
        }

        #endregion

        #region PublicMethods

        public void GainLife()
        {
            _currentHealthPoints ++;
        }

        #endregion

        #region Coroutines

        protected IEnumerator Cooldown()
        {
            _isInCooldown = true; //To be Inmune for a certain time ;)
            _agent.InvincibleMode(cooldownTime);
            yield return new WaitForSeconds(cooldownTime);
            _isInCooldown = false;
        }

        #endregion

        #region GettersAndSetter

        public int GetCurrentHealthPoint
        {
            get { return _currentHealthPoints; }
        }

        #endregion
    }
}