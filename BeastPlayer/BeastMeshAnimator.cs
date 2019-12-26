using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BeastMeshAnimator : MonoBehaviour
{
    [Header("Playback")]
    public float Framerate = 25f;
    public float FrameInterval;
    public float FrameIntervalCounter = 0f;
    [SerializeField] private int currentMeshIndex = 0;
    [Header("Mesh")]
    public List<Mesh> Meshes = new List<Mesh>();
    public Mesh originMesh;
    public MeshFilter meshFilter;
    public SkinnedMeshRenderer childMesh;
    public List<GameObject> Models = new List<GameObject>();
    public List<MeshFilter> ModelMeshFilters = new List<MeshFilter>();

    public bool BPaused;

    public enum MeshAnimPlaybackDir
    {
        Forwards,
        Backwards,
    }

    // Start is called before the first frame update
    void Start()
    {
        BPaused = true;
        
        GenerateMeshFiltersFromObjects();
        GrabMeshsFromFilters();
        StartCoroutine(MeshAnimating());
    }

    

    private void GenerateMeshFiltersFromObjects()
    {
        foreach(GameObject model in Models)
        {
            GameObject modelInstance = Instantiate(model);
            foreach (MeshFilter meshFilter in modelInstance.GetComponentsInChildren<MeshFilter>())
            {
                ModelMeshFilters.Add(meshFilter);
            }
            Destroy(modelInstance);

        }

    }
    private void GrabMeshsFromFilters()
    {
        foreach (MeshFilter filter in ModelMeshFilters)
        {
            Meshes.Add(filter.sharedMesh);
            
            
        }
    }

    private IEnumerator MeshAnimating()
    {
        
        FrameInterval = 1f / Framerate;
        FrameIntervalCounter += Time.deltaTime;

        UpdateIndex();
        LoopUpdate();
        UpdateMesh();
        
        throw new NotImplementedException();
    }

    private void UpdateMesh()
    {
        throw new NotImplementedException();
    }

    private void LoopUpdate()
    {
        throw new NotImplementedException();
    }

    private void UpdateIndex()
    {
        throw new NotImplementedException();
    }
}
