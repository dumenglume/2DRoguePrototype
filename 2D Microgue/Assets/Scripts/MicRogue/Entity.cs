using UnityEngine;
using UnityEngine.Tilemaps;

namespace MicRogue
{
public class Entity : MonoBehaviour // TODO May need to create player and enemy sub-classes
{
    protected EntityMovement entityMovement;
    protected EntityInputBase entityInputController;

    // Components
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D coll2D;
    [HideInInspector] public Animator animator;

    // Fields
    protected enum EntityType { player, enemy }
    [SerializeField] protected EntityType entityType;

    // Turn flags
    protected bool hasTurn;
    public bool HasTurn { get { return hasTurn; } set { hasTurn = value; } }
    [SerializeField] protected int turnCount = 1;
    public int TurnCount { get { return turnCount; } set { turnCount = value; } }
    protected int remainingTurns = 1;
    public int RemainingTurns { get { return remainingTurns; } set { remainingTurns = value; } }
    protected int turnReductionAmount = 1;

    // Tilemap references
    [SerializeField] protected Tilemap groundTilemap;
    [SerializeField] protected Tilemap collisionTilemap;

    void Awake()
    {
        entityMovement        = GetComponent<EntityMovement>();
        entityInputController = GetComponent<EntityInputBase>();

        rb       = GetComponent<Rigidbody2D>();
        coll2D   = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        remainingTurns = turnCount;
    }

    protected void ReduceTurnCount(int _amount)
    {
        remainingTurns -= _amount;
    }
}
}