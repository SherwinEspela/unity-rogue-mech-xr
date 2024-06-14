using UnityEngine;
using UnityEngine.InputSystem;

public class DummyPlayer : PlayerCharacter
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] float speedMult = 2.0f;

    InputAction inputMove;
    float currentX;
    float currentY;
    float currentZ;

    public void Init()
    {
        currentX = transform.position.x;
        currentZ = transform.position.z;
        currentY = transform.position.y;

        if (playerInput)
        {
            inputMove = playerInput.actions.FindAction("Move");
        }
    }

    private void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        if (inputMove == null) return;
        var direction = inputMove.ReadValue<Vector2>();
        currentX += direction.x * speedMult * Time.deltaTime;
        currentZ += direction.y * speedMult * Time.deltaTime;
        transform.position = new Vector3(currentX , currentY, currentZ);
    }
}
