using System;

namespace TinyGames.Engine.Scenes
{
    public class ResourceSceneBehaviour<T> : SceneBehaviour
    {
        public T Data { get; private set; }

        public ResourceSceneBehaviour(T data)
        {
            Data = data;
        }
    }

    public static class ResourceSceneBehaviourExtensions
    {
        public static ResourceSceneBehaviour<T> GetResourceSceneBehaviour<T>(this Scene scene)
        {
            return scene.GetBehaviour<ResourceSceneBehaviour<T>>();
        }

        public static T GetResource<T>(this Scene scene)
        {
            var behaviour = GetResourceSceneBehaviour<T>(scene);

            return behaviour.Data;
        }

        public static void SetResource<T>(this Scene scene, T data)
        {
            var behaviour = GetResourceSceneBehaviour<T>(scene);

            if (behaviour != null) throw new ApplicationException("Resource already registered");

            scene.AddBehaviour(new ResourceSceneBehaviour<T>(data));
        }
        public static void SetResource<TInterface, TData>(this Scene scene, TData data) where TData : TInterface
        {
            var behaviour = GetResourceSceneBehaviour<TInterface>(scene);

            if (behaviour != null) throw new ApplicationException("Resource already registered");

            scene.AddBehaviour(new ResourceSceneBehaviour<TInterface>(data));
        }
    }
}
