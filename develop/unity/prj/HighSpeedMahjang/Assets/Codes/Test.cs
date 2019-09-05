using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mahjang;

public class Test : MonoBehaviour
{
    [SerializeField]
    bool bCheck_ = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    void check() {
        Pai[] pais = new Pai[] {
            new Pai( 27 ),
            new Pai( 28 ),
            new Pai( 29 ),
            new Pai( 30 ),
            new Pai( 31 ),
            new Pai( 32 ),
            new Pai( 33 ),
            new Pai( 0 ),
            new Pai( 8 ),
            new Pai( 9 ),
            new Pai( 17 ),
            new Pai( 18 ),
            new Pai( 26 ),
            new Pai( 26 ),
        };
        PaiGroup group = new PaiGroup();
        var type = group.set( pais, false );

		Pais tehai = new Pais();
		var ankous = new List<Pai>() {
            new Pai( 27 ),
            new Pai( 28 ),
            new Pai( 29 ),
            new Pai( 30 ),
            new Pai( 31 ),
            new Pai( 32 ),
            new Pai( 33 ),
            new Pai( 0 ),
            new Pai( 8 ),
            new Pai( 9 ),
            new Pai( 17 ),
            new Pai( 18 ),
            new Pai( 26 ),
            new Pai( 26 ),
        };
        var minkous = new List<PaiGroup>();
        ankous[ 12 ].Agari = true;
        var calc = new MahjangScoreCalculator();
        calc.analyze( ankous, minkous, false );
    }

    // Update is called once per frame
    void Update()
    {
        if ( bCheck_ == true ) {
            bCheck_ = false;
            check();
        }
    }
}
