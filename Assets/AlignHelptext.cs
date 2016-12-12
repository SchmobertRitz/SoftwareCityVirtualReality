using UnityEngine;
using System.Collections;

public class AlignHelptext : MonoBehaviour {


    private Camera camera;
	
    void Start () {
        Show();
	}

    public void Show()
    {
        camera = FindObjectOfType<Camera>();
    }
	
	void Update () {
	    if (camera != null)
        {
            transform.rotation = Quaternion.LookRotation((transform.position - camera.transform.position).normalized, Vector3.up);
        }
	}
}
