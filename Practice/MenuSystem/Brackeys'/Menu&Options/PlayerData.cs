using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int Level;
    public int Health;
    public float[] PlayerPosition;


    public PlayerData(PlayerPrac player)
    {
        Level = player.Level;
        Health = player.Health;
        PlayerPosition = new float[3];
        PlayerPosition[0] = player.transform.position.x;
        PlayerPosition[1] = player.transform.position.y;
        PlayerPosition[2] = player.transform.position.z;
    }

}
