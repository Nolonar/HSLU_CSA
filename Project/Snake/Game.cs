using System;
using System.Collections.Generic;
using System.Numerics;

using Explorer700Wrapper;

namespace Project.Snake
{
    class Game
    {
        private const long moveDelay = 500 * Unit.Millisecond;

        private long currentMoveDelay = 0;
        private bool isGameRunning;
        private DateTime previousTime;

        private InputManager InputManager => Program.InputManager;

        public const float Scaling = 2;

        public static Vector2 FieldDimension => Program.ScreenDimension / Scaling;
        public Player Player { get; private set; }
        public Food Food { get; private set; }

        public Game()
        {
            Player = new Player(FieldDimension / 2);
            Food = new Food();

            Reset();
        }

        private IEnumerable<IRenderObject> GetRenderObjects()
        {
            yield return Player;
            yield return Food;
        }

        private void Reset()
        {
            Player.Reset();
            Food.MoveToNewPosition(Player);
        }

        public void Run()
        {
            isGameRunning = true;
            while(isGameRunning)
            {
                DateTime currentTime = DateTime.UtcNow;
                Update(currentTime - previousTime);
                Draw(Program.Display);
                previousTime = currentTime;
            }
        }

        private void ProcessInput()
        {
            isGameRunning = !InputManager.IsKeyPressed(Keys.Center, 1 * Unit.Second);
        }

        private void Update(TimeSpan delta)
        {
            ProcessInput();

            currentMoveDelay += delta.Ticks;
            if (currentMoveDelay < moveDelay)
                return;

            currentMoveDelay %= moveDelay;
            Player.ChangeDirection(InputManager.KeysPressed);
            if (!Player.IsMoving)
                return;

            Player.Grow();
            Player.UpdatePosition();
            if (Player.Position == Food.Position)
            {
                Program.Buzzer.Beep(1);
                Player.Eat(Food);
            }
            else
            {
                Player.Shrink();
            }
        }

        private void Draw(Display display)
        {
            display.Clear();
            foreach (IRenderObject o in GetRenderObjects())
                o.Draw(display.Graphics);

            display.Update();
        }
    }
}
