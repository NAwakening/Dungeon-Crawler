using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace N_Awakening.DungeonCrawler 
{
    #region

    public enum GameStates
    {
        GAME,
        PAUSE,
        VICTORY,
        DEFEAT
    }

    #endregion

    public class LevelManager : MonoBehaviour
    {
        #region Knobs

        public GameStates state;

        #endregion

        #region References

        [SerializeField] protected GameObject _pausePanel;

        #endregion

        #region RuntimeVariables

        protected int _playersAlive;
        protected bool _paused;

        #endregion

        #region UnityMethods

        void Start ()
        {
            ChangeState(GameStates.GAME);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ChangeState(GameStates.VICTORY);
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
                ChangeState(GameStates.DEFEAT);
            }
        }

        public void PauseGame()
        {
            if (!_paused)
            {
                ChangeState(GameStates.PAUSE);
            }
            else
            {
                ChangeState(GameStates.GAME);
            }
        }

        #endregion

        #region LocalMethods

        protected void ChangeState(GameStates nextState)
        {
            switch (nextState)
            {
                case GameStates.GAME:
                    GameState();
                    break;
                case GameStates.PAUSE:
                    PauseState();
                    break;
                case GameStates.VICTORY:
                    VictoryState();
                    break;
                case GameStates.DEFEAT:
                    DefeatState();
                    break;
            }
        }

        #endregion

        #region FiniteStateMethods

        protected void GameState()
        {
            state = GameStates.GAME;
            _paused = false;
            _pausePanel.SetActive(false);
            Time.timeScale = 1;
        }

        protected void PauseState()
        {
            state = GameStates.PAUSE;
            _paused = true;
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
        }

        protected void VictoryState()
        {
            state = GameStates.VICTORY;
            LoadScene(2);
        }

        protected void DefeatState()
        {
            state = GameStates.DEFEAT;
            LoadScene(3);
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

