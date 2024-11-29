using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace N_Awakening.DungeonCrawler
{
    public class UIManager : MonoBehaviour
    {
        #region References

        [SerializeField] protected GameObject[] _heartpanels;
        [SerializeField] protected Image[] _heartsp1;
        [SerializeField] protected Image[] _heartsp2;
        [SerializeField] protected Image[] _heartsp3;
        [SerializeField] protected Image[] _heartsp4;

        #endregion

        #region publicMethods

        public void ActivatePanel(PlayerIndexes index)
        {
            switch (index)
            {
                case PlayerIndexes.ONE:
                    _heartpanels[0].SetActive(true);
                    break;
                case PlayerIndexes.TWO:
                    _heartpanels[1].SetActive(true); 
                    break;
                case PlayerIndexes.THREE:
                    _heartpanels[2].SetActive(true);
                    break;
                case PlayerIndexes.FOUR:
                    _heartpanels[3].SetActive(true);
                    break;
            }
        }

        public void DeactivatePanel(PlayerIndexes index)
        {
            switch (index)
            {
                case PlayerIndexes.ONE:
                    _heartpanels[0].SetActive(false);
                    break;
                case PlayerIndexes.TWO:
                    _heartpanels[1].SetActive(false);
                    break;
                case PlayerIndexes.THREE:
                    _heartpanels[2].SetActive(false);
                    break;
                case PlayerIndexes.FOUR:
                    _heartpanels[3].SetActive(false);
                    break;
            }
        }

        public void LooseHeart(PlayerIndexes index, int currentLife)
        {
            switch (index)
            {
                case PlayerIndexes.ONE:
                    _heartsp1[currentLife-1].color = Color.black;
                    break;
                case PlayerIndexes.TWO:
                    _heartsp2[currentLife - 1].color = Color.black;
                    break;
                case PlayerIndexes.THREE:
                    _heartsp3[currentLife - 1].color = Color.black;
                    break;
                case PlayerIndexes.FOUR:
                    _heartsp4[currentLife - 1].color = Color.black;
                    break;
            }
        }
        public void GainHeart(PlayerIndexes index, int currentLife)
        {
            switch (index)
            {
                case PlayerIndexes.ONE:
                    _heartsp1[currentLife].color = Color.white;
                    break;
                case PlayerIndexes.TWO:
                    _heartsp2[currentLife].color = Color.white;
                    break;
                case PlayerIndexes.THREE:
                    _heartsp3[currentLife].color = Color.white;
                    break;
                case PlayerIndexes.FOUR:
                    _heartsp4[currentLife].color = Color.white;
                    break;
            }
        }

        #endregion
    }
}

