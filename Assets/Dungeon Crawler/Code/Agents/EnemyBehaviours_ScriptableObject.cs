using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N_Awakening.DungeonCrawler
{
    #region Enum

    public enum EnemyBehaviourType
    {   //They are related to the action mechanics from
        //the Finite State Machine
        STOP,  //STOP
        MOVE_TO_RANDOM_DIRECTION,  //MOVE
        FIRE,  //ATTACK
        PERSECUTE_THE_AVATAR  //MOVE  //Persecution
    }

    #endregion

    #region Structs

    [System.Serializable]
    public struct EnemyBehaviour
    {
        public EnemyBehaviourType type;
        public float speed;
        public float time;
    }

    #endregion

    [CreateAssetMenu(menuName = "Dungeon Crawler/New Enemy Behaviour")]
    public class EnemyBehaviours_ScriptableObject : ScriptableObject
    {
        //Purpose to store the interactive script
        //of the enemy NPC, in order to simulate
        //the Artificial Inteligence of the agent

        //Two Sub-State Machines

        //Patrolling when avatar is not sighted
        [SerializeField] public EnemyBehaviour[] patrolBehaviours;

        //Persecution when avatar is sighted
        [SerializeField] public EnemyBehaviour[] persecutionBehaviours;
    }
}