using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;

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
        public const float PLAYER_INITIAL_DOOR_OPEN_MOVEMENT_TIME = 0.4f;
        public const float PLAYER_DOOR_OPEN_VERTICAL_LOOK_ANGLE = 20f;

        public static void ReloadGame(float delay = 0f)
        {
            if (Mathf.Approximately(0f, delay))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
            {
                GameplayEntryPoint entryPoint = GameObject.FindFirstObjectByType<GameplayEntryPoint>();
                _ = ReloadGameRoutine(delay, entryPoint.destroyCancellationToken);
            }            
        }

        public static async Awaitable ReloadGameRoutine(float delay, CancellationToken ct)
        {
            await Awaitable.WaitForSecondsAsync(delay, ct);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}