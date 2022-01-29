using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame
{
    public interface IScreenService
    {
        public void Exit();

        public void ShowSplashScreen();
        public void ShowTitleScreen();
        public void ShowMenuScreen();
        public void ShowPlayerSelectScreen();
        public void ShowCharacterSelectScreen(PlayerInfo[] players);
        public void ShowMapSelectScreen(PlayerInfo[] players);
        public void ShowInGameScreen(PlayerInfo[] players);
        public void ShowResultScreen(Fight fight);
    }
}
