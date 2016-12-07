﻿using UnityEngine;
using System.Collections;

public class LookAtBuildingHandler : MonoBehaviour {

    private Ray ray;
    private float timeNextSample;
    private Building lastSelectedBuilding;
    private Vector3 lastSelectedPosition;
    private GameObject currentSelectionObject;
    
    public float samplingIntervalInSeconds = 0.25f;
    public GameObject selectionPrefab;
    public GameObject displayPrefab;

	void Start () {
        EventBus.Register(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timeNextSample)
        {
            ray.origin = transform.position;
            ray.direction = transform.forward;
            timeNextSample = Time.time + samplingIntervalInSeconds;

            float nearestDist = float.MaxValue;
            Building selectedBuilding = null;

            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100))
            {
                if (hit.distance > nearestDist)
                {
                    continue;
                }
                nearestDist = hit.distance;
                if (hit.collider.transform.GetComponent<Building>() != null)
                {
                    selectedBuilding = hit.collider.transform.GetComponent<Building>();
                    lastSelectedPosition = hit.point;
                }
            }
            if (selectedBuilding != lastSelectedBuilding)
            {
                if (selectedBuilding != null)
                {
                    transform.FindChild("TextHolder/FileNameLabel").GetComponent<TextMesh>().text = selectedBuilding.node.name;
                    transform.FindChild("TextHolder/PathNameLabel").GetComponent<TextMesh>().text = selectedBuilding.node.pathName;
                    AnimateThis anim = transform.FindChild("TextHolder").GetComponent<AnimateThis>();
                    anim.CancelAll();
                    anim.Transformate().Duration(1).FromScale(new Vector3(1, 0, 1)).ToScale(Vector3.one).Ease(AnimateThis.EaseOutElastic).Start();

                    if (currentSelectionObject != null)
                    {
                        Destroy(currentSelectionObject);
                    }
                    currentSelectionObject = (GameObject) GameObject.Instantiate(selectionPrefab);
                    Bounds bounds = selectedBuilding.gameObject.GetComponent<Renderer>().bounds;
                    currentSelectionObject.transform.position = bounds.center;
                    currentSelectionObject.transform.localScale = bounds.size + new Vector3(0.001f, 0.001f, 0.001f);
                }
                else
                {
                    transform.FindChild("TextHolder/FileNameLabel").GetComponent<TextMesh>().text = "";
                    transform.FindChild("TextHolder/PathNameLabel").GetComponent<TextMesh>().text = "";
                    if (currentSelectionObject != null)
                    {
                        Destroy(currentSelectionObject);
                        currentSelectionObject = null;
                    }
                }
                lastSelectedBuilding = selectedBuilding;
            }
        }
	}

    public void OnEvent(Events.BuildingSelectionConfirmedEvent e)
    {
        if (lastSelectedBuilding != null)
        {
            GameObject display = GameObject.Instantiate(displayPrefab);
            display.GetComponent<DisplayBehaviour>().SetData(lastSelectedBuilding, lastSelectedPosition, FindObjectOfType<Camera>().transform);
        }
    }
}
