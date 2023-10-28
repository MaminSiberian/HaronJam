using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Haron
{
    public enum DirectionState
    {
        right, up, left, down 
    }

    public enum HaronBehavior
    {
        Floating, Attack, Dash,
    }

    public class HaronController : MonoBehaviour
    {

        //inspector
        [SerializeField] internal Transform areaAttack;
        [SerializeField] internal Transform pivotAttack;
        //[SerializeField] internal tra colliderAttack;
        [Range(0f, 3f)][SerializeField] internal float distanceAttack;
        [Range(0f, 1f)][SerializeField] internal float durationAttack;
        [Range(0f, 1f)][SerializeField] internal float cooldownAttack;

        [Range(0, 10)][SerializeField] internal int damage;
        [Range(0f, 1f)][SerializeField] private float timeCaiot;
        [Space(10)]
        [Range(0f, 10f)][SerializeField] internal float speedmove;
        [SerializeField] internal AnimationCurve acceleration;
        [SerializeField] internal AnimationCurve deaceleration;
        [Space]
        [SerializeField] internal AnimationCurve dashCurve;
        [Range(0f, 100f)][SerializeField] internal float forceDash;
        [Range(0f, 3f)][SerializeField] internal float cooldownDash;
        [Range(0f, 1f)][SerializeField] internal float durationDash;
        //debug
        [SerializeField] private DirectionState directionState;
        [SerializeField] private HaronBehavior state;
        [SerializeField] internal Vector2 directionMove;
        [SerializeField] internal Vector2 directionAttack;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] Vector2 velosity;
        [SerializeField] internal bool isAttacking = false;
        [SerializeField] internal bool isDash;
        [SerializeField] internal bool isReloadDash;
        [SerializeField] internal float currentTimeCooldawnDash;
        //local
        private Dictionary<Type, IHaronBehavior> behavioraMap;
        internal IHaronBehavior behaviorCurrent;

        public DirectionState DirectionState {  get => directionState; internal set => directionState = value; }
        public HaronBehavior State { get => state; internal set => state = value; }

        private void Start()
        {
            this.InitBehaviors();
            this.SetBehaviorDefault();
            rb = GetComponent<Rigidbody2D>();
        }

        private void InitBehaviors()
        {
            this.behavioraMap = new Dictionary<Type, IHaronBehavior>();
            this.behavioraMap[typeof(HaronFloatingBehavior)] = new HaronFloatingBehavior(this);
            this.behavioraMap[typeof(HaronAttackBehavior)] = new HaronAttackBehavior(this);
            this.behavioraMap[typeof(HaronDashBehavior)] = new HaronDashBehavior(this);
            //this.behavioraMap[typeof(HookAIMBehavior)] = new HookAIMBehavior(this);
            //this.behavioraMap[typeof(HookRotationBehavior)] = new HookRotationBehavior(this);
            //this.behavioraMap[typeof(HookCatcEmptyhBehavior)] = new HookCatcEmptyhBehavior(this);
            //this.behavioraMap[typeof(HookCatchPointBehavior)] = new HookCatchPointBehavior(this);
            //this.behavioraMap[typeof(HookCathcEnemyAndProjectileBehavior)] = new HookCathcEnemyAndProjectileBehavior(this);
            //this.behavioraMap[typeof(HookRotationWithObjectBehavior)] = new HookRotationWithObjectBehavior(this);
            //this.behavioraMap[typeof(HookThrowCaptureObject)] = new HookThrowCaptureObject(this);
            //this.behavioraMap[typeof(HookStunBehavior)] = new HookStunBehavior(this);
        }
        private void SetBehaviorDefault()
        {
            SetBehaviorFloating();
        }

        public void SetBehavior(IHaronBehavior newBehavior)
        {
            if (this.behaviorCurrent != null)
                this.behaviorCurrent.Exit();

            this.behaviorCurrent = newBehavior;
            this.behaviorCurrent.Enter();
        }



        private IHaronBehavior GetBehavior<T>() where T : IHaronBehavior
        {
            var type = typeof(T);
            return this.behavioraMap[type];
        }

        private void FixedUpdate()
        {
            behaviorCurrent?.UpdateBehavior();
            velosity = rb.velocity;
            CaiotTime();
            ResetCooldownDash();
        }

        private void ResetCooldownDash()
        {
            if (currentTimeCooldawnDash >= cooldownDash)
            {
                isReloadDash = true;
            }
            else
            {
                currentTimeCooldawnDash += Time.fixedDeltaTime;
            }
        }

        private void CaiotTime()
        {
            if (isAttacking)
            {
                Invoke("ResetIsActiveAttack", timeCaiot);
            }

            if (isDash)
            {
                Invoke("ResetIsActiveDash", timeCaiot);
            }
        }
        private void ResetIsActiveAttack()
        {
            isAttacking = false;
            
        }
        private void ResetIsActiveDash()
        {
            isDash = false;

        }

        public void SetBehaviorFloating()
        {
            var behavior = this.GetBehavior<HaronFloatingBehavior>();
            this.SetBehavior(behavior);
        }

        public void SetBehaviorAttack()
        {
            var behavior = this.GetBehavior<HaronAttackBehavior>();
            this.SetBehavior(behavior);
        }
        public void SetBehaviorDash()
        {
            var behavior = this.GetBehavior<HaronDashBehavior>();
            this.SetBehavior(behavior);
        }


        //public void SetBehaviorStan()
        //{
        //    var behavior = this.GetBehavior<HookStunBehavior>();
        //    this.SetBehavior(behavior);
        //}

        //public void SetBehaviorThrowCaptureObject()
        //{
        //    var behavior = this.GetBehavior<HookThrowCaptureObject>();
        //    this.SetBehavior(behavior);
        //}

        //public void SetBehaviorRotationWithObject()
        //{
        //    var behavior = this.GetBehavior<HookRotationWithObjectBehavior>();
        //    this.SetBehavior(behavior);
        //}

        //public void SetBehaviorCatchEnemyAndProjectile()
        //{
        //    var behavior = this.GetBehavior<HookCathcEnemyAndProjectileBehavior>();
        //    this.SetBehavior(behavior);
        //}

        //public void SetBehaviorCarchPoint()
        //{
        //    var behavior = this.GetBehavior<HookCatchPointBehavior>();
        //    this.SetBehavior(behavior);
        //}

        //public void SetBehaviorAIM()
        //{
        //    var behavior = this.GetBehavior<HookAIMBehavior>();
        //    this.SetBehavior(behavior);
        //}

        //public void SetBehaviorCatchEmpty()
        //{
        //    var behavior = this.GetBehavior<HookCatcEmptyhBehavior>();
        //    this.SetBehavior(behavior);
        //}


    }
}