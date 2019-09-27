using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パーティクルエミッター
//  登録したパーティクルを作成して放出する

public class ParticleEmitter : MonoBehaviour
{
    [SerializeField]
    List<Particle> particlePrefabs_ = new List<Particle>();

    private void Awake() {
        foreach ( var s in particlePrefabs_ ) {
            particles_[ s.name ] = s;
        }
    }

    public Particle emit( string name ) {
        if ( particles_ .ContainsKey( name ) == false )
            return null;
        return PrefabUtil.createInstance( particles_[ name ] );
    }

    Dictionary<string, Particle> particles_ = new Dictionary<string, Particle>();
}
