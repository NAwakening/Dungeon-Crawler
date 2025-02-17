using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        #region KnobsParameters

        //public int damage = 1;
        public float lifetime = 1f;

        #endregion

        #region References

        [SerializeField] protected BoxCollider2D _collider;

        #endregion

        #region RuntimeVariables

        protected bool _isHitBoxActive;

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider2D>();
            }
            #endif
        }

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            if (!_isHitBoxActive)
            {
                StartCoroutine(Lifetime());
            }
        }

        public void DeactiveteHitBox()
        {
            _collider.enabled = false;
        }

        public void StartHitbox()
        {
            _collider.enabled = true;
        }

        #endregion

        #region Coroutines

        public IEnumerator Lifetime()
        {
            _isHitBoxActive = true;
            _collider.enabled = true;
            yield return new WaitForSeconds(lifetime);
            _collider.enabled = false;
            _isHitBoxActive = false;
        }

        #endregion

        #region GettersandSetters

        public bool IsHitBoxActive
        {
            get { return _isHitBoxActive; }
        }

        #endregion
    }
}