using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PathFindingDebug : MonoBehaviour {
    [SerializeField] private int fieldSize = 30;
    [SerializeField] private int startX = 2;
    [SerializeField] private int startY = 2;
    [SerializeField] private int goalX = 13;
    [SerializeField] private int goalY = 13;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;

    private Dictionary<Integer2, GameObject> cells = new Dictionary<Integer2, GameObject>();

    private void Start() {
        DrawField();
        DrawPath();
    }

    private void DrawField() {
        for (int x = 0; x < fieldSize; x++) {
            for (int y = 0; y < fieldSize; y++) {
                GameObject cell = Instantiate(cellPrefab, cellParent);
                RectTransform rectCell = cell.GetComponent<RectTransform>();
                Image imageCell = cell.GetComponent<Image>();

                if (startX == x && startY == y) imageCell.color = Color.blue;
                if (goalX == x && goalY == y) imageCell.color = Color.green;

                rectCell.anchoredPosition = new Vector2(x * rectCell.sizeDelta.x, y * rectCell.sizeDelta.y);

                cells[new Integer2(x,y)] = cell;
            }
        }
    }

    private void DrawPath() {
        int[,] field = new int[fieldSize, fieldSize];
        for (int x = 0; x < fieldSize; x++) {
            for (int y = 0; y < fieldSize; y++) {
                field[x, y] = 0;
            }
        }

        List<Integer2> path = SwiftStar.FindPath(field, new Integer2(startX, startY), new Integer2(goalX, goalY));
        foreach (Integer2 p in path) {
            GameObject cell = cells.Where(x => x.Key.Equals(p)).FirstOrDefault().Value;
            if (cell != null && !(startX == p.x && startY == p.y) && !(goalX == p.x && goalY == p.y)) {
                cell.GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
