﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : SphereSurfaceObject {

    [SerializeField]
    float escapeSpeed_ = 90.0f;

    [SerializeField]
    float escapeRegion_ = 50.0f;

    [SerializeField]
    float minEscapeSec_ = 1.0f;

    [SerializeField]
    float escapeSecRange_ = 3.0f;

    public Human Human { set { human_ = value; } }

	// Use this for initialization
	void Start () {
        state_ = new Normal( this );
    }
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
        innerUpdate();
    }

    float calcDistFromHuman()
    {
        return ( human_.transform.position - transform.position ).magnitude;
    }


    class StateBase : State
    {
        public StateBase( Robot parent )
        {
            parent_ = parent;
        }
        protected Robot parent_;
    }

    // 通常
    class Normal : StateBase
    {
        public Normal(Robot parent) : base( parent ) { }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.cont_.setSpeed( parent_.speed_ );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // 近くにHumanがいたら逃走モードに
            if ( parent_.calcDistFromHuman() < parent_.escapeRegion_ )
                return new Escape( parent_ );
            return this;
        }
    }

    // 逃走
    class Escape : StateBase
    {
        public Escape(Robot parent) : base( parent ) { }

        // 内部初期化
        override protected void innerInit()
        {
            parent_.cont_.setSpeed( parent_.escapeSpeed_ );
            moveStates_[ 0 ] = moveLeft;
            moveStates_[ 1 ] = moveRight;
            moveStates_[ 2 ] = moveStraight;
            curState_ = moveStraight;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // Humanが離れたら通常に
            if ( parent_.calcDistFromHuman() >= parent_.escapeRegion_ )
                return new Normal( parent_ );

            // 指定時間追われ続けていると判断
            // 左右もしくはそのまま真っすぐ進むのいずれかを判断
            t_ -= Time.deltaTime;
            if ( t_ <= 0.0f ) {
                curState_ = moveStates_[ Random.Range( 0, 3 ) ];
                t_ = parent_.minEscapeSec_ + Random.Range( 0.0f, parent_.escapeSecRange_ );
            }

            curState_();

            return this;
        }


        void moveLeft()
        {
            var lr = -parent_.lrSpeed_ * Time.deltaTime;
            var tangent = parent_.transform.forward * parent_.speed_ + parent_.transform.right * lr;
            parent_.cont_.setDir( tangent );
        }

        void moveRight()
        {
            var lr = parent_.lrSpeed_ * Time.deltaTime;
            var tangent = parent_.transform.forward * parent_.speed_ + parent_.transform.right * lr;
            parent_.cont_.setDir( tangent );
        }

        void moveStraight()
        {
        }

        System.Action[] moveStates_ = new System.Action[ 3 ];
        System.Action curState_;
        float t_ = 0.0f;
    }

    Human human_;
    State state_;
}
