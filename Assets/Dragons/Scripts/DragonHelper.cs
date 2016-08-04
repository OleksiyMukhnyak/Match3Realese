using UnityEngine;
using System.Collections;

public class DragonHelper : MonoBehaviour
{
    const float Speed = 0.25f;
    public GameObject DragonImage;

    public Vector3 Side { get; set; }
    // Use this for initialization
    void Start()
    {
    }

    public void Run(bool horizontal)
    {
        if (horizontal)
        {
            transform.position = new Vector3(-4, transform.position.y, transform.position.z);
            Side = Vector3.right;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -5, transform.position.z);
            Side = Vector3.up;

            DragonImage.transform.eulerAngles = new Vector3(DragonImage.transform.eulerAngles.x, DragonImage.transform.eulerAngles.y, DragonImage.transform.eulerAngles.z + 90);

        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Side * Speed);
    }
}
