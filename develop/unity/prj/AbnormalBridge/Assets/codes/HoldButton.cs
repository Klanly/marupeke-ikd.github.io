using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldButton : MonoBehaviour {

    [SerializeField]
    int count_ = 0;

    public System.Action OnPush { set { onPush_ = value; } }
    System.Action onPush_ = null;

    public UnityEngine.UI.Button Button { get { return button_; } }

    private void Start()
    {
        var eventTrigger = GetComponent<UnityEngine.EventSystems.EventTrigger>();
        if ( eventTrigger == null ) {
            eventTrigger = gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        }
        var entry = new UnityEngine.EventSystems.EventTrigger.Entry();
        entry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
        entry.callback.AddListener( (x) => {
            buttonOn_ = true;
        } );
        eventTrigger.triggers.Add( entry );

        var entryOff = new UnityEngine.EventSystems.EventTrigger.Entry();
        entryOff.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
        entryOff.callback.AddListener( (x) => {
            buttonOn_ = false;
        } );
        eventTrigger.triggers.Add( entryOff );

        button_ = GetComponent<UnityEngine.UI.Button>();
    }

    private void Update()
    {
        if ( buttonOn_ == true ) {
            count_++;
            if ( onPush_ != null )
                onPush_();
        }
    }

    UnityEngine.UI.Button button_;
    bool buttonOn_ = false;
}
