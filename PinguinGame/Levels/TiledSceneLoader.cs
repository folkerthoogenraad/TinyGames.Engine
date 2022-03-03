using PinguinGame.Levels.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Levels
{
    public class TiledSceneLoaderRegister
    {
        private Dictionary<string, Func<TiledObject, GameObject>> _objectTypes;

        public Action<TiledLevel, Scene> OnInit;
        public Action<TiledLayer, Scene> OnLoadLayer;
        public Action<TiledObject, Scene> OnObjectLoad;

        public Action<TiledObject, Scene, GameObject> OnObjectLoaded;

        public TiledSceneLoaderRegister()
        {
            _objectTypes = new Dictionary<string, Func<TiledObject, GameObject>>();
        }

        public bool IsObjectTypeRegistered(string type)
        {
            return _objectTypes.ContainsKey(type);
        }
        public void RegisterObjectType(string type, Func<TiledObject, GameObject> function)
        {
            _objectTypes.Add(type, function);
        }
        public GameObject CreateObject(TiledObject obj)
        {
            if (obj == null) throw new ArgumentException("Illegal call to create object");
            if (!_objectTypes.TryGetValue(obj.Type, out var loader)) throw new ArgumentException($"No loader registered for object of type ${obj.Type}.");

            return loader(obj);
        }
    }

    public class TiledSceneLoader
    {
        private TiledLevel _level;
        private TiledSceneLoaderRegister _register;

        public TiledSceneLoader(TiledLevel level, TiledSceneLoaderRegister register)
        {
            _level = level;
            _register = register;
        }

        public Scene Load(IServiceProvider services)
        {
            var scene = new Scene(services);

            _register.OnInit?.Invoke(_level, scene);

            foreach (var layer in _level.Layers)
            {
                _register.OnLoadLayer?.Invoke(layer, scene);

                if (!layer.IsObjectLayer) continue;

                foreach(var obj in layer.Objects)
                {
                    _register.OnObjectLoad?.Invoke(obj, scene);

                    var result = _register.CreateObject(obj);

                    if(result != null)
                    {
                        scene.AddGameObject(result);
                    }
                    
                    _register.OnObjectLoaded?.Invoke(obj, scene, result);
                }
            }

            return scene;
        }
    }
}
