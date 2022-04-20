using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings"), Space(5)]
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float mouseSensitivityX = 2f;
    [SerializeField] float mouseSensitivityY = 2f;

    [Header("References"), Space(5)]
    [SerializeField] Transform playerCamera;

    CharacterController charController;

    float cameraRotationX = 0f;


    private void Awake()
    {
        charController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        Move();

        Rotation();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        charController.Move(movementSpeed * Time.deltaTime * move.normalized);
    }

    void Rotation()
    {
        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraRotationX -= mouse.y * mouseSensitivityY;

        cameraRotationX = Mathf.Clamp(cameraRotationX, -60f, 50f);

        playerCamera.localEulerAngles = Vector3.right * cameraRotationX;
        transform.Rotate(mouse.x * mouseSensitivityX * Vector3.up);
    }
}
