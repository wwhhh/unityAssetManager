namespace ActorCore
{

    public static class EActorState
    {
        public const int None = 0;
        public const int Idle = 1;
        public const int Move = 2;
        public const int Skill = 3;
        public const int Hit = 4;

        public static string ToString(int state)
        {
            switch (state)
            {
                case 0:
                    return "None";
                case 1:
                    return "Idle";
                case 2:
                    return "Move";
                case 3:
                    return "Skill";
                default:
                    return "None";
            }
        }
    }

}