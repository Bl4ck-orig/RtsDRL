namespace ReinforcementLearning
{
    public class RtsConfig
    {
         public float AmountOfGhoulsForBuildingPercent { get; set; } = 0.5f;
         public float AmountOfGhoulsForAttackPercent { get; set; } = 0.25f;
         public float AmountOfGhoulsForPickupWeaponsPercent { get; set; } = 0.5f;
         public float AmountOfGhoulsForTribeTakeoverPercent { get; set; } = 0.25f;
         public int MinAmountOfGhoulsForTribeTakeOver { get; set; } = 2;
         public int MinAmountOfGhoulsForDefensiveTribeTakeOver { get; set; } = 2;
         public int MinAmountOfGhoulsForAttack { get; set; } = 2;
         //Operation Timing
         public float OperationsInterval { get; set; } = 1f;
         public float StartOperationInSeconds { get; set; } = 1f;
         //Danger Distancing
         public int MapInfluenceDangerCheckDistance { get; set; } = 3;
         public int MapInfluenceNearbyAllyCheckDistance { get; set; } = 2;
         public int MapInfluenceNearbyEnemyCheckDistance { get; set; } = 3;
         public int DefensiveAreaMinDistanceToEnemy { get; set; } = 5;
         public int AggressiveAreaMaxDistanceToEnemy { get; set; } = 2;
         //Buildspot Distancing
         public int BuildingCloseRangeCheckDistance { get; set; } = 1;
         public int BuildingMidRangeCheckDistance { get; set; } = 2;
         public int BuildingDistanceFromMapBorder { get; set; } = 1;
         //Buildspot Weights
         public int RaceWeightInfluenceValueForBuildSpotTribeMin { get; set; } = 0;
         public int RaceWeightInfluenceValueForBuildSpotTribeMax { get; set; } = 1;
         public int RaceWeightInfluenceValueForBuildSpotWorkshopMin { get; set; } = 2;
         public int RaceWeightInfluenceValueForBuildSpotWorkshopMax { get; set; } = 3;
    }
}
