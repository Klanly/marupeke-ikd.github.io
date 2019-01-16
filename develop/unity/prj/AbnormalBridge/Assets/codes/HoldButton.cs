using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldButton : MonoBehaviour {

    [SerializeField]
    int count_ = 0;

    [SerializeField]
    AudioSource se_on;

    [SerializeField]
    AudioSource se_off;

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
            se_off.Play();
        } );
        eventTrigger.triggers.Add( entryOff );

        button_ = GetComponent<UnityEngine.UI.Button>();
    }

    private void Update()
    {
        if ( buttonOn_ == true ) {
            if ( count_ == 0 ) {
                se_on.Play();
            }
            count_++;
            if ( onPush_ != null )
                onPush_();
        } else {
            count_ = 0;
        }
    }

    public void resetAll()
    {
        buttonOn_ = false;
        count_ = 0;
    }

    UnityEngine.UI.Button button_;
    bool buttonOn_ = false;
}
