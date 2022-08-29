using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGames.Engine
{
    /// <summary>
    /// A class you can inherit from when creating your game. It is not required for using TinyGames engine. It extends from the regular monogame Game class and adds some additional functionality.
    /// </summary>
    public class TinyGame : Game
    {
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        public int WindowWidth => GraphicsDevice.PresentationParameters.BackBufferWidth;
        public int WindowHeight => GraphicsDevice.PresentationParameters.BackBufferHeight;

        public int ScreenWidth => GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        public int ScreenHeight => GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        public TinyGame()
        {
            GraphicsDeviceManager = CreateDefaultGraphicsDeviceManager();
            GraphicsDeviceManager.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Creates a new instance of a graphics device manager. This gets called from the TinyGame constructor. Shouldn't be called manually, but can be overwritten if you have specific requirements for the graphics device manager constructor.
        /// </summary>
        /// <returns>A new instance of the graphics device manager.</returns>
        protected virtual GraphicsDeviceManager CreateDefaultGraphicsDeviceManager()
        {
            return new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef,
            };

        }

        /// <summary>
        /// Sets the target framerate to a number of FPS. By default the framerate is set to 60fps.
        /// 
        /// If setFixedTimeStep is true, 
        /// </summary>
        /// <param name="fps">The frames per second.</param>
        /// <param name="setFixedTimeStep">Whether it should overwrite the fixed timestep value. </param>
        public void SetTargetFPS(double fps, bool setFixedTimeStep = true)
        {
            if(fps > 0)
            {
                TargetElapsedTime = TimeSpan.FromSeconds(1.0 / fps);
            }

            if (setFixedTimeStep)
            {
                IsFixedTimeStep = fps > 0;
            }
        }

        // This bit is a bit ugly because I don't want to set/overwrite paramters that the user didn't specify
        private int? _width;
        private int? _height;
        private bool? _fullscreen;
        private int? _msaa;
        private RenderTargetUsage? _renderTargetUsage;
        private DepthFormat? _depthFormat;
        private DisplayOrientation? _orientation;
        private SurfaceFormat? _surfaceFormat;
        private PresentInterval? _presentationInterval;

        public void SetGraphicsProperties(
            int? width = null, 
            int? height = null, 
            bool? fullscreen = null,
            int? msaa = null,
            bool? vsync = null,
            RenderTargetUsage? renderTargetUsage = null,
            DepthFormat? depthFormat = null,
            DisplayOrientation? orientation = null,
            SurfaceFormat? surfaceFormat = null,
            PresentInterval? presentationInterval = null)
        {
            _width = width;
            _height = height;
            _fullscreen = fullscreen;
            _msaa = msaa;
            _renderTargetUsage = renderTargetUsage;
            _depthFormat = depthFormat;
            _orientation = orientation;
            _surfaceFormat = surfaceFormat;
            _presentationInterval = presentationInterval;

            // Vsync has to be set directly :')
            if (vsync.HasValue) GraphicsDeviceManager.SynchronizeWithVerticalRetrace = vsync.Value;
            if(depthFormat.HasValue) GraphicsDeviceManager.PreferredDepthStencilFormat = depthFormat.Value;
            if(width.HasValue) GraphicsDeviceManager.PreferredBackBufferWidth = width.Value;
            if(height.HasValue) GraphicsDeviceManager.PreferredBackBufferHeight= height.Value;
            if(surfaceFormat.HasValue) GraphicsDeviceManager.PreferredBackBufferFormat = surfaceFormat.Value;
            if (msaa.HasValue) GraphicsDeviceManager.PreferMultiSampling = msaa.Value > 0;

            GraphicsDeviceManager.ApplyChanges();
        }

        private void ResetWindowProperties()
        {
            _width = null;
            _height = null;
            _fullscreen = null;
            _msaa = null;
            _renderTargetUsage = null;
            _depthFormat = null;
            _orientation = null;
            _surfaceFormat = null;
            _presentationInterval = null;
        }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var parameters = e.GraphicsDeviceInformation.PresentationParameters;

            if(_width.HasValue) parameters.BackBufferWidth = _width.Value;
            if(_height.HasValue) parameters.BackBufferHeight = _height.Value;
            if(_fullscreen.HasValue) parameters.IsFullScreen = _fullscreen.Value;
            if(_msaa.HasValue) parameters.MultiSampleCount = _msaa.Value;

            if(_renderTargetUsage.HasValue) parameters.RenderTargetUsage = _renderTargetUsage.Value;
            if(_depthFormat.HasValue) parameters.DepthStencilFormat = _depthFormat.Value;
            if(_orientation.HasValue) parameters.DisplayOrientation = _orientation.Value;
            if(_surfaceFormat.HasValue) parameters.BackBufferFormat = _surfaceFormat.Value;
            if(_presentationInterval.HasValue) parameters.PresentationInterval = _presentationInterval.Value;

            // Prevent overwriting again
            ResetWindowProperties();
        }
    }
}
