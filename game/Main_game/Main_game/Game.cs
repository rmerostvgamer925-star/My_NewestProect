using Main_game.Core;
using Main_game.Systems;
using OpenTK.Graphics;
using RLNET;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Main_game
{
    internal class Game
    {
        private static bool _renderRequired = true;

        public static CommandSystem CommandSystem { get; private set; }

        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        public static Player Player { get; private set; }

        public static DungeonMap DungeonMap { get; private set; }

        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }

        public static void Main()
        {
            // Establish the seed for the random number generator from the current time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            // The title will appear at the top of the console window 
            // also include the seed used to generate the level
            string consoleTitle = $"RougeSharp V3 Tutorial - Level 1 - Seed {seed}";

            // Это должно быть точное название файла растрового шрифта, который мы используем, иначе возникнет ошибка.
            string fontFileName = "ascii_8x8.png";

            // Указываем RLNet, что нужно использовать указанный нами растровый шрифт и что размер каждой плитки — 8 x 8 пикселей
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            _statConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);


            // Настраиваем обработчик события Update в RLNET
            _rootConsole.Update += OnRootConsoleUpdate;

            // Настраиваем обработчик события Render в RLNET
            _rootConsole.Render += OnRootConsoleRender;

            CommandSystem = new CommandSystem();

            Player = new Player();
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7);
            DungeonMap = mapGenerator.CreateMap();
            // End of existing code
            DungeonMap.UpdatePlayerFieldOfView();


            // Запускаем игровой цикл RLNET
            _rootConsole.Run();

        }
        // Обработчик события Update в RLNET
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Up || keyPress.Key == RLKey.Keypad8)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if (keyPress.Key == RLKey.Down || keyPress.Key == RLKey.Keypad2)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if (keyPress.Key == RLKey.Left || keyPress.Key == RLKey.Keypad4)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if (keyPress.Key == RLKey.Right || keyPress.Key == RLKey.Keypad6)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if (keyPress.Key == RLKey.Escape)
                {
                    _rootConsole.Close();
                }
            }

            if (didPlayerAct)
            {
                _renderRequired = true;
            }
        }

        // Обработчик события Render в RLNET
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (_renderRequired)
            {
                DungeonMap.Draw(_mapConsole);

                Player.Draw(_mapConsole, DungeonMap);

                // Blit the sub consoles to the root console in the correct locations
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();

                _renderRequired = false;
            }

        }

    }

}

