using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellule
{
    public Vector3 positionXYZ { get;private set; }
    public GameObject go { get; private set; }
    public bool hasTower;

    public Cellule(GameObject go)
    {
        this.go = go;
        positionXYZ = go.transform.position;
        positionXYZ = new Vector3(positionXYZ.x, positionXYZ.y, 0);
    }

    //private Tower tower;
}

public class Grille : MonoBehaviour
{
    public static Grille instance { get; private set; }
    public float cellRadius;
    private List<Cellule> grille = new List<Cellule>();


    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void MakeGrid()
    {
        GameObject[] landsArray = GameObject.FindGameObjectsWithTag("Land");

        foreach(GameObject go in landsArray)
        {
            grille.Add(new Cellule(go));
        }
    }

    public Cellule getCellAtPosition(Vector3 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        foreach(Cellule cellule in grille)
        {
            if(Vector3.Distance(worldPos, cellule.positionXYZ) < cellRadius){
                return cellule;
            }
        }
        return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        MakeGrid();
    }

    public void RestartGrille() {
        foreach (Cellule cellule in grille) {
            cellule.hasTower = false;
        }
    }
}
