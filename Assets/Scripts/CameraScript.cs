using UnityEngine;
using System.Collections;


namespace camera
{
    public class CameraScript : MonoBehaviour
    {
        public Transform target1;
        public Transform target2;

        private int activeCamera;

        void Start()
        {
            activeCamera = 1;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                switch (activeCamera)
                {
                    case 1: activeCamera = 2;
                        break;
                    case 2: activeCamera = 1;
                        break;
                    default:
                        break;
                }
            }

            if (activeCamera == 1)
            {
                transform.position = new Vector3(target1.position.x, target1.position.y, transform.position.z);
            }
            else if(activeCamera == 2)
            {
                transform.position = new Vector3(target2.position.x, target2.position.y, transform.position.z);
            }
        }

    }
}