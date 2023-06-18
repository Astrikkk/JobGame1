using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform leftColumn; // ��������� �� ���� �����
    public Transform rightColumn; // ��������� �� ������ �����
    public GameObject blockPrefab; // ������ �����
    public float columnSpeed = 2f; // �������� ���� ������
    public float blockSpeed = 5f; // �������� ���� �����
    public float blockSpawnRate = 1f; // ������� ����� �����
    public int PlusHeightRight;
    public int PlusHeightLeft;

    private bool isOnLeftColumn = true; // �����, �� �����, �� �������� ����������� �� ����� �����
    private GameObject cam;

    private float jumpDuration = 0.5f; // ��������� ������
    private bool BlockPlace = true;

    private void Start()
    {
        InvokeRepeating("SpawnBlock", 0f, blockSpawnRate);
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        cam.transform.Translate(Vector3.up * columnSpeed * Time.deltaTime);
    }

    private void MoveColumns()
    {
        if (leftColumn.position.y >= rightColumn.position.y)
        {
            rightColumn.position = new Vector3(rightColumn.position.x, leftColumn.position.y);
        }
        else
        {
            leftColumn.position = new Vector3(leftColumn.position.x, rightColumn.position.y);
        }
    }

    private void SpawnBlock()
    {
        float randomX;
        Vector3 targetPosition;

        if (BlockPlace)
        {
            randomX = 5;
            targetPosition = new Vector3(leftColumn.position.x, leftColumn.position.y + 2f + PlusHeightLeft);
            BlockPlace = false;
            PlusHeightLeft += 1;
        }
        else
        {
            randomX = -5;
            targetPosition = new Vector3(rightColumn.position.x, rightColumn.position.y + 2f + PlusHeightRight);
            BlockPlace = true;
            PlusHeightRight += 1;
        }

        GameObject block = Instantiate(blockPrefab, new Vector3(randomX, leftColumn.position.y+1 + PlusHeightRight, 0f), Quaternion.identity);
        block.GetComponent<BlockController>().MoveToTarget(targetPosition, blockSpeed);
    }

    private void Jump()
    {
        Vector3 targetPosition;

        if (!isOnLeftColumn)
        {
            targetPosition = new Vector3(rightColumn.position.x, transform.position.y + 2);
            isOnLeftColumn = true;
        }
        else
        {
            targetPosition = new Vector3(leftColumn.position.x, transform.position.y + 2);
            isOnLeftColumn = false;
        }

        StartCoroutine(JumpRoutine(targetPosition));
    }

    private IEnumerator JumpRoutine(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}