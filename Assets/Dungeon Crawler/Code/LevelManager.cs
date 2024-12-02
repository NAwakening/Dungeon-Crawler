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
        protected bool _paused;

        #endregion

        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                LoadScene(2);
            }
        }

        #endregion

        #region PublicMethods

        public void LoadScene(int id)
        {
            SceneManager.LoadScene(id);
        }

        public void AddPlayer()
        {
            _playersAlive++;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void RemovePlayer()
        {
            _playersAlive--;
            if (_playersAlive == 0)
            {
                LoadScene(3);
            }
        }

        public void PauseGame()
        {
            if (!_paused)
            {
                _paused = true;
                _pausePanel.SetActive(true);
                Time.timeScale= 0;
            }
            else
            {
                _paused = false;
                _pausePanel.SetActive(false);
                Time.timeScale= 1;
            }
        }

        #endregion

        #region GettersAndSetters

        public bool IsPaused
        {
            get { return _paused; }
        }

        #endregion
    }
}

