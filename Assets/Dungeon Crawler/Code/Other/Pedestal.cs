using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace N_Awakening.DungeonCrawler
{
    public class Pedestal : MonoBehaviour
    {
        #region References

        [SerializeField] GameObject _sphere, _bridge, _bloackade;
        [SerializeField] CinemachineTargetGroup _targetGroup;
        [SerializeField] CinemachineVirtualCamera _virtualCamara;

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
            _bridge.GetComponent<Animator>().Play("Activate");
            _bridge.GetComponent<Tilemap>().color = Color.white;
            _targetGroup.AddMember(_bridge.transform.GetChild(0), 1, 0);
            _virtualCamara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
            _virtualCamara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
            StartCoroutine(EliminateFromTargetGroup());
        }

        #endregion

        #region Corrutina

        IEnumerator EliminateFromTargetGroup()
        {
            yield return new WaitForSeconds(5);
            _targetGroup.RemoveMember(_bridge.transform.GetChild(0));
            _virtualCamara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            _virtualCamara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
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
