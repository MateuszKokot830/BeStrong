using Domain.Common;

namespace Domain.Tests.Common
{
    public class MuscleGroupMappingsTests
    {
        [Theory]
        [InlineData(MuscleSubgroup.Lats, MuscleGroup.Back)]
        [InlineData(MuscleSubgroup.Trapezius, MuscleGroup.Back)]
        [InlineData(MuscleSubgroup.LowerBack, MuscleGroup.Back)]
        [InlineData(MuscleSubgroup.Chest, MuscleGroup.Chest)]
        [InlineData(MuscleSubgroup.Abs, MuscleGroup.Core)]
        [InlineData(MuscleSubgroup.FrontDeltoid, MuscleGroup.Shoulders)]
        [InlineData(MuscleSubgroup.RearDeltoid, MuscleGroup.Shoulders)]
        [InlineData(MuscleSubgroup.RotatorCuff, MuscleGroup.Shoulders)]
        [InlineData(MuscleSubgroup.Biceps, MuscleGroup.Arms)]
        [InlineData(MuscleSubgroup.Triceps, MuscleGroup.Arms)]
        [InlineData(MuscleSubgroup.Quads, MuscleGroup.Legs)]
        [InlineData(MuscleSubgroup.Hamstrings, MuscleGroup.Legs)]
        [InlineData(MuscleSubgroup.Glutes, MuscleGroup.Legs)]
        [InlineData(MuscleSubgroup.Adductors, MuscleGroup.Legs)]
        [InlineData(MuscleSubgroup.Calves, MuscleGroup.Legs)]
        public void ToMuscleGroup_MapsSubgroupToExpectedGroup(MuscleSubgroup subgroup, MuscleGroup expected)
        {
            var result = subgroup.ToMuscleGroup();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToMuscleGroup_EveryDefinedSubgroupHasAMapping()
        {
            foreach (MuscleSubgroup subgroup in Enum.GetValues<MuscleSubgroup>())
            {
                var exception = Record.Exception(() => subgroup.ToMuscleGroup());

                Assert.Null(exception);
            }
        }
    }
}
