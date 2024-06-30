/*
 * Eastern Autumn Prototype
 * Version 7
 * Amir Jaini (AJ)
 * 30 June 2024
 */
using System.Timers;

namespace EasternAutumnPrototype7
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu.SplashScreen();
        }

        // Refers to current active player, this information could be used to create a ScoreRecord or PlayerAccount instance
        static class Player
        {
            public static string Name { get; set; } = "";
            public static int PointCount { get; set; } = 0;
            public static int LifeCount { get; set; } = 3;

            public static void AddPoint()
            {
                PointCount++;
            }

            public static void LoseLife()
            {
                LifeCount--;
            }

            public static void PlayerNamePrompt()
            {
                Console.Clear();
                Console.WriteLine("Enter a player name:");
                Name = Console.ReadLine();
            }

            public static void ReportScore()
            {
                Console.Clear();
                Console.WriteLine("++++++++++Your Score++++++++++");
                Console.WriteLine("Name: " + Name);
                Console.WriteLine("Points: " + PointCount);
                Console.WriteLine("Lives Remaining: " + LifeCount);
                Console.WriteLine("++++++++++++++++++++++++++++++");
            }
        }

        // [Create a PlayerRecord class for storing scores here?]

        // Per feedback, reliance on static classes to be revised
        static class MainMenu
        {
            // Splash screen when booting up the application, all non-English text is in Finnish. Idea: The player could "unlock" translations as rewards?
            public static void SplashScreen()
            {
                // "Work teaches the worker"
                Console.WriteLine("=========================");
                Console.WriteLine("      EASTERN AUTUMN     ");
                Console.WriteLine(" 'Työ tekijäänsä neuvoo' ");
                Console.WriteLine("=========================");
                Console.WriteLine("*Press any key to start*");
                Console.ReadKey();
                MainMenuScreen();
            }

            // Main menu for accessing all options available
            public static void MainMenuScreen()
            {
                Console.Clear();
                Console.WriteLine("========MAIN MENU========");
                Console.WriteLine("");
                Console.WriteLine("    1. NEW GAME          ");
                Console.WriteLine("");
                Console.WriteLine("    2. SELECT STAGE      ");
                Console.WriteLine("");
                Console.WriteLine("    3. OPTIONS           ");
                Console.WriteLine("");
                Console.WriteLine("    4. QUIT              ");
                Console.WriteLine("");
                Console.WriteLine("=========================");
                Console.WriteLine("");
                Console.Write("Select Action: ");
                MainMenuAction(Console.ReadLine().ToUpper());
            }

            public static void MainMenuAction(string input)
            {
                switch (input)
                {
                    case "1":
                    case "NEW":
                    case "NEW GAME":
                        GameplayInitialiser.NewGame();
                        break;
                    case "2":
                    case "SELECT":
                    case "SELECT STAGE":
                        GameplayInitialiser.SelectStage();
                        break;
                    case "3":
                    case "OPTIONS":
                        OptionsSettings.OptionsMenu();
                        break;
                    case "4":
                    case "QUIT":
                        ExitHandler();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        MiscUI.AnyKeyPrompt();
                        MainMenuScreen();
                        break;
                }
            }

            public static void ExitHandler()
            {
                Console.Clear();
                Console.WriteLine("Are you sure you want to leave the game?");
                Console.Write("Y/N: ");
                string input = Console.ReadLine().ToUpper();
                switch (input)
                {
                    case "Y":
                    case "YES":
                        Environment.Exit(0);
                        break;
                    default:
                        MainMenuScreen();
                        break;
                }
            }
        }

        static class GameplayInitialiser
        {
            public static void NewGame()
            {
                DifficultySelector.DifficultyLevel = 0;
                InitialiseGameplay();
            }

            public static void SelectStage()
            {
                DifficultySelector.DifficultySelect();
                InitialiseGameplay();
            }

            public static void InitialiseGameplay()
            {
                // To do: Players will be informed of "recent events" (like city renames) at the start of a stage
                Playset.PlaysetSelector(DifficultySelector.DifficultyLevel);
                // In the final version this should only apply at the start of a new/level selected game session
                Player.LifeCount = 3;
                GameplayLoop.LoadMail();
            }
        }

        // This feature may be omitted in the final version based on user requirements capture data, or locked as a reward mechanic
        static class OptionsSettings
        {
            public static int Time { get; set; } = 60;
            public static int Target { get; set; } = 10;

            public static void OptionsMenu()
            {
                Console.Clear();
                Console.WriteLine("========OPTIONS========");
                Console.WriteLine("");
                Console.WriteLine("   Current Settings:   ");
                Console.WriteLine("");
                Console.WriteLine(" Timer (seconds): " + Time);
                Console.WriteLine(" Targets to win: " + Target);
                Console.WriteLine(" Player Name: " + Player.Name);
                Console.WriteLine("");
                Console.WriteLine("      1. MODIFY        ");
                Console.WriteLine("      2. DEFAULT       ");
                Console.WriteLine("      3. BACK          ");
                Console.WriteLine("");
                Console.WriteLine("========================");

                OptionsAction();
            }

            public static void OptionsAction()
            {
                string input = Console.ReadLine().ToUpper();
                switch (input)
                {
                    case "1":
                    case "MODIFY":
                        ModifyMenu();
                        break;
                    case "2":
                    case "DEFAULT":
                        Console.WriteLine("Are you sure you wish to reset to default values? (Y/N)");
                        ResetAction();
                        break;
                    case "3":
                    case "BACK":
                        MainMenu.MainMenuScreen();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        MiscUI.AnyKeyPrompt();
                        OptionsAction();
                        break;
                }

            }

            public static void ModifyMenu()
            {
                Console.Clear();
                Console.WriteLine("=========MODIFY========");
                Console.WriteLine("");
                Console.WriteLine("   Enter new values:   ");
                Console.WriteLine("");
                Console.Write(" Timer (seconds): ");
                string newTimer = Console.ReadLine().ToUpper();
                Console.Write(" Targets to win: ");
                string newTarget = Console.ReadLine().ToUpper();
                Console.Write(" Player Name: ");
                string newName = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("       1. SAVE          ");
                Console.WriteLine("       2. CANCEL        ");
                Console.WriteLine("");
                Console.WriteLine("========================");
                ModifyAction(newTimer, newTarget, newName);
            }

            public static void ModifyAction(string modTime, string modTarget, string modName)
            {
                string input = Console.ReadLine().ToUpper();
                switch (input)
                {
                    case "1":
                    case "SAVE":
                        try
                        {
                            Time = int.Parse(modTime);
                            Target = int.Parse(modTarget);
                            Player.Name = modName;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid values!");
                            MiscUI.AnyKeyPrompt();
                            ModifyMenu();
                        }
                        OptionsMenu();
                        break;
                    case "2":
                    case "CANCEL":
                        OptionsMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        ModifyAction(modTime, modTarget, modName);
                        break;
                }
            }

            public static void ResetAction()
            {
                string input = Console.ReadLine().ToUpper();
                switch (input)
                {
                    case "Y":
                        Time = 60;
                        Target = 10;
                        OptionsMenu();
                        break;
                    case "N":
                        OptionsMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        MiscUI.AnyKeyPrompt();
                        ResetAction();
                        break;
                }
            }
        }

        // This might be renamed as "stage/level" selector. Players are expected to play from stages 1 through 4 progressively. 
        static class DifficultySelector
        {
            public static int DifficultyLevel { get; set; }

            public static void DifficultySelect()
            {
                // In final version, player will be required to unlock later stages
                Console.Clear();
                Console.WriteLine("===Select gameplay year===");
                Console.WriteLine("");
                Console.WriteLine("     1. [Easy]   1989");
                Console.WriteLine("     2. [Medium] 1992");
                Console.WriteLine("     3. [Hard]   1994");
                Console.WriteLine("     4. [Expert] 1996");
                Console.WriteLine("");
                Console.WriteLine("==========================");
                string input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "1989":
                    case "89":
                    case "EASY":
                    case "1":
                        DifficultyLevel = 0;
                        break;
                    case "1992":
                    case "92":
                    case "MEDIUM":
                    case "2":
                        DifficultyLevel = 1;
                        break;
                    case "1994":
                    case "94":
                    case "HARD":
                    case "3":
                        DifficultyLevel = 2;
                        break;
                    case "1996":
                    case "96":
                    case "EXPERT":
                    case "4":
                        DifficultyLevel = 3;
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        MiscUI.AnyKeyPrompt();
                        DifficultySelect();
                        break;
                }
            }
        }

        // This refers to the potential mail addresses generated based on difficulty, increasing with complexity
        static class Playset
        {
            public static IDictionary<int, List<string>> Nations = new Dictionary<int, List<string>>();
            // Used to check user input against internal number
            public static IDictionary<string, int> Checklist = new Dictionary<string, int> {
                {"ALBANIA", 1},
                {"BULGARIA", 2},
                {"HUNGARY", 3},
                {"POLAND", 4},
                {"ROMANIA", 5},
                {"RUSSIA", 6},
                {"ARMENIA", 7},
                {"AZERBAIJAN", 8},
                {"BELARUS", 9},
                {"ESTONIA", 10},
                {"GEORGIA", 11},
                {"KAZAKHSTAN", 12},
                {"KYRGYZSTAN", 13},
                {"LATVIA", 14},
                {"LITHUANIA", 15},
                {"MOLDOVA", 16},
                {"TAJIKISTAN", 17},
                {"TURKMENISTAN", 18},
                {"UKRAINE", 19},
                {"UZBEKISTAN", 20}
            };

            // Generates the playset based on the current difficulty
            public static void PlaysetSelector(int difficulty)
            {
                // Ensures playset is empty before adding anything
                Nations.Clear();

                if (difficulty < 2)
                {
                    // Starting Country List (1989, Pre-Dissolution/ Eastern Bloc). Certain countries like East Germany/Czechoslovakia are omitted for simplicity.
                    Nations.Add(1, new List<string> { "Tirana" });    // Albania
                    Nations.Add(2, new List<string> { "Sofia" });     // Bulgaria
                    Nations.Add(3, new List<string> { "Budapest" });  // Hungary
                    Nations.Add(4, new List<string> { "Warsaw" });    // Poland
                    Nations.Add(5, new List<string> { "Bucharest" }); // Romania
                    Nations.Add(6, new List<string> { "Moscow" });    // Russia
                }
                if (difficulty == 1)
                {
                    // Additional Country List (1992, Post-Dissolution)
                    Nations.Add(7, new List<string> { "Yerevan" });     // Armenia
                    Nations.Add(8, new List<string> { "Baku" });        // Azerbaijan
                    Nations.Add(9, new List<string> { "Minsk" });       // Belarus
                    Nations.Add(10, new List<string> { "Tallinn" });    // Estonia
                    Nations.Add(11, new List<string> { "Tbilisi" });    // Georgia
                    Nations.Add(12, new List<string> { "Alma-Alta" });  // Kazakhstan (later renamed in 1993; capital also changed in 1997)
                    Nations.Add(13, new List<string> { "Bishkek" });    // Kyrgzstan
                    Nations.Add(14, new List<string> { "Riga" });       // Latvia
                    Nations.Add(15, new List<string> { "Vilnius" });    // Lithuania
                    Nations.Add(16, new List<string> { "Chișinău" });   // Moldova (Kishinev pre 1991)
                    Nations.Add(17, new List<string> { "Dushanbe" });   // Tajikistan
                    Nations.Add(18, new List<string> { "Ashgabat" });   // Turkmenistan
                    Nations.Add(19, new List<string> { "Kiev" });       // Ukraine (officially renamed in 1995)
                    Nations.Add(20, new List<string> { "Tashkent" });   // Uzbekistan
                }
                if (difficulty == 2)
                {
                    // Each country now has 2 cities (1993)
                    Nations.Add(1, new List<string> { "Tirana", "Durrës" });            // Albania
                    Nations.Add(2, new List<string> { "Sofia", "Plovdiv" });            // Bulgaria
                    Nations.Add(3, new List<string> { "Budapest", "Debrecen" });        // Hungary
                    Nations.Add(4, new List<string> { "Warsaw", "Kraków" });            // Poland
                    Nations.Add(5, new List<string> { "Bucharest", "Cluj-Napoca" });    // Romania
                    Nations.Add(6, new List<string> { "Moscow", "Saint Petersburg" });  // Russia (Leningrad pre-1991)
                    Nations.Add(7, new List<string> { "Yerevan", "Gyumri" });           // Armenia (Leninakan pre-1991)
                    Nations.Add(8, new List<string> { "Baku", "Ganja" });               // Azerbaijan
                    Nations.Add(9, new List<string> { "Minsk", "Gomel" });              // Belarus
                    Nations.Add(10, new List<string> { "Tallinn", "Tartu" });           // Estonia
                    Nations.Add(11, new List<string> { "Tbilisi", "Batumi" });          // Georgia
                    Nations.Add(12, new List<string> { "Almatу", "Karaganda" });        // Kazakhstan (Almatу was Alma-Ata pre-1993)
                    Nations.Add(13, new List<string> { "Bishkek", "Osh" });             // Kyrgzstan
                    Nations.Add(14, new List<string> { "Riga", "Daugavpils" });         // Latvia
                    Nations.Add(15, new List<string> { "Vilnius", "Kaunas" });          // Lithuania
                    Nations.Add(16, new List<string> { "Chișinău", "Bălți" });          // Moldova
                    Nations.Add(17, new List<string> { "Dushanbe", "Khujand" });        // Tajikistan
                    Nations.Add(18, new List<string> { "Ashgabat", "Türkmenabat" });    // Turkmenistan
                    Nations.Add(19, new List<string> { "Kiev", "Kharkov" });            // Ukraine (renamed in 1995)
                    Nations.Add(20, new List<string> { "Tashkent", "Samarkand" });      // Uzbekistan
                }
                if (difficulty == 3)
                {
                    // Each country now has 3 cities (1996)
                    Nations.Add(1, new List<string> { "Tirana", "Durrës", "Vlorë" });                   // Albania
                    Nations.Add(2, new List<string> { "Sofia", "Plovdiv", "Varna" });                   // Bulgaria
                    Nations.Add(3, new List<string> { "Budapest", "Debrecen", "Miskolc" });             // Hungary
                    Nations.Add(4, new List<string> { "Warsaw", "Kraków", "Łódź" });                    // Poland
                    Nations.Add(5, new List<string> { "Bucharest", "Cluj-Napoca", "Timișoara" });       // Romania
                    Nations.Add(6, new List<string> { "Moscow", "Saint Petersburg", "Novosibirsk" });   // Russia
                    Nations.Add(7, new List<string> { "Yerevan", "Gyumri", "Vanadzor" });               // Armenia
                    Nations.Add(8, new List<string> { "Baku", "Ganja", "Sumqayit" });                   // Azerbaijan
                    Nations.Add(9, new List<string> { "Minsk", "Gomel", "Grodno" });                    // Belarus
                    Nations.Add(10, new List<string> { "Tallinn", "Tartu", "Narva" });                  // Estonia
                    Nations.Add(11, new List<string> { "Tbilisi", "Batumi", "Kutaisi" });               // Georgia
                    Nations.Add(12, new List<string> { "Almatу", "Karaganda", "Akmola" });              // Kazakhstan (Astana post-1998/ post-2022, they need to make up their minds)
                    Nations.Add(13, new List<string> { "Bishkek", "Osh", "Jalal-Abad" });               // Kyrgzstan
                    Nations.Add(14, new List<string> { "Riga", "Daugavpils", "Liepāja" });              // Latvia
                    Nations.Add(15, new List<string> { "Vilnius", "Kaunas", "Klaipėda" });              // Lithuania
                    Nations.Add(16, new List<string> { "Chișinău", "Bălți", "Bender" });                // Moldova
                    Nations.Add(17, new List<string> { "Dushanbe", "Khujand", "Kulob" });               // Tajikistan
                    Nations.Add(18, new List<string> { "Ashgabat", "Türkmenabat", "Dasoguz" });         // Turkmenistan
                    Nations.Add(19, new List<string> { "Kyiv", "Kharkiv", "Odessa" });                  // Ukraine (1995 Ukrainisation)
                    Nations.Add(20, new List<string> { "Tashkent", "Samarkand", "Namangan" });          // Uzbekistan
                }
            }
        }

        // Used to handle both the gameplay countdown timer and (potentially?) timers for UI elements
        class GameplayTimer
        {
            public int SecondsCount { get; set; }
            public System.Timers.Timer Countdown { get; set; }
            public bool TimerRunning { get; set; }

            public GameplayTimer(int seconds)
            {
                SecondsCount = seconds;
                Countdown = new System.Timers.Timer(1000);
                Countdown.Elapsed += TimerElapsed;
                Countdown.AutoReset = true;
            }

            public void StartTimer()
            {
                TimerRunning = true;
                Countdown.Start();
            }

            // Forces the timer to stop prematurely
            public void ForceStop()
            {
                Countdown.Stop();
                Countdown.Elapsed -= TimerElapsed;
                TimerRunning = false;
                Countdown.Dispose();
            }

            private void TimerElapsed(object sender, ElapsedEventArgs e)
            {
                SecondsCount--;
                if (SecondsCount <= 0)
                {
                    Countdown.Stop();
                    Countdown.Elapsed -= TimerElapsed;
                    TimerRunning = false;
                    Countdown.Dispose();
                }
            }
        }

        // This class manages the gameplay loop mechanics within a gameplay session
        static class GameplayLoop
        {
            // NOTE: Timer is deliberately not visible to player to add tension
            public static GameplayTimer Timer;

            // Initialises all nations list, informs player of target required to win
            public static void SessionStart()
            {
                Console.Clear();
                Console.WriteLine("Today's Required Target: " + OptionsSettings.Target + " mail items");
                Console.WriteLine("*Press any key to start!");
                Console.ReadKey();
            }

            // This represents the main gameplay loop that repeats until timer/lives reaches zero
            public static void LoadMail()
            {
                SessionStart();

                // Timer only starts after user has "started"
                Timer = new GameplayTimer(OptionsSettings.Time);
                Timer.StartTimer();

                while (Player.LifeCount > 0 && Timer.TimerRunning)
                {
                    if (!Timer.TimerRunning)
                    {
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine("*Press any key to load a mail item*");
                    Console.ReadKey();
                    MailGenerator.GenerateMail();
                    ViewMail();
                    UserAction();
                }
                // Game-loop ended, display win/loss message
                MiscUI.EndGame();

                // Report Player stats & return to main menu
                Player.ReportScore();

            }

            public static void ViewMail()
            {
                Console.Clear();
                Console.WriteLine("=====MAIL ITEM=====");
                Console.WriteLine("To: " + MailGenerator.MailAddress);
                Console.WriteLine("===================");
                Console.WriteLine("");
            }

            public static void UserAction()
            {
                //Prompt user input, case-sensitive
                Console.WriteLine("Select action:");
                Console.WriteLine("[C]HECK / [S]END / [Q]UIT)");
                string input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "CHECK":
                    case "C":
                        // Switch to Sheet Mode
                        GameplaySheetMode.ViewSheet();
                        break;
                    case "SEND":
                    case "S":
                        // Switch to Send Mode
                        GameplaySendMode.UserAction();
                        break;
                    case "QUIT":
                    case "Q":
                        QuitSession();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        Console.WriteLine("");
                        UserAction();
                        break;
                }
            }

            // Allows the player to quit the game early
            public static void QuitSession()
            {
                Console.WriteLine("");
                Console.WriteLine("Are you sure you want to quit and return to the main menu?");
                Console.Write("Y/N: ");
                string input = Console.ReadLine().ToUpper();
                switch (input)
                {
                    case "Y":
                    case "YES":
                        // Stop timer and 
                        Timer.ForceStop();
                        break;
                    default:
                        ViewMail();
                        UserAction();
                        break;
                }
            }

        }

        // This is the "Cheat Sheet" mode where the player can "look up" correct answers at the cost of time
        static class GameplaySheetMode
        {
            // Determines which page the cheat sheet is on (true = Eastern Bloc, false = USSR)
            public static bool SheetPage { get; set; } = true;

            public static void UserAction()
            {
                Console.WriteLine("Select action:");
                Console.WriteLine("[F]LIP / [S]END / [Q]UIT");
                string input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "FLIP":
                    case "F":
                        FlipSheet();
                        break;
                    case "SEND":
                    case "S":
                        // Switch to Send Mode
                        GameplaySendMode.UserAction();
                        break;
                    case "QUIT":
                    case "Q":
                        GameplayLoop.QuitSession();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        Console.WriteLine("");
                        UserAction();
                        break;
                }
            }

            public static void ViewSheet()
            {
                Console.Clear();
                if (SheetPage)
                {
                    CheatSheet.BlocSheet();
                }
                else
                {
                    CheatSheet.SovietSheet();
                }
                UserAction();
            }

            public static void FlipSheet()
            {
                SheetPage = !SheetPage;
                Console.Clear();
                ViewSheet();
            }
        }

        // Considering merging this into the GameplaySheetMode class 
        static class CheatSheet
        {
            // Used to guide the player, deliberately out of date and ergo less reliable as (in-story) time & difficulty progresses
            public static void BlocSheet()
            {
                // "Pekka" is a stereotypical "old man" Finnish name, a subtle hint that the cheatsheet is a bit out of date
                Console.WriteLine("====PEKKA'S EASTERN BLOC CHEAT SHEET====");
                Console.WriteLine("");
                Console.WriteLine("ALBANIA");
                Console.WriteLine("Tirana, Durrës, Vlorë");
                Console.WriteLine("");
                Console.WriteLine("BULGARIA");
                Console.WriteLine("Sofia, Plovdiv, Varna");
                Console.WriteLine("");
                Console.WriteLine("HUNGARY");
                Console.WriteLine("Budapest, Debrecen, Miskolc");
                Console.WriteLine("");
                Console.WriteLine("POLAND");
                Console.WriteLine("Warsaw, Kraków, Łódź");
                Console.WriteLine("");
                Console.WriteLine("ROMANIA");
                Console.WriteLine("Bucharest, Cluj-Napoca, Timișoara");
                Console.WriteLine("");
                Console.WriteLine("=======================================");
            }

            public static void SovietSheet()
            {
                Console.WriteLine("=======PEKKA'S SSR CHEAT SHEET=========");
                Console.WriteLine("");
                Console.WriteLine("RUSSIAN FSR");
                Console.WriteLine("Moscow, Leningrad, Novosibirsk");
                Console.WriteLine("");
                Console.WriteLine("ARMENIAN SSR");
                Console.WriteLine("Yerevan, Leninakan, Vanadzor");
                Console.WriteLine("");
                Console.WriteLine("AZERBAIJAN SSR");
                Console.WriteLine("Baku, Ganja, Sumqayit");
                Console.WriteLine("");
                Console.WriteLine("BYELORUSSIAN (BELARUSSIAN) SSR");
                Console.WriteLine("Minsk, Gomel, Grodno");
                Console.WriteLine("");
                Console.WriteLine("ESTONIAN SSR");
                Console.WriteLine("Tallinn, Tartu, Narva");
                Console.WriteLine("");
                Console.WriteLine("GEORGIAN SSR");
                Console.WriteLine("Tbilisi, Batumi, Kutaisi");
                Console.WriteLine("");
                Console.WriteLine("KAZAKH SSR");
                Console.WriteLine("Alma-Ata, Karaganda, Akmoly");
                Console.WriteLine("");
                Console.WriteLine("KIRGHIZ (KYRGYZ) SSR");
                Console.WriteLine("Bishkek, Osh, Jalal-Abad");
                Console.WriteLine("");
                Console.WriteLine("LATVIAN SSR");
                Console.WriteLine("Riga, Daugavpils, Liepāja");
                Console.WriteLine("");
                Console.WriteLine("LITHUANIAN SSR");
                Console.WriteLine("Vilnius, Kaunas, Klaipėda");
                Console.WriteLine("");
                Console.WriteLine("MOLDAVAN (MOLDOVAN) SSR");
                Console.WriteLine("Chișinău, Tiraspol, Bălți");
                Console.WriteLine("");
                Console.WriteLine("TAJIK SSR");
                Console.WriteLine("Dushanbe, Khujand, Kulob");
                Console.WriteLine("");
                Console.WriteLine("TURKMEN SSR");
                Console.WriteLine("Ashgabat, Türkmenabat, Dasoguz");
                Console.WriteLine("");
                Console.WriteLine("UKRAINIAN SSR");
                Console.WriteLine("Kiev, Kharkov, Odessa");
                Console.WriteLine("");
                Console.WriteLine("UZBEK SSR");
                Console.WriteLine("Tashkent, Samarkand, Namangan");
                Console.WriteLine("");
                Console.WriteLine("=======================================");
            }
        }

        // This represents "Send Mode" where the player picks an answer which is then checked
        static class GameplaySendMode
        {
            public static void UserAction()
            {
                // Re-displays mail item
                GameplayLoop.ViewMail();

                // User selects a receiving country, case-insensitive
                Console.WriteLine("Select Receiving Country:");
                Console.WriteLine("(or use [C]HECK / [Q]UIT)");
                Console.WriteLine("");
                Console.WriteLine("***WARNING: Typos will be penalised!***");
                string input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "CHECK":
                    case "C":
                        // Switch to Sheet mode
                        GameplaySheetMode.ViewSheet();
                        break;
                    case "QUIT":
                    case "Q":
                        GameplayLoop.QuitSession();
                        break;
                    default:
                        AnswerChecker(input);
                        break;
                }
            }

            public static void AnswerChecker(string input)
            {
                try
                {
                    int checker = Playset.Checklist[input];

                    if (checker == MailGenerator.ReceiverNation)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Hyvää työtä! +1 Point");
                        Player.AddPoint();
                    }
                    else
                    {
                        Console.WriteLine("");
                        // This is a (popular) Finnish swearword and will not be present in the final version
                        Console.WriteLine("Perkele! -1 Life");
                        Player.LoseLife();
                    }
                    MiscUI.AnyKeyPrompt();
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("");
                    Console.WriteLine("That country doesn't exist! -2 Lives");
                    Player.LoseLife();
                    Player.LoseLife();
                    MiscUI.AnyKeyPrompt();
                }
            }
        }

        static class MailGenerator
        {
            public static int ReceiverNation { get; set; }
            public static int ReceiverCity { get; set; }
            public static string MailAddress { get; set; }
            public static Random Random = new Random();

            public static void GenerateMail()
            {
                ReceiverNation = Random.Next(1, (Playset.Nations.Count + 1));
                GenerateAddress();
            }

            public static void GenerateAddress()
            {
                if (DifficultySelector.DifficultyLevel == 0)
                {
                    ReceiverCity = 0;
                }
                else
                {
                    ReceiverCity = Random.Next(0, DifficultySelector.DifficultyLevel);
                }
                MailAddress = Playset.Nations[ReceiverNation][ReceiverCity];
            }
        }

        static class MiscUI
        {
            public static void EndGame()
            {
                Console.WriteLine("===========================================");
                Console.WriteLine("                TIME IS UP!                ");
                Console.WriteLine("       ***Press any key to continue***     ");
                Console.WriteLine("===========================================");
                Console.ReadKey();
                Console.Clear();
                if (Player.LifeCount <= 0)
                {
                    // “It’s no use crying at the marketplace”
                    Console.WriteLine("===========================================");
                    Console.WriteLine("           YOU ARE OUT OF LIVES!           ");
                    Console.WriteLine("        'Ei auta itku markkinoilla.' D:    ");
                    Console.WriteLine("===========================================");
                }
                else if (Player.PointCount < OptionsSettings.Target)
                {
                    // "No one is born a smith"
                    Console.WriteLine("===========================================");
                    Console.WriteLine("        YOU DIDN'T MEET YOUR TARGET!       ");
                    Console.WriteLine("     'Ei kukaan ole seppä syntyessään.' :| ");
                    Console.WriteLine("===========================================");
                }
                else
                {
                    // "All’s well that ends well!"
                    Console.WriteLine("===========================================");
                    Console.WriteLine("              YOU HAVE WON!                ");
                    Console.WriteLine("      'Loppu hyvin, kaikki hyvin!' :D      ");
                    Console.WriteLine("===========================================");
                }
                Player.ReportScore();
                Console.WriteLine("*Press any key to return to the main menu*");
                Console.ReadKey();
                MainMenu.MainMenuScreen();
            }

            public static void AnyKeyPrompt()
            {
                Console.WriteLine("*Press any key to continue*");
                Console.ReadKey();
            }
        }
        //[insert new classes here]
    }
}