using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshAnimatorPrac : MonoBehaviour
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

    // [HideInInspector]
    public bool BPaused = true;
    // Start is called before the first frame update
    void Start()
    {
        BPaused = true;

        if (meshFilter == null)
        {
            meshFilter = GetComponentInChildren<MeshFilter>();
        }
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        FrameInterval = 1 / Framerate;

        if(Models.Count != 0)
        {
            Models = Models.OrderBy(x => x.name).ToList();
        }


        GenerateMeshFilters();
        foreach(MeshFilter meshFilterInModel in ModelMeshFilters)
        {
            Mesh mesh = meshFilterInModel.sharedMesh;
            Meshes.Add(mesh);
        }

        StartCoroutine(Update_Animation());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        BPaused = false;
    }
    public void Pause()
    {
        BPaused = true;
    }

    private void GenerateMeshFilters()
    {
        foreach(GameObject model in Models)
        {
            GameObject modelInstance = Instantiate(model);
            //foreach(MeshFilter meshFilter in modelInstance.GetComponents<MeshFilter>())
            foreach (MeshFilter meshFilter in modelInstance.GetComponentsInChildren<MeshFilter>()) 
            {
                ModelMeshFilters.Add(meshFilter);
                Debug.Log("insert model to meshfilter");
            }
            Destroy(modelInstance);
        }
    }

    IEnumerator Update_Animation()
    {
        while(true)
        {
            if (BPaused)
            {
                childMesh.sharedMesh = originMesh;
                currentMeshIndex = 0;
            }
            else if (!BPaused)
            {
                FrameInterval = 1f / Framerate;

                FrameIntervalCounter += Time.deltaTime;

                UpdateMeshIndex();

                UpdateMesh();

            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    private void UpdateMeshIndex()
    {
        while(FrameIntervalCounter >= FrameInterval)
        {
            ++currentMeshIndex;
            if(currentMeshIndex >= Meshes.Count - 1)
            {
                if(Meshes.Count == 1)
                {
                    currentMeshIndex = 0;
                }
                else
                {
                    currentMeshIndex = Meshes.Count - 2;
                }
            }
            FrameIntervalCounter -= FrameInterval;
        }
    }

    private void UpdateMesh()
    {
        // meshFilter.sharedMesh = Meshes[currentMeshIndex];
        childMesh.sharedMesh = Meshes[currentMeshIndex];
        
        // Debug.Log("number of animation frame is  " + currentMeshIndex + " now! ");
    }
}
