using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Serialization;

//extend the XRBaseInteractable class
public class DoorHandle : XRBaseInteractable
    {
        //Make variables visible in Unity under "Door Handle Data" heading
        [Header("Door Handle Data")]
        public Transform draggedTransform;
        public Vector3 localDragDirection;
        public float dragDistance;

        //Door 'heaviness' index
        public int doorWeight = 20;


        //Make variables visible in Unity under "Visual References" heading
        [Header("Visual References")]
        public LineRenderer handleToHandLine;
        public LineRenderer dragVectorLine;

        private Vector3 m_StartPosition;
        private Vector3 m_EndPosition;
        private Vector3 m_WorldDragDirection;

        private void Start()
        {
            //set drag direction only in one plane throughout scene lifetime
            m_WorldDragDirection = transform.TransformDirection(localDragDirection).normalized;


            //assign start and end position of door
            m_StartPosition = draggedTransform.position;
            m_EndPosition = m_StartPosition + m_WorldDragDirection * dragDistance;

            //have lines off at start 
            handleToHandLine.gameObject.SetActive(false);
            dragVectorLine.gameObject.SetActive(false);
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (isSelected)
            {
                var interactorTransform = firstInteractorSelecting.GetAttachTransform(this);

                //get vector from handle to interactor
                Vector3 selfToInteractor = interactorTransform.position - transform.position;

                //calulate the component of pulling force in direction of door movement plane
                float forceInDirectionOfDrag = Vector3.Dot(selfToInteractor, m_WorldDragDirection);

                
                //drag direction check(positive or negative)
                bool dragToEnd = forceInDirectionOfDrag > 0.0f;

                //take absolute value for speed of door calc
                float absoluteForce = Mathf.Abs(forceInDirectionOfDrag);

                //force into speed calc. include door weight index
                float speed = absoluteForce / Time.deltaTime / doorWeight;

                //move target towards start or end at calculated speed
                draggedTransform.position = Vector3.MoveTowards(draggedTransform.position,
                    dragToEnd ? m_EndPosition : m_StartPosition,
                    speed * Time.deltaTime);

                //set both points for line
                handleToHandLine.SetPosition(0, transform.position);
                handleToHandLine.SetPosition(1, interactorTransform.position);

                dragVectorLine.SetPosition(0, transform.position);
                dragVectorLine.SetPosition(1, transform.position + forceInDirectionOfDrag * m_WorldDragDirection);

            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 worldDirection = transform.TransformDirection(localDragDirection);
            //turn into unit vector to be multiplied by drag distance
            worldDirection.Normalize();

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + worldDirection * dragDistance);
        }

        //turn lines on when handle is selected
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            handleToHandLine.gameObject.SetActive(true);
            dragVectorLine.gameObject.SetActive(true);
        }

        //turn lines off when deselected
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            handleToHandLine.gameObject.SetActive(false);
            dragVectorLine.gameObject.SetActive(false);
        }
    }