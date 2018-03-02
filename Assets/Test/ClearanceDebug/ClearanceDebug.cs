using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClearanceDebug : MonoBehaviour {
    [SerializeField] private int fieldSize = 30;

    [SerializeField] private List<int> wallX;
    [SerializeField] private List<int> wallY;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;

    [SerializeField] private int agentSize;

    private Dictionary<Integer2, GameObject> cells = new Dictionary<Integer2, GameObject>();

    private void Start() {
        DrawField();
        DrawClearance();
    }

    private void DrawField() {
        for (int x = 0; x < fieldSize; x++) {
            for (int y = 0; y < fieldSize; y++) {
                GameObject cell = Instantiate(cellPrefab, cellParent);
                RectTransform rectCell = cell.GetComponent<RectTransform>();
                Image imageCell = cell.GetComponent<Image>();

                for (int i = 0; i < wallX.Count; i++) {
                    if (wallX[i] == x && wallY[i] == y) {
                        cell.transform.GetChild(0).gameObject.SetActive(false);
                        imageCell.color = Color.grey;
                        break;
                    }
                }

                rectCell.anchoredPosition = new Vector2(x * rectCell.sizeDelta.x, y * rectCell.sizeDelta.y);

                cells[new Integer2(x, y)] = cell;
            }
        }
    }

    private void DrawClearance() {
        int[,] field = new int[fieldSize, fieldSize];
        int[,] clearanceField = new int[fieldSize, fieldSize];

        for (int x = 0; x < fieldSize; x++) {
            for (int y = 0; y < fieldSize; y++) {
                bool blocked = false;

                for (int i = 0; i < wallX.Count; i++) {
                    if (wallX[i] == x && wallY[i] == y) {
                        field[x, y] = 1;
                        blocked = true;
                        break;
                    }
                }

                if (!blocked) {
                    field[x, y] = 0;
                }
            }
        }

        clearanceField = SwiftStar.CalculateClearance(field, agentSize);

        for (int x = 0; x < fieldSize; x++) {
            for (int y = 0; y < fieldSize; y++) {
                GameObject cell = cells.Where(c => c.Key.Equals(new Integer2(x,y))).FirstOrDefault().Value;
                if (cell != null) {
                    cell.transform.GetChild(0).GetComponent<Text>().text = clearanceField[x, y].ToString();
                }
            }
        }
    }
}
