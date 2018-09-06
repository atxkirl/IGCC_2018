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
    //[SerializeField]
    //GameObject line;
    [SerializeField]
    Animator animator;

    //public ArduinoSerialMsgReader ArduinoSerialMsgReader;
    LineRenderer lineRenderer;
    Vector3 target;
    private bool jump;
    private bool fall;
    private bool slide;
    public float initialHeight;
    private float timer;
	private float timer2;
    // Use this for initialization
    void Start()
    {
        target.Set(0, 0, 0);
        initialHeight = transform.position.y;
        jump = false;
        fall = false;
        slide = false;
        timer = 0;
		timer2 = 0;
		//lineRenderer = line.GetComponent<LineRenderer>();

	}

    // Update is called once per frame
    void Update()
    {
		//RaycastHit hit;
		//Ray ray = new Ray(new Vector3(transform.position.x, 5, transform.position.z + 3), Vector3.back);
		//for(float i =5; i<40; i++)
		//{
		//	ray = new Ray(new Vector3(transform.position.x, i, transform.position.z + 3), Vector3.back);
		//	if (!Physics.Raycast(ray, out hit) || hit.distance > 3)
		//	{
		//		initialHeight = i + 2;
		//		transform.localPosition = new Vector3(transform.position.x, initialHeight, transform.position.z);
		//		Debug.Log("sadsda");
		//		break;
		//	}
		//}
		//if (Physics.Raycast(ray, out hit))
		//{
		//	initialHeight = 50 - hit.distance;
		//	//transform.localPosition = new Vector3(transform.position.x, initialHeight, transform.position.z);
		//	Debug.Log("sadsda");
		//}
		//timer2 += Time.deltaTime;
		//if (timer2 > 0.1)
		//{
		for (int i = 0; i < GameObject.FindGameObjectsWithTag("Floor").Length; i++)
		{
			float length = GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<MeshFilter>().mesh.bounds.size.x;
			if (GameObject.FindGameObjectsWithTag("Floor")[i].transform.position.x - transform.position.x >= -(length / 2)
				&& GameObject.FindGameObjectsWithTag("Floor")[i].transform.position.x - transform.position.x <= (length / 2))
			{
				bool check = false;
				List<Vector3> points = new List<Vector3>();

				GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<MeshFilter>().mesh.GetVertices(points);
				Debug.Log(points);
				int kev1 = 0;
				int kev2 = 0;
				//float length = GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<MeshFilter>().mesh.bounds.size.x;
				float some = length - ((GameObject.FindGameObjectsWithTag("Floor")[i].transform.position.x - transform.position.x)) / (length / points.Count);
				for (int j = (int)some; j < points.Count; j++)
				{

					float X = (GameObject.FindGameObjectsWithTag("Floor")[i].transform.position.x - (length / 2)) + ((length / points.Count) * j);
					//Debug.Log(X);
					if (X >= transform.position.x - 0.2
						&& X <= transform.position.x + 0.2 && points[j].y > 0)
					{
						initialHeight = points[j].y + 2;// + (GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<MeshFilter>().mesh.bounds.size.y/ 2);
						check = true;
						timer2 = 0f;
						Debug.Log(initialHeight);
						if (initialHeight < 5)
						{

						}
						//target.y += initialHeight - transform.position.y;
						if (!jump && !fall)
						{
							transform.localPosition = new Vector3(transform.position.x, initialHeight, transform.position.z);
						}
						break;

					}

				}
				points.Clear();
				if (check)
				{
					break;
				}
			}

		}
		//}
		if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idle") && !jump)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if(jump || fall)
        {
			Debug.Log("hello");
            transform.localScale = new Vector3(6, 6, 1);
        }
        else
        {
            transform.localScale = new Vector3(5, 5, 1);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            //animator.SetBool("Change", !animator.GetBool("Change"));
        }
        if (!jump && !fall)
        {
			//Vector3 temp = new Vector3();
			//temp.Set(transform.position.x, initialHeight, transform.position.z);
			//transform.position = temp;
			//transform.localPosition = new Vector3(transform.position.x, initialHeight, transform.position.z);
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
                transform.GetChild(0).gameObject.SetActive(false);
                jump = true;
                animator.SetBool("Jump", jump);
            }

        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idle"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if (tapController.GetDoubleTap())
        {
            if (!jump && !fall && !slide)
            {
                Debug.Log("Adrian2");
                slide = true;
                animator.SetBool("Slide", slide);
                transform.GetChild(0).gameObject.SetActive(false);
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
                animator.SetBool("Jump", jump);
                animator.SetBool("Fall", fall);
                //animator.SetBool("Change", !animator.GetBool("Change"));
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
                //Quaternion target = Quaternion.Euler(0, 0, 30);
                //transform.rotation = target;
            }
            else
            {
                Quaternion target = Quaternion.Euler(0, 0, 0);
                transform.rotation = target;
                timer = 0;
                slide = false;
                animator.SetBool("Slide", slide);
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        transform.position += target * speed * Time.deltaTime;
        if (transform.position.y <= initialHeight && fall)
        {
            fall = false;

            animator.SetBool("Fall", fall);
            animator.SetBool("Change", !animator.GetBool("Change"));
            //transform.GetChild(0).gameObject.SetActive(true);
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idle") && !jump)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idle") && !jump)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

