using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    PaiGenerator paiGenerator_;
    public PaiGenerator PaiGenerator { get { return paiGenerator_; } }

    [SerializeField]
    Field field_;
    public Field Field { get { return field_; } }

    public static GameManager getInstance() {
        return manager_g;
    }

    private void Awake() {
        manager_g = this;
    }

    private void OnDestroy() {
        manager_g = null;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static GameManager manager_g = null;
}
