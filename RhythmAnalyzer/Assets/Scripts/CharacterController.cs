using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    TapControllerInputModule tapController;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float slideTimer;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    GameObject line;
    [SerializeField]
    Animator animator;

    //public ArduinoSerialMsgReader ArduinoSerialMsgReader;
    LineRenderer lineRenderer;
    Vector3 target;
    private bool jump;
    private bool fall;
    private bool slide;
    private float initialHeight;
    private float timer;
    // Use this for initialization
    void Start()
    {
        target.Set(0, 0, 0);
        initialHeight = transform.position.y;
        jump = false;
        fall = false;
        slide = false;
        timer = 0;
        lineRenderer = line.GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //ArduinoSerialMsgReader.Instance.onAccelerometerMsgRecieved += Test;
        Vector3[] linePoints = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePoints);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            if (linePoints[i].x >= transform.position.x - 0.5
                && linePoints[i].x <= transform.position.x + 0.5)
            {
                initialHeight = linePoints[i].y + 2;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Change", !animator.GetBool("Change"));
        }
        if (!jump && !fall)
        {
            Vector3 temp = new Vector3();
            temp.Set(transform.position.x, initialHeight, transform.position.z);
            transform.position = temp;
        }
        target.Set(0, 0, 0);
        if (Test.Instance.tap)
        {
            tapController.InputTap();
            Test.Instance.tap = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tapController.InputTap();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            target.Set(transform.position.x + 1, transform.position.y, transform.position.z);
            target -= transform.position;

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            target.Set(transform.position.x - 1, transform.position.y, transform.position.z);
            target -= transform.position;
        }
        if (tapController.GetTap())
        {
            initialHeight = transform.position.y;
            Debug.Log("Adrian1");
            if (!jump && !fall && !slide)
            {
                jump = true;
            }

        }
        if (tapController.GetDoubleTap())
        {
            if (!jump && !fall && !slide)
            {
                Debug.Log("Adrian2");
                slide = true;
            }
        }
        if (tapController.GetTripleTap())
        {
            Debug.Log("Adrian3");
        }
        if (jump)
        {
            if ((transform.position.y + 1) < initialHeight + jumpHeight)
            {
                target.y += 1;
            }
            else
            {
                jump = false;
                fall = true;
            }
        }
        if (fall)
        {
            target.y -= 1;
        }
        if (slide)
        {
            if (timer < slideTimer)
            {
                timer += Time.deltaTime;
                Quaternion target = Quaternion.Euler(0, 0, 30);
                transform.rotation = target;
            }
            else
            {
                Quaternion target = Quaternion.Euler(0, 0, 0);
                transform.rotation = target;
                timer = 0;
                slide = false;
            }
        }
        transform.position += target * speed * Time.deltaTime;
        if (transform.position.y <= initialHeight && fall)
        {
            fall = false;
        }
    }
}

