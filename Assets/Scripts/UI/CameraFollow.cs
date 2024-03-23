using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private float smoothTime = 0.25f;
    // Minimum distance of camera from scene
    [SerializeField] private float min_offset = -10f;
    [SerializeField] private float z_offset = -10f;

    [SerializeField] private float player_bound = 0.25f;
    [SerializeField] private float sub_bound_x = 0.25f;
    [SerializeField] private float sub_bound_y = 0.25f;

    private Vector3 currentVelocity = Vector3.zero;

    // Used to store information about objects that must be kept in view of camera
    public struct ViewObject {
        public ViewObject(Vector3 pos, float x, float y){
            position = pos;
            x_bound = x;
            y_bound = y;
        }

        public ViewObject(Vector3 pos, float xy){
            position = pos;
            x_bound = xy;
            y_bound = xy;
        }

        public Vector3 position;
        public float x_bound;
        public float y_bound;
    }

    private void LateUpdate() {
        // Initialise camera target to submarine
        Vector3 submarine_pos = GameObject.FindGameObjectWithTag("Submarine").GetComponent<Transform>().position;
        Vector3 target = submarine_pos;
        z_offset = min_offset;
        GameObject [] players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length > 0) {
            ViewObject submarine_obj = new ViewObject(submarine_pos, sub_bound_x, sub_bound_y);
            // Populate list of view objects
            List<ViewObject> viewObjects = new List<ViewObject>{submarine_obj};
            foreach(GameObject player in players) {
                viewObjects.Add(new ViewObject(player.GetComponent<Transform>().position, player_bound));
            }

            // Calculate furthest X and Y distances between any two view objects and record them
            float max_x = 0f;
            (ViewObject left, ViewObject right) max_x_obj = (submarine_obj, submarine_obj);
            float max_y = 0f;
            (ViewObject down, ViewObject up) max_y_obj = (submarine_obj, submarine_obj);
            for(int i=0; i<viewObjects.Count; i++) {
                ViewObject objA = viewObjects[i];

                for(int j=i; j<viewObjects.Count; j++) {
                    ViewObject objB = viewObjects[j];
    
                    float x_dis = Mathf.Abs(objA.position.x - objB.position.x) + objA.x_bound + objB.x_bound;
                    float y_dis = Mathf.Abs(objA.position.y - objB.position.y) + objA.y_bound + objB.y_bound;

                    if(x_dis > max_x) {
                        max_x = x_dis;
                        // Check which object is left/right of the other
                        if(objA.position.x < objB.position.x)
                            max_x_obj = (objA, objB);
                        else
                            max_x_obj = (objB, objA);
                    }
                    if(y_dis > max_y) {
                        max_y = y_dis;
                        // Check which object is below/above the other
                        if(objA.position.y < objB.position.y)
                            max_y_obj = (objA, objB);
                        else
                            max_y_obj = (objB, objA);
                    }
                }
            }
            // Debug.Log("max_x: " + max_x + ", max_x_pair: " + max_x_obj.left.position + ", " + max_x_obj.right.position);
            // Debug.Log("max_y: " + max_y + ", max_y_pair: " + max_y_obj.down.position + ", " + max_y_obj.up.position);

            // Update target to centre of screen depending on objects with furthest X/Y separation
            target.x = (max_x_obj.left.position.x-max_x_obj.left.x_bound + max_x_obj.right.position.x+max_x_obj.right.x_bound) / 2;
            target.y = (max_y_obj.down.position.y-max_y_obj.down.y_bound + max_y_obj.up.position.y+max_y_obj.up.y_bound) / 2;

            // Calculate potential z-offset for x and y max distances
            float z_offset_x = -(max_x/2 / Mathf.Tan(Mathf.Deg2Rad * Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect)/2));
            float z_offset_y = -(max_y/2 / Mathf.Tan(Mathf.Deg2Rad * cam.fieldOfView/2));

            // Calculate which z_offset dominates
            z_offset = Mathf.Min(z_offset_x, z_offset_y);
            z_offset = Mathf.Min(z_offset, min_offset);
        }

        // Calculate target position
        target.z = z_offset;
        // Animate camera movement to target position, dampened by 'smoothTime'
        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime);
    }
}
