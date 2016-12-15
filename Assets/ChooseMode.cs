using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseMode : MonoBehaviour {
    private Button BarbarasButton;
    private Button JuRoButton;

	// Use this for initialization
	void Start () {
        BarbarasButton = GameObject.Find("BarbarasButton").GetComponent<Button>();
        BarbarasButton.onClick.AddListener(ClickedBar);
        JuRoButton = GameObject.Find("JuRoButton").GetComponent<Button>();
        JuRoButton.onClick.AddListener(ClickedJuRob);
    }

    void ClickedBar()
    {
        Debug.Log("Clicked Barbara");
        GameObject.Find("Main").GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.ChooseBar);
    }

    void ClickedJuRob()
    {
        Debug.Log("Clicked JuRob");
        GameObject.Find("Main").GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.ChooseJuRob);
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.B))
        {
            ClickedBar();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ClickedJuRob();
        }
	}
}
