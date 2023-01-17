using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 
using TMPro; //To use TextMeshPro library and display scanner information



//extend the XRGrabInteractable class
public class Scanner : XRGrabInteractable
{

    //Make variables visible in Unity under "Scanner Data" heading
    [Header("Scanner Data")]
    public Animator animator;
    public LineRenderer laserRenderer;
    public TextMeshProUGUI targetName;
    public TextMeshProUGUI targetPosition;

    //grouped scanner functionality switch
    private void ScannerActivated (bool isActivated)
    {
        laserRenderer.gameObject.SetActive(isActivated);
        targetName.gameObject.SetActive(isActivated);
        targetPosition.gameObject.SetActive(isActivated);
    }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args); //base. used to call parent method to retain other functionality
        animator.SetBool("Opened", true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        animator.SetBool("Opened", false);
    }

    //turns all scanner functionality off at start of scene as default
    protected override void Awake() 
    {
        base.Awake();
        ScannerActivated(false);
    }

    //activate scanner functionality when trigger pressed
    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        ScannerActivated(true);

    }

    //deactivate scanner functionality when trigger released
    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        ScannerActivated(false);
    }

    //Scanner display information
    private void ScanForObjects() 
    {
        RaycastHit hit;
        Vector3 worldHit = laserRenderer.transform.position + laserRenderer.transform.forward * 1000.0f; //laser endpoint if no collisions

    //check if laser is hitting any objects and assigns targetName and targetPosition with the objects name and position
        if (Physics.Raycast(laserRenderer.transform.position, laserRenderer.transform.forward, out hit)) 
        {
            worldHit = hit.point; //set laser endpoint to hit location to prevent passing through objects

            targetName.SetText(hit.collider.name);
            targetPosition.SetText(hit.collider.transform.position.ToString());
        }
        //set second point on line
        laserRenderer.SetPosition(1, laserRenderer.transform.InverseTransformPoint(worldHit));
    }

    //Scan for objects every frame while laser is active
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (laserRenderer.gameObject.activeSelf) 
        {
            ScanForObjects();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
