using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalButton : MonoBehaviour {

    [SerializeField]
    AudioSource se_on;

    public System.Action OnPush { set { onPush_ = value; } }
    System.Action onPush_ = null;

    private void Start()
    {
        var eventTrigger = GetComponent<UnityEngine.EventSystems.EventTrigger>();
        if ( eventTrigger == null ) {
            eventTrigger = gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        }
        var entry = new UnityEngine.EventSystems.EventTrigger.Entry();
        entry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
        entry.callback.AddListener( (x) => {
            se_on.Play();
        } );
        eventTrigger.triggers.Add( entry );
    }
}
