using UnityEngine;

public class MagicCube :MonoBehaviour
{
    public bool BRed { get; set; }
    private void Start()
    {
        BRed = GameDataController.GetState(this);
        UpdateColor();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                BRed = !BRed;
                UpdateColor();
                GameDataController.SetState(this, BRed);

            }
        }
    }
    private void UpdateColor()
    {
        GetComponent<MeshRenderer>().material.color = BRed ? Color.red : Color.green;
    }
}