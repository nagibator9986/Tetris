using MalbersAnimations.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MalbersAnimations.Utilities
{
    /// <summary>For when someone with LookAt enters it will set this transform as the target</summary>
    public class AimTarget : MonoBehaviour, IAimTarget
    {
        /// <summary>This will set AutoAiming for the Aim Logic</summary>
        [SerializeField,Tooltip("This will set AutoAiming for the Aim Logic")] 
        private bool aimAssist;

        [Tooltip("Invoke when This Target is been hit by the Aim Ray of the Aim Compnent")] 
        public BoolEvent OnAimEnter = new BoolEvent();

        /// <summary>This will set AutoAiming for the Aim Logic</summary>
        public bool AimAssist { get => aimAssist; set => aimAssist = value; }

        /// <summary>Is the target been aimed by the Aim Ray of the Aim Script</summary>
        public void IsBeenAimed(bool enter)
        { OnAimEnter.Invoke(enter); }

        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return; //Ignore if the Collider entering is a Trigger

            IAim Aimer = other.GetComponentInParent<IAim>();

            if (Aimer != null)
            {
                Aimer.AimTarget = transform;
                OnAimEnter.Invoke(true);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;                //Ignore if the Collider exiting is a Trigger

            IAim Aimer = other.GetComponentInParent<IAim>();

            if (Aimer != null)
            {
                Aimer.AimTarget = null;
                OnAimEnter.Invoke(false);
            }
        }
    }
}