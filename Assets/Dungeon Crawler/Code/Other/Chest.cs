using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    public class Chest : MonoBehaviour
    {
        #region References

        [SerializeField] Animator _anim;

        #endregion

        #region RunTimeVariables

        protected bool _open;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_open && other.CompareTag("Player"))
            {
                if (!other.GetComponent<PlayersAvatar>().IsCarrying)
                {
                    other.GetComponent<PlayersAvatar>().CanOpenChest = true;
                    other.GetComponent<PlayersAvatar>().SetChest = this;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayersAvatar>().CanOpenChest = false;
                if (!other.GetComponent<PlayersAvatar>().IsCarrying)
                {
                    other.GetComponent<PlayersAvatar>().SetChest = null;
                }
            }
        }

        #endregion

        #region PublicMethods

        public void OpenChest()
        {
            if(!_open)
            {
                _open = true;
                _anim.Play("OpenChest");
            }
        }

        public void CloseChest()
        {
            _open = false;
            _anim.Play("CloseChest");
        }

        #endregion
    }
}
