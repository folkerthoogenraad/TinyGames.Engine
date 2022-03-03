using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.Behaviours
{
    public struct GroundInfo
    {
        public bool IsSolid;
        public float Height;
        public GroundMaterial Material;
        public Vector2 Velocity;

        public static GroundInfo Solid(GroundMaterial material, float height = 0) => new GroundInfo() { Height = height, IsSolid = true, Velocity = Vector2.Zero, Material = material };
        public static GroundInfo Solid(GroundMaterial material, Vector2 velocity, float height = 0) => new GroundInfo() { Height = height, IsSolid = true, Velocity = velocity, Material = material };
        public static GroundInfo Empty() => new GroundInfo() { Height = -9999, IsSolid = false, Velocity = Vector2.Zero, Material = GroundMaterial.Void };
    }

    public enum GroundMaterial
    {
        Void,
        Snow,
        Wood,
        Water
    }

    public interface IWalkable
    {
        public GroundInfo GetGroundInfo(Vector2 point);
    }

    public class WalkablesSceneBehaviour : ISceneBehaviour
    {
        public Scene Scene { get; set; }
        private List<IWalkable> _walkables;

        public WalkablesSceneBehaviour()
        {
            _walkables = new List<IWalkable>();
        }

        public GroundInfo GetGroundInfo(Vector2 position)
        {
            return _walkables.Select(x => x.GetGroundInfo(position)).OrderByDescending(x => x.Height).FirstOrDefault();
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
        public static WalkablesSceneBehaviour GetWalkables(this Scene scene)
        {
            return scene.GetBehaviour<WalkablesSceneBehaviour>();
        }
    }
}
