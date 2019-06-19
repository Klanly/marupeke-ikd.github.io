using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatenToMimic : MonoBehaviour
{
    [SerializeField]
    Light light_;

    [SerializeField]
    UnityEngine.UI.Image youWeweEaten_;

    public System.Action FinishCallback { set; get; }

    private void Awake() {
        youWeweEaten_.gameObject.SetActive( false );
    }

    // Start is called before the first frame update
    void Start()
    {
        float initIntensity = light_.intensity;
        GlobalState.wait( 0.8f, () => {
            return false;
        } ).nextTime( 0.3f, (sec, t) => {
            light_.intensity = Lerps.Float.easeInOut( initIntensity, 0.0f, t );
            return true;
        } ).oneFrame( () => {
            youWeweEaten_.gameObject.SetActive( true );
        } ).wait( 4.0f )
        .finish(()=> {
            FinishCallback();
        } );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
