using UnityEngine;
using System.Collections;

public class artillery : BaseBullet
{

    public float velocity;
    public float angle;
    private float opvordsMomentom, forwardMomentom, gravityForce;
    private Vector2 forward;
    private float startHeight;
	// Use this for initialization
	void Start () {
        startHeight = transform.position.y;
        angle = Mathf.PI / angle;
        opvordsMomentom = velocity * Mathf.Sin(angle);
        forwardMomentom = velocity * Mathf.Cos(angle);
        float distance = Vector2.Distance(new Vector2(target.position.x, target.position.z), new Vector2(transform.position.x, transform.position.z));
        float time = distance / forwardMomentom;
        gravityForce = (2 * (target.position.y - startHeight - opvordsMomentom*time ))/Mathf.Pow(time,2);
        transform.LookAt(target);
        //transform.eulerAngles = new Vector3(-angle , transform.eulerAngles.y, transform.eulerAngles.z);
        forward = (new Vector2(target.position.x, target.position.z) - new Vector2(transform.position.x, transform.position.z)).normalized;
        
    }
	
	// Update is called once per frame
	void Update () {
        opvordsMomentom = opvordsMomentom + (gravityForce * Time.deltaTime);  
        Vector3 temp = new Vector3(forward.x, 0, forward.y) * forwardMomentom+ Vector3.up*opvordsMomentom;
        transform.position += temp * Time.deltaTime;
	}
}
