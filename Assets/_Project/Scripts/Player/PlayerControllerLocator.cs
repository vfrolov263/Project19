namespace Assets._Project.Scripts.Player
{
    /// <summary>
    /// Локатор для доступа к текущему PlayerController из интеракторов,
    /// расположенных на сцене (не инжектируемых через VContainer).
    /// </summary>
    public static class PlayerControllerLocator
    {
        public static PlayerController Current { get; set; }
    }
}