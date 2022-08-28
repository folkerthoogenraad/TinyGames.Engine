# TinyGames.Engine
TinyGames.Engine a set of classes and helpers for creating games in MonoGame. It features abstractions for common use cases for making 2D games.

## Installing
Get the engine via Nuget:
```bash
nuget install TinyGames.Engine
```

## Usage
This documentation provides an outline of general features and problems you'd want to solve using the TinyGames.Engine libraries. 

## Examples
TinyGames features things like Linq extensions specially for games:

```csharp
var list = new List<int>(){ 0, 1, 2};

foreach(var (from, to) in list.Pairs()){
    Console.WriteLine($"{from}, {to}"); // (0, 1) (1, 2)
}
foreach(var (from, to) in list.Loop()){
    Console.WriteLine($"{from}, {to}"); // (0, 1) (1, 2) (2, 0)
}

var i = list.Random();
list.Shuffle();
```

And easy to use graphics libraries:
```csharp
Graphics2D graphics = new Graphics2D(GraphicsDevice);
graphics.Begin(Camera.GetMatrix());

graphics.DrawLine(...);
graphics.DrawRectangle(...);

graphics.DrawSprite(...);

graphics.End();
```

## Docs state
 - [ ] TinyGames.Engine
 - [ ] TinyGames.Engine.Graphics
 - [ ] TinyGames.Engine.Animations
 - [ ] TinyGames.Engine.Collisions