using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_movement_script : MonoBehaviour
{

    [SerializeField] GameObject Final_target_point;
    [SerializeField] GameObject player;
    [SerializeField] float camera_move_speed;
    [SerializeField] bool dynamic_camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FocusOnSamurai()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Final_target_point.activeSelf && Vector3.Distance(transform.position, Final_target_point.transform.position) > 5.0f && dynamic_camera) 
        {
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(Final_target_point.transform.position.x, Final_target_point.transform.position.y, transform.position.z), camera_move_speed);
            transform.position = Vector3.Lerp(transform.position, new Vector3(Final_target_point.transform.position.x, Final_target_point.transform.position.y, transform.position.z), Time.deltaTime * camera_move_speed);
        }
        else if (transform.position != player.transform.position) //  && player.GetComponent<Dash_script>().move_to_position == player.transform.position
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), Time.deltaTime * camera_move_speed);
        }
    }
}
