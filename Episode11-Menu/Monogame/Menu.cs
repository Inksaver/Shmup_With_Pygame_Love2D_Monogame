using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shmup
{
    internal static class Menu
    {
        public static void Update(KeyboardState keyboardState)
        {
            /// TextInputHandler(object sender, TextInputEventArgs args)
            /// in ShmupMain.cs used to handle keystokes
        }
        public static void Draw(SpriteBatch spriteBatch, string text)
        {
            Shared.DrawString(spriteBatch, text:text, size:"size50", posX:0, posY:Shared.HEIGHT * 0.25f, Color.Yellow, "centre");
            Shared.DrawString(spriteBatch, "Arrow keys move, space to fire", size: "size20", posX: 0, posY: Shared.HEIGHT * 0.5f, Color.White, "centre", scale: 0.8f);
            Shared.DrawString(spriteBatch, "Press Enter to begin", size: "size30", posX: 0, posY: Shared.HEIGHT * 0.65f, Color.White, "centre", scale: 0.74f);
            Shared.DrawString(spriteBatch, "Escape to quit at any time", size: "size18", posX: 0, posY: Shared.HEIGHT * 0.85f, Color.White, "centre", scale: 0.74f);
            Shared.DrawString(spriteBatch, "Press B to toggle background music", size: "size16", posX: 0, posY: Shared.HEIGHT * 0.9f, Shared.GREEN, "centre", scale: 0.74f);
        }
    }
}
