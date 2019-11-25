using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataController : MonoBehaviour
{
    public static SaveData saveData;

    private void Awake()
    {
        LoadData();
    }
    [ContextMenu("Save Data")]
    private void SaveGame()
    {
        var data = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("GameData", data);
    }


    [ContextMenu("Load Data")]
    private void LoadData()
    {
        var data = PlayerPrefs.GetString("GameData");
        saveData = JsonUtility.FromJson<SaveData>(data);
        
    }
    private void OnDisable()
    {
        SaveGame();
    }
    public static bool GetState(MagicCube magicCube)
    {
        if(saveData.magicCubes == null)
        {
            return false;
        }

        if (saveData.magicCubes.Any(t=> t.id == magicCube.name))
        {
            return saveData.magicCubes.FirstOrDefault(t => t.id == magicCube.name).bRed;
        }
        return false;
    }
    public static void SetState(MagicCube magicCube, bool bRed)
    {
        if (saveData.magicCubes == null)
        {
            saveData.magicCubes = new List<MagicCubeData>();
        }
        var magicCubeData = new MagicCubeData() { id = magicCube.name, bRed = bRed };
        saveData.magicCubes.RemoveAll(t => t.id == magicCubeData.id);
        saveData.magicCubes.Add(magicCubeData);
    }
}
