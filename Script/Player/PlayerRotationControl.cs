using UnityEngine;

public class PlayerRotationControl : MonoBehaviour
{
    private Camera mainCamera; // ���콺 ��ġ�� ����� ī�޶� (�ַ� ���� ī�޶�)

    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform lookTransform;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Animator animator;

    private bool isFlip;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        animator.SetBool("Swim", _inputReader.Movement != Vector2.zero);

        if (_inputReader.Movement != Vector2.zero)
        {
            visualTransform.rotation = Quaternion.Lerp(
                visualTransform.rotation, Quaternion.LookRotation(Vector3.forward, _inputReader.Movement.normalized) * Quaternion.Euler(0, 0, 90f), Time.deltaTime * 8);

            Vector3 scale = visualTransform.localScale;
            scale.y = visualTransform.right.x > 0 ? 1 : -1;
            visualTransform.localScale = scale;
            Vector3 lookTrmScale = lookTransform.localScale;
            lookTrmScale.y = visualTransform.right.x > 0 ? 1 : -1;
            lookTransform.localScale = lookTrmScale;
        }

        LookRotation();
    }

    private void LookRotation()
    {
        // ���콺 ��ġ ��������
        Vector3 mousePosition = Input.mousePosition;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane+15));
        worldMousePosition.z = transform.position.z; // ������� ������ Z�� ����

        // ���� ���
        Vector3 direction = worldMousePosition - transform.position;


        lookTransform.transform.right = direction;
    }
}
