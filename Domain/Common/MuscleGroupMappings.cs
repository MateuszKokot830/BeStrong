namespace Domain.Common
{
    public static class MuscleGroupMappings
    {
        private static readonly Dictionary<MuscleSubgroup, MuscleGroup> _map = new()
        {
            [MuscleSubgroup.Lats]         = MuscleGroup.Back,
            [MuscleSubgroup.Trapezius]    = MuscleGroup.Back,
            [MuscleSubgroup.LowerBack]    = MuscleGroup.Back,
            [MuscleSubgroup.Chest]        = MuscleGroup.Chest,
            [MuscleSubgroup.Abs]          = MuscleGroup.Core,
            [MuscleSubgroup.FrontDeltoid] = MuscleGroup.Shoulders,
            [MuscleSubgroup.RearDeltoid]  = MuscleGroup.Shoulders,
            [MuscleSubgroup.RotatorCuff]  = MuscleGroup.Shoulders,
            [MuscleSubgroup.Biceps]       = MuscleGroup.Arms,
            [MuscleSubgroup.Triceps]      = MuscleGroup.Arms,
            [MuscleSubgroup.Quads]        = MuscleGroup.Legs,
            [MuscleSubgroup.Hamstrings]   = MuscleGroup.Legs,
            [MuscleSubgroup.Glutes]       = MuscleGroup.Legs,
            [MuscleSubgroup.Adductors]    = MuscleGroup.Legs,
            [MuscleSubgroup.Calves]       = MuscleGroup.Legs,
        };

        public static MuscleGroup ToMuscleGroup(this MuscleSubgroup subgroup) => _map[subgroup];
    }
}
