using UnityEngine;

public class TetrisBlockSpawner : MonoBehaviour
{
    [SerializeField] TetrisBlockContainer[] polyominoes;

    static TetrisBlockSpawner instance;
    public static TetrisBlockSpawner Instance => instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {
        TetrisBlockContainer.BlockContainerPlaced += NewPolyomino;
    }

    void OnDisable()
    {
        TetrisBlockContainer.BlockContainerPlaced -= NewPolyomino;
    }

    void Start()
    {
        NewPolyomino();
    }

    void NewPolyomino()
    {
        if (polyominoes.Length <= 0) { throw new System.Exception("Block array is empty."); }

        TetrisBlockContainer newContainer;
        newContainer = Instantiate(polyominoes[Random.Range(0, polyominoes.Length)], transform.position, Quaternion.identity);
    }
}
