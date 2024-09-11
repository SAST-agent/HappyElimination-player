using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float bias;
    public Vector3 direction;
    float moveTime;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < moveTime)
        {
            transform.position += direction * Time.deltaTime;
            timer += Time.deltaTime;
        }
    }

    public void SetMove(Vector3 targetDirection, float moveTotalTime)
    {
        direction = targetDirection.normalized * bias;
        moveTime = moveTotalTime;
        timer = 0;
    }
}
