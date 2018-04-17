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
        hero _hero;
        menu MENU;
        GameState gameState = GameState.Menu;

        private environment _environment;
        private bots _bots;

        public Game1()
        {
          graphics = new GraphicsDeviceManager(this);
          Content.RootDirectory = "Content";
          graphics.PreferredBackBufferWidth = 1000; // ширина приложения
          graphics.PreferredBackBufferHeight = 640; // высота приложения
          graphics.IsFullScreen = false; // флаг полноэкранного приложения
          graphics.ApplyChanges(); // применяем параметры
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

            this._environment = new environment(true);
            _environment.Load(Content);
            _environment.create_world();

            this._bots = new bots(_environment);
            _bots.Load(Content);

            this._hero = new hero(5, _environment);
            _hero.Load(Content);

            _bots.create_bots();
        }

        protected override void LoadContent()
        {
          MENU.Load(Content);
          spriteBatch = new SpriteBatch(GraphicsDevice);
          heroBatch = new SpriteBatch(graphics.GraphicsDevice);
          botsBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.Menu;

            if (gameState == GameState.Game)
            {
              _environment.Update(gameTime);
              _hero.Update(gameTime);
              _bots.Update(gameTime);

              Window.Title = "BomberMan. Level " + _environment.curent_lvl + ". frags: " + _environment.creep_kill;
            }
            else {
              MENU.Update();
              }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            if (gameState == GameState.Game)
            {
              _hero.DrawAnimation(heroBatch);
              _bots.DrawAnimation(heroBatch);
            }
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
