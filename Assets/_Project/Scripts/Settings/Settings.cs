namespace Assets._Project.Scripts.Settings
{
    public static class Settings
    {
        public static float MouseSensitivity { get; set; } = 3f;
        public const float CHECK_INTERACTION_INTERVAL = 0.5f;
        public const float MAX_INTERACTION_DISTANCE = 4f;
        public const float DEFAULT_INTERACTION_DISTANCE = 1.8f;
        public const float COMMENT_BASE_TIME = 2f;
        public const int COMMENT_BASE_LENGTH = 100;
        public const float COMMENT_TIME_PER_EXTRA_LETTER = 0.1f;
    }
}