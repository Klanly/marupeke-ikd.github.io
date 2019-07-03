using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideCallback : MonoBehaviour {
    public System.Action Callback { set { callback_ = value; } get { return callback_; } }
    System.Action callback_;
}
