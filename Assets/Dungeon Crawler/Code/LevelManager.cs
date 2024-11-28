using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace N_Awakening.DungeonCrawler 
{
    public class LevelManager : MonoBehaviour
    {
        #region References

        [SerializeField] protected GameObject _pausePanel;

        #endregion

        #region RuntimeVariables

        protected int _playersAlive;
        protected bool _pause;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(2);
            }
        }

        #endregion

        #region PublicMethods

        public void AddPlayer()
        {
            _playersAlive++;
        }

        public void RemovePlayer()
        {
            _playersAlive--;
            if (_playersAlive == 0)
            {
                SceneManager.LoadScene(3);
            }
        }

        public void PauseGame()
        {
            if (!_pause)
            {
                _pause = true;
                _pausePanel.SetActive(true);
                Time.timeScale= 0;
            }
            else
            {
                _pause = false;
                _pausePanel.SetActive(false);
                Time.timeScale= 1;
            }
        }

        #endregion
    }
}

