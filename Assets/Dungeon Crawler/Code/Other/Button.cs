using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    public class Other : MonoBehaviour
    {
        #region Knobs

        [SerializeField]protected bool _activatesObject;

        #endregion

        #region References

        [SerializeField] protected GameObject _objectToModify;
        [SerializeField] protected Animator _anim;

        #endregion

        #region RuntimeVariables

        protected bool _activated;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_activated)
            {
                if (other.CompareTag("Player"))
                {
                    _activated = true;
                    _anim.Play("BActivated");
                    if(_activatesObject)
                    {
                        _objectToModify.SetActive(true);
                    }
                    else
                    {
                        _objectToModify.SetActive(false);
                    }
                }
            }
        }

        #endregion
    }
}

