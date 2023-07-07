using Discord;
using MelonLoader;
using System;
using System.Threading;


[assembly: MelonInfo(typeof(ProjectDR.Main), "P06 Better Rich Presence ", "1.0.0", "Touyama")]
[assembly: MelonColor(ConsoleColor.DarkBlue)]

namespace ProjectDR
{
    public class Main : MelonMod

    {
        public const long AppId = 1124936962697072660;

        public Discord.Discord discordClient;
        public ActivityManager activityManager;
        public GameInfo gameInfo;

        private bool gameClosing;
        public bool GameStarted { get; private set; }
        public long gameStartedTime;


        public override void OnApplicationStart()
        {

            DiscordLibraryLoader.LoadLibrary();
            InitializeDiscord();
            UpdateActivity();
            new Thread(DiscordLoopThread).Start();
        }

        public override void OnApplicationLateStart()
        {
            GameStarted = true;
            gameStartedTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

            UpdateActivity();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            gameInfo.CurrentScene = gameInfo.GetCurrentStageData(sceneName);
            UpdateActivity(); //Updates image & stage data
        }

        public override void OnApplicationQuit()
        {
            gameClosing = true;
        }

        public void DiscordLoopThread()
        {
            for (; ; )
            {
                if (gameClosing)
                    break;

                discordClient.RunCallbacks();
                Thread.Sleep(200);
            }
        }

        public void InitializeDiscord()
        {
            discordClient = new Discord.Discord(AppId, (ulong)CreateFlags.NoRequireDiscord);
            discordClient.SetLogHook(LogLevel.Debug, DiscordLogHandler);

            activityManager = discordClient.GetActivityManager();
        }

        private void DiscordLogHandler(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Debug:
                    LoggerInstance.Msg(message);
                    break;

                case LogLevel.Warn:
                    LoggerInstance.Warning(message);
                    break;

                case LogLevel.Error:
                    LoggerInstance.Error(message);
                    break;
            }
        }

        public void UpdateActivity()
        {
            if (gameInfo == null)
            {
                gameInfo = new GameInfo();
                gameInfo.Load();
            }


            var activity = new Activity
            {
                Name = $"Sonic The Hedgehog P-06",
                Type = ActivityType.Playing

            };

            activity.Assets.LargeImage = "gameimage";
            activity.Name = $"Sonic The Hedgehog P-06";
            activity.Instance = true;
            activity.Assets.LargeText = activity.Name;

            if (GameStarted)
            {
                activity.Assets.LargeImage = gameInfo.CurrentStageImage;
                activity.Timestamps.Start = gameStartedTime;
                activity.State = gameInfo.CurrentScene; //The game has to start before updating the State since Unity will not pass any default values
            }

            activityManager.UpdateActivity(activity, ResultHandler);
        }

        public void ResultHandler(Result result)
        {

        }
    }
}
