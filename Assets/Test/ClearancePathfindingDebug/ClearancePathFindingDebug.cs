using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClearancePathFindingDebug : MonoBehaviour {
    [SerializeField] private int fieldSize = 30;
    [SerializeField] private int startX = 2;
    [SerializeField] private int startY = 2;
    [SerializeField] private int goalX = 13;
    [SerializeField] private int goalY = 13;

    [SerializeField] private List<int> wallX;
    [SerializeField] private List<int> wallY;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;

    [SerializeField] private BotController botController;
    [SerializeField] private int agentSize;
    [SerializeField] private float agentSpeed = 7F;

    private Dictionary<Integer2, GameObject> cells = new Dictionary<Integer2, GameObject>();

    private void Start() {
        botController.Init(agentSize);
        botController.ForceMove(startX, startY);

        DrawField();
        DrawPath();
    }

    private void DrawField() {
        for (int x = 0; x < fieldSize; x++) {
            for (int y = 0; y < fieldSize; y++) {
                GameObject cell = Instantiate(cellPrefab, cellParent);
                RectTransform rectCell = cell.GetComponent<RectTransform>();
                Image imageCell = cell.GetComponent<Image>();

                if (goalX == x && goalY == y) imageCell.color = Color.green;

                for (int i = 0; i < wallX.Count; i++) {
                    if (wallX[i] == x && wallY[i] == y) {
                        imageCell.color = Color.grey;
                        break;
                    }
                }

                rectCell.anchoredPosition = new Vector2(x * rectCell.sizeDelta.x, y * rectCell.sizeDelta.y);

                cells[new Integer2(x,y)] = cell;
            }
        }
    }

    private void DrawPath() {
        int[,] field = new int[fieldSize, fieldSize];
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

        List<Integer2> path = SwiftStar.FindPath(field, new Integer2(startX, startY), new Integer2(goalX, goalY), agentSize);
        foreach (Integer2 p in path) {
            GameObject cell = cells.Where(x => x.Key.Equals(p)).FirstOrDefault().Value;
            if (cell != null && !(startX == p.x && startY == p.y) && !(goalX == p.x && goalY == p.y)) {
                cell.GetComponent<Image>().color = Color.cyan;
            }
        }

        DrawMove(path);
    }

    private void DrawMove(List<Integer2> path) {
        StopAllCoroutines();
        StartCoroutine(IEDrawMove(path));
    }

    private IEnumerator IEDrawMove(List<Integer2> path) {
        for (int i = 0; i < path.Count; i++) {
            yield return botController.Move(path[i].x, path[i].y, agentSpeed);
        }
        yield return null;
    }
}
