using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrac : MonoBehaviour
{
    public int Level = 3;
    public int Health = 40;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Level = data.Level;
        Health = data.Health;

        Vector3 position;
        position.x = data.PlayerPosition[0];
        position.y = data.PlayerPosition[1];
        position.z = data.PlayerPosition[2];
        transform.position = position;

    }

}
