namespace Domain.Common
{
    public enum DateUnit
    {
        Year,
        Month,
        Day
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum PostType
    {
        Normal,
        WorkoutPublication
    }

    public enum WorkoutPlanCategory
    {
        FullBody,
        PushPullLegs,
        PushPull,
        UpperLower,
        BodyPartSplit
    }

    public enum MuscleGroup
    {
        Back,
        Chest,
        Core,
        Shoulders,
        Arms,
        Legs
    }

    public enum MuscleSubgroup
    {
        Chest,
        FrontDeltoid,
        RearDeltoid,
        Biceps,
        Triceps,
        Lats,
        Trapezius,
        LowerBack,
        Abs,
        Quads,
        Hamstrings,
        Glutes,
        Adductors,
        Calves,
        RotatorCuff
    }

    public enum ProfileVisibility
    {
        Public,
        FollowersOnly,
        Private
    }
}