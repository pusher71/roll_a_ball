using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool canMove = false; //может ли игрок двигаться
    public GameObject controller; //ссылка на контроллер
    private ControllerScript controllerScript; //скрипт контроллера

    public float speed = 0;

    private Rigidbody rb;
    private float movementX;
    private float movementY;

    private List<GameObject> pickUps; //встретившиеся лакомства

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerScript = controller.GetComponent<ControllerScript>();
        pickUps = new List<GameObject>();
    }

    //восстановить все лакомства
    public void restorePickUps()
    {
        foreach (GameObject go in pickUps)
            go.SetActive(true);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        if (canMove)
            rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            pickUps.Add(other.gameObject);
            other.gameObject.SetActive(false);
            controllerScript.incrementCount();
        }
    }
}
