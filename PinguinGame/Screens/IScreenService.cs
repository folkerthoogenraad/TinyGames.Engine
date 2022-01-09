using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame
{
    public interface IScreenService
    {
        public void ShowPlayerSelectScreen();
        public void ShowInGameScreen();
        public void ShowResultScreen(Fight fight);
    }
}
