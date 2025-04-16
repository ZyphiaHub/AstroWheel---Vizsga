using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    private float delayTimer = 1;

	void Start () {
		
	}
	
	void Update () {
        delayTimer -= Time.deltaTime;
        if(delayTimer<= 0){
            Destroy(this.gameObject);
        }
	}
}
