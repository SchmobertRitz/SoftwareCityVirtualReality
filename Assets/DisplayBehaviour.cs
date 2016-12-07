using UnityEngine;
using System.Collections;

public class DisplayBehaviour : MonoBehaviour {
    
	void Start () {
        GetComponent<AnimateThis>().Transformate().FromScale(0).ToScale(1).Duration(1f).Ease(AnimateThis.EasePow2).Start();
	}
	
	public void SetData(Building building, Vector3 startPos, Transform viewport)
    {
        
        Vector3 endPos = viewport.position - Vector3.ProjectOnPlane(viewport.position - startPos, Vector3.up).normalized *  1.5f;
        Quaternion startRot = Quaternion.LookRotation((endPos - startPos).normalized, Vector3.up);
        Quaternion endRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(-viewport.forward, Vector3.up), Vector3.up);

        GetComponent<AnimateThis>().Transformate()
            .FromPosition(startPos)
            .ToPosition(endPos)
            .FromRotation(startRot)
            .ToRotation(endRot)
            .Duration(1).Start();
    }
}
