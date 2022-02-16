using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Scenes;

namespace PinguinGame.MiniGames.Ice
{
    public struct GroundInfo
    {
        // TODO add velocity and whatnot here.
        public bool Solid;
        public float Height;
        public Vector2 Velocity;
    }

    public interface IWalkable
    {
        public float Height { get; }
        public Vector2 Velocity { get; }
        public bool PointInside(Vector2 point);
    }

    public class Walkables : ISceneBehaviour
    {
        // TODO remove the level part of this :)
        public IceLevel Level { get; set; }

        public Scene Scene { get; set; }
        private List<IWalkable> _walkables;

        public Walkables(IceLevel level)
        {
            Level = level;
            _walkables = new List<IWalkable>();
        }

        public GroundInfo GetGroundInfo(Vector2 position)
        {
            // This function became really ugly because of the two different sources.
            // TODO move the ice blocks to the walkables and we're done.

            var block = Level.GetIceBlockForPosition(position, 0);

            var walkable = _walkables.Where(x => x.PointInside(position)).OrderByDescending(x => x.Height).FirstOrDefault();

            if(walkable != null)
            {
                if(block != null)
                {
                    if (block.Height > walkable.Height)
                    {
                        return new GroundInfo() { Height = block.Height, Solid = true, Velocity = block.Velocity };
                    }
                    else
                    {
                        return new GroundInfo() { Height = walkable.Height, Solid = true, Velocity = walkable.Velocity };
                    }
                }
                else
                {
                    return new GroundInfo() { Height = walkable.Height, Solid = true, Velocity = walkable.Velocity };
                }
            }
            else if(block != null)
            {
                return new GroundInfo() { Height = block.Height, Solid = block.Solid, Velocity = block.Velocity };
            }

            return new GroundInfo() { Height = 0, Solid = false, Velocity = Vector2.Zero };
        }

        public void Init(Scene scene)
        {
            Scene = scene;

            Scene.OnGameObjectCreated += OnGameObjectCreated;
            Scene.OnGameObjectDestroyed += OnGameObjectDestroyed;
        }

        public void Destroy()
        {
            Scene.OnGameObjectCreated -= OnGameObjectCreated;
            Scene.OnGameObjectDestroyed -= OnGameObjectDestroyed;
        }

        public void AfterUpdate(float delta)
        {
            // Do nothing
        }

        public void BeforeUpdate(float delta)
        {
            // Do nothing
        }

        public void OnGameObjectCreated(Scene scene, GameObject obj)
        {
            if (obj is IWalkable) AddManualWalkable(obj as IWalkable);
        }
        public void OnGameObjectDestroyed(Scene scene, GameObject obj)
        {
            if (obj is IWalkable) RemoveManualWalkable(obj as IWalkable);
        }

        public void AddManualWalkable(IWalkable walkable)
        {
            _walkables.Add(walkable);
        }

        public void RemoveManualWalkable(IWalkable walkable)
        {
            _walkables.Remove(walkable);
        }

    }

    public static class WalkablesExtensions
    {
        public static Walkables GetWalkables(this Scene scene)
        {
            return scene.GetBehaviour<Walkables>();
        }
    }
}
