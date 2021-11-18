# HSLU_CSA

A couple of games written in C# running on a Raspberry Pi 4 (2 GB) with a [Joy-IT Explorer 700](https://joy-it.net/en/products/RB-Explorer700).

## Requirements

- Raspberry Pi (models B+, 2B, 3B, 3B+, 4B)
- Explorer 700
- .NET Core 3.1

## How to run

1. Clone this project to Raspberry Pi.
2. `./run.sh Project` on Raspberry Pi (SSH recommended).
3. Push the joystick up or down to select the game to launch, press on the joystick to start the game.

## Games

Hold the joystick pressed for 1 second to return to the game selection menu.

### Pong

- Press on the joystick to get the ball moving.
- Push the joystick up or down to move the paddle.
- Try to get the ball past the opposing paddle, avoid letting the ball past your own.

### GravityPong

- Same as Pong, but with gravity affecting the ball.

### Snake

- Push the joystick in any direction to move the snake.
- Try to eat as much food as possible without colliding with your own snake's body.
- The playing field wraps around, so don't be afraid to go out-of-bounds.
