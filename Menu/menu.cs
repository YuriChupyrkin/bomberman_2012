using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BomberMan
{
    class menu
    {
        public List<MenuItems> Items;
        SpriteFont menuFont;
        int curentItem;
        KeyboardState oldState;
        public Texture2D backTex;
        public Vector2 backPos;

        public menu()
        {
            Items = new List<MenuItems>();
        }

        public void Load(ContentManager Content)
        {
            menuFont = Content.Load<SpriteFont>(@"menuFont");
            backTex = Content.Load<Texture2D>(@"Textures/background");
            backPos = new Vector2(0, 0);
        }

        public void Update()
        {
            KeyboardState State = Keyboard.GetState();

            if (State.IsKeyDown(Keys.Enter))
                Items[curentItem].OnClick();

            int delta = 0;

            if (State.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
                delta = -1;

            if (State.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
                delta = 1;

            curentItem += delta;

            bool ok = false;

            while (!ok)
            {
                if (curentItem < 0)
                    curentItem = Items.Count - 1;
                else if (curentItem > Items.Count - 1)
                    curentItem = 0;
                else if (Items[curentItem].active == false)
                    curentItem += delta;
                else
                    ok = true;
            } 

            oldState = State;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backTex, backPos, Color.White);

            int y = 200;
            foreach (MenuItems X in Items)
            {
                Color color = Color.White;
                if (X.active == false)
                    color = Color.Black;
                if (X == Items[curentItem])
                    color = Color.Red;

                spriteBatch.DrawString(menuFont, X.name, new Vector2(30, y), color);
                y += 100;
            }

            spriteBatch.End();
        }
    }
}
