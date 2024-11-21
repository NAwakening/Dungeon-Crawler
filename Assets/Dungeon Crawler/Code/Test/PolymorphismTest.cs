using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using N_Awakening.DungeonCrawler;

public class PolymorphismTest : MonoBehaviour
{
    #region Knobs

    //Polymorphism
    public Agent[] agentsInTheGame;

    #endregion

    #region RuntimeVariables

    int countAvatars;
    int countEnemies;
    int countDestroyableObjects;

    #endregion

    #region UnityMethods

    private void Start()
    {
        ManageAgents();
    }

    #endregion

    #region RuntimeMethods

    protected void ManageAgents()
    {
        //casting example
        int randomNumber = (int)UnityEngine.Random.Range(
            0.0f, 100.0f);

        foreach (Agent agent in agentsInTheGame)
        {
            if (agent as EnemyNPC)
            {
                EnemyNPC enemy = (EnemyNPC)agent;
                //TODO: Invoke a method or manage an atttrirbute
                //of this enemy
                countEnemies++;
            }
            else if (agent as PlayersAvatar)
            {
                PlayersAvatar avatar = (PlayersAvatar)agent;
                countAvatars++;
            }
            else if (agent as DestroyableObjects)
            {
                DestroyableObjects destroyableObject =
                    (DestroyableObjects)agent;
                countDestroyableObjects++;
            }
        }

        Debug.Log("Number of avatars " + countAvatars);
        Debug.Log("Enemy count " + countEnemies);
        Debug.Log("Number of Destroyable Objects " + countDestroyableObjects);
    }

    #endregion
}
