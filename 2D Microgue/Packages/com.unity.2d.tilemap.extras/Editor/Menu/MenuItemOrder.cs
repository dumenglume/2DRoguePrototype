using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
    internal enum ETilesMenuItemOrder
    {
        AnimatedTile = 2,
        RuleTile = 100,
        CustomRuleTile,
        RandomTile = 200,
        WeightedRandomTile,
        PipelineTile,
        TerrainTile,
    }
    internal enum EBrushMenuItemOrder
    {
        RandomBrush = 3,
        PrefabBrush,
        PrefabRandomBrush
    }
    
    static internal partial class AssetCreation
    {
        
        [MenuItem("Assets/Create/2D/Tiles/Animated Tile",  priority = (int) ETilesMenuItemOrder.AnimatedTile)]
        static void CreateAnimatedTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<AnimatedTile>(), "New Animated Tile.asset");
        }
            
        [MenuItem("Assets/Create/2D/Tiles/Rule Tile", priority = (int)ETilesMenuItemOrder.RuleTile)]
        static void CreateRuleTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<RuleTile>(), "New Rule Tile.asset");
        }
    }
}
