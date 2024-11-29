using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    #region Enums

    public enum DestroyableType
    {
        Bush,
        Vase
    }

    #endregion

    #region Structs


    #endregion

    public class DestroyableObjects : Agent
    {
        #region Knobs

        public DestroyableType _type;

        #endregion

        #region References

        [SerializeField] protected GameObject _heart;

        #endregion

        #region RuntimeVariables

        protected GameObject _heartInstance;
        protected int _index;

        #endregion

        #region LocalMethods



        #endregion

        #region UnityMethods

        void Start()
        {
            //we can access the rigidbody via the
            //inheritance of the Agent
            //_rigidbody.velocity = Vector2.right;
        }

        void Update()
        {
            
        }

        #endregion

        #region PublicMethods

        public void Destroy()
        {
            if (_type == DestroyableType.Vase)
            {
                _heartInstance = Instantiate(_heart);
                _heartInstance.transform.position = transform.position;
            }
            else
            {
                _index = Random.Range(1, 11);
                if (_index == 1)
                {
                    _heartInstance = Instantiate(_heart);
                    _heartInstance.transform.position = transform.position;
                }
            }
        }

        #endregion

        #region GettersSetters

        #endregion
    }
}