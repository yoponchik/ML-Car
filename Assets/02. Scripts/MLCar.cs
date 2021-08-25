using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MLCar : Agent
{

    private Transform tr;
    private Rigidbody rb;

    public float moveSpeed = 1.5f;
    public float turnSpeed = 100.0f;

    private Renderer offTrackRd;
    private Material originalMt;

    public Material continueMt, goodMt, badMt;

    public GameObject[] checkPoints;
    public GameObject[] lastCheckPoints;
    public GameObject finishLine;

    private int checkPointCount;


    // Start is called before the first frame update
    public override void Initialize()
    {
        finishLine.SetActive(false);
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        offTrackRd = tr.parent.Find("Off Track").GetComponent<Renderer>();
        originalMt = offTrackRd.material;

        MaxStep = 200000;
    }

    public override void CollectObservations(VectorSensor sensor)
    {

    }
    //float direction = Vector3.Dot(transform.forward, checkPoints[currentCheckPoint].forward);
    //addreward(-1/(float) MaxStep);

    public override void OnEpisodeBegin()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
        //tr.localPosition = new Vector3(-211.705f, 164.634f, -130.857f);
        tr.localPosition = new Vector3(-211.705f, 164.634f, -13.5f);
        tr.localRotation = Quaternion.identity;

        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].SetActive(true);
        }

        for (int i = 0; i < lastCheckPoints.Length; i++)
        {
            lastCheckPoints[i].SetActive(false);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions; // might need to change this
        //Debug.Log($"[0]={action[0]}, [1]={action[1]}");
        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }

        switch (action[1])
        {
            case 1: rot = -tr.up; break;
            case 2: rot = tr.up; break;
        }

        tr.Rotate(rot, Time.fixedDeltaTime * turnSpeed);
        rb.AddForce(dir * moveSpeed, ForceMode.VelocityChange);

        //make agent prefer continuous motion due to minus penalty
        AddReward(-1 / (float)MaxStep); //5000 -> 0.005

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        //Branch 0 - move (stop/forward/back) 0,1,2: size 3
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1; //forward
        }

        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2; //back
        }

        //Branch 1- rotate (stop/left/right) 0,1,2: size 3
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1; //turnleft
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2; //turn right
        }
    }

    IEnumerator RevertMaterial(Material changedMt)
    { // blinks color
        offTrackRd.material = changedMt;
        yield return new WaitForSeconds(0.2f);
        offTrackRd.material = originalMt;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CHECKPOINT"))
        {
            StartCoroutine(RevertMaterial(goodMt));
            AddReward(+0.2f);
            checkPointCount++;
            print("checkPoint");
            print(checkPointCount);
            other.gameObject.SetActive(false);

            if (other.gameObject.name == "CheckPointTrigger")
            {
                for (int i = 0; i < lastCheckPoints.Length; i++)
                {
                    lastCheckPoints[i].SetActive(true);
                }
            }

            if (other.gameObject.name == "Last CheckPoint")
            {
                finishLine.SetActive(true);
            }
        }


        if (other.gameObject == finishLine)
        {
            checkPointCount = 0;
            AddReward(+1.0f);
            EndEpisode();
            print("finishline");
        }

        if (other.gameObject.CompareTag("OFFTRACK"))
        {
            StartCoroutine(RevertMaterial(badMt));
            AddReward(-10.0f);
            EndEpisode();
            checkPointCount = 0;
            print("offtrack");
        }

    }


}