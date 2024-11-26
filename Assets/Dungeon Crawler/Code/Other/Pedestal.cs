using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    public class Pedestal : MonoBehaviour
    {
        #region References

        [SerializeField] GameObject _sphere, _bridge, _bloackade;

        #endregion

        #region RunTimeVariables

        protected bool _spherePlaced;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayersAvatar>().CanPlaceSphere = true;
                other.GetComponent<PlayersAvatar>().SetPedestal = this;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayersAvatar>().CanPlaceSphere = false;
                other.GetComponent<PlayersAvatar>().SetPedestal = null;
            }
        }

        #endregion

        #region PublicMethods

        public void PlaceSphere()
        {
            _sphere.SetActive(true);
            _spherePlaced = true;
            _bloackade.SetActive(false);
            _bridge.SetActive(true);
        }

        #endregion

        #region GettersAndSetter

        public bool SpherePlaced
        {
            get { return _spherePlaced; }
        }

        #endregion
    }
}
