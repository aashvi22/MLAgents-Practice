using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Movement : Agent
{
    public Transform cube2;
    private int count;
    public Material winMaterial;
    public Material loseMaterial;
    public MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        count = 1200;
        float random = Random.Range(-10f, 10f);
        cube2.transform.localPosition = new Vector3(random, 0, random);
        float random2 = Random.Range(-10f, 10f);
        while (Mathf.Abs(random-random2)<=2) 
            random2 = Random.Range(-10f, 10f);
        transform.localPosition = new Vector3(random2, 0, random2);
        cube2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        
        cube2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        floorMeshRenderer.material = winMaterial;

        
        SetReward(1f*count);
        Debug.Log("count: " + count + " collided");
        EndEpisode(); 
        
    }
    //take the position of the other cube
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition); //all of the positions would be relative to the red cube
        sensor.AddObservation(cube2.transform.localPosition);
    }
    public override void OnActionReceived(float[] vectorAction)//the actions sent in (floats)
    {
        transform.Translate(new Vector3(vectorAction[0]*Time.deltaTime,0,vectorAction[1] * Time.deltaTime));
        count--;
        if (count <= 0)
        {
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
            SetReward(-1200f);
        }
    }
}
//mlagents-learn --force

//make the neural network a parameter to be dragged and dropped into the cube

