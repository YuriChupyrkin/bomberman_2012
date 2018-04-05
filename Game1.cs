using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BomberMan
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteBatch heroBatch;
        SpriteBatch botsBatch;
        hero HERO;
        menu MENU;
        GameState gameState = GameState.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000; // ширина приложения
            graphics.PreferredBackBufferHeight = 640; // высота приложения
            graphics.IsFullScreen = false; // флаг полноэкранного приложения
            graphics.ApplyChanges(); // применяем параметры
            HERO = new hero(5);

        }

        protected override void Initialize()
        {

            //HERO.create_world();
            MENU = new menu();
            MenuItems NewGame = new MenuItems("New Game");
            MenuItems Resume = new MenuItems("Resume Game");
            MenuItems Exit = new MenuItems("Exit");

            Resume.active = false;

            NewGame.Click += new EventHandler(NewGame_Click);
            Resume.Click += new EventHandler(Resume_Click);
            Exit.Click += new EventHandler(Exit_Click);

            MENU.Items.Add(NewGame);
            MENU.Items.Add(Resume);
            MENU.Items.Add(Exit);

            base.Initialize();

        }

        void Exit_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        void Resume_Click(object sender, EventArgs e)
        {
            gameState = GameState.Game;
        }

        void NewGame_Click(object sender, EventArgs e)
        {
            MENU.Items[1].active = true;
            gameState = GameState.Game;
            HERO.WIN = false;
            HERO.curent_lvl = 0;
            HERO.creep_kill = 0;
            HERO.player_pos_i = 1;
            HERO.player_pos_j = 1;
            HERO.player_dead = false;
            HERO.int_game_time = 0;
            HERO.bonus_tolk = false;
            HERO.rad_bang = 1;
            HERO.player_speed = 1;
            HERO.max_num_bomb = 1; 
            HERO.creep_speed = 1; 
            HERO.bot_add_time = 10000;
            HERO.create_world();
        }

        protected override void LoadContent()
        {
            MENU.Load(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            heroBatch = new SpriteBatch(graphics.GraphicsDevice);
            botsBatch = new SpriteBatch(graphics.GraphicsDevice);
            HERO.Load(Content);
        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.Menu;

            if (gameState == GameState.Game)
                HERO.Update(gameTime);
            else
                MENU.Update();

            Window.Title = "BomberMan. Level " + HERO.curent_lvl+". frags: " + HERO.creep_kill;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            if (gameState == GameState.Game)
                HERO.DrawAnimation(heroBatch);
            else
            {
                MENU.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }

        enum GameState
        {
            Game,
            Menu
        }
    }
}
