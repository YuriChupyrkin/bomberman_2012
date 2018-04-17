﻿using System;
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
    class bots//: environment
    {

        public Texture2D creepTex;
        public Texture2D creep_go_up_tex;
        public Texture2D creep_go_down_tex;
        public Texture2D creep_go_left_tex;
        public Texture2D creep_go_right_tex;
        public List<Creep> creep_list;

        private environment _environment;

        //конструктор
        public bots(environment environment)
        {
          this._environment = environment;
          creep_list = new List<Creep>();
        }

        //создаем в начале 3 бота
        public void create_bots()
        {
          if (_environment.curent_lvl == 1)
          {
            //creep_list.Add(new Creep(1, _environment.size2 - 2, this, _environment));
            creep_list.Add(new Creep(_environment.size1 - 2, 1, this, _environment));
            //creep_list.Add(new Creep(_environment.size1 - 2, _environment.size2 - 2, this, _environment));
          }

          if (_environment.curent_lvl == 2)
            {
              creep_list.Add(new Creep(1, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(2, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 3, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 3, _environment.size2 - 2, this, _environment));
            }

          if (_environment.curent_lvl == 3)
            {
              creep_list.Add(new Creep(1, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(1, _environment.size2 - 3, this, _environment));
              creep_list.Add(new Creep(2, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 3, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, _environment.size2 - 3, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 3, _environment.size2 - 2, this, _environment));
            }

          if (_environment.curent_lvl == 4)
            {
              creep_list.Add(new Creep(1, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(1, _environment.size2 - 3, this, _environment));
              creep_list.Add(new Creep(2, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(3, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 3, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 4, 1, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 2, _environment.size2 - 3, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 3, _environment.size2 - 2, this, _environment));
              creep_list.Add(new Creep(_environment.size1 - 4, _environment.size2 - 2, this, _environment));
            }

            //if (curent_lvl == 5)
            //{
            //    creep_list.Add(new creep(1, size2 - 2, this));
            //    creep_list.Add(new creep(1, size2 - 3, this));
            //    creep_list.Add(new creep(2, size2 - 2, this));
            //    creep_list.Add(new creep(3, size2 - 2, this));
            //    creep_list.Add(new creep(1, size2 - 4, this));
            //    creep_list.Add(new creep(size1 - 2, 1, this));
            //    creep_list.Add(new creep(size1 - 2, 2, this));
            //    creep_list.Add(new creep(size1 - 3, 1, this));
            //    creep_list.Add(new creep(size1 - 4, 1, this));
            //    creep_list.Add(new creep(size1 - 2, 3, this));
            //    creep_list.Add(new creep(size1 - 2, size2 - 2, this));
            //    creep_list.Add(new creep(size1 - 2, size2 - 3, this));
            //    creep_list.Add(new creep(size1 - 3, size2 - 2, this));
            //    creep_list.Add(new creep(size1 - 4, size2 - 2, this));
            //    creep_list.Add(new creep(size1 - 2, size2 - 4, this));
            //}

            //if (curent_lvl == 6)
            //{
            //    creep_list.Add(new creep(1, size2 - 2, this));
            //    creep_list.Add(new creep(1, size2 - 3, this));
            //    creep_list.Add(new creep(2, size2 - 2, this));
            //    creep_list.Add(new creep(3, size2 - 2, this));
            //    creep_list.Add(new creep(1, size2 - 4, this));
            //    creep_list.Add(new creep(size1 - 2, 1, this));
            //    creep_list.Add(new creep(size1 - 2, 2, this));
            //    creep_list.Add(new creep(size1 - 3, 1, this));
            //    creep_list.Add(new creep(size1 - 4, 1, this));
            //    creep_list.Add(new creep(size1 - 2, 3, this));
            //    creep_list.Add(new creep(size1 - 2, size2 - 2, this));
            //    creep_list.Add(new creep(size1 - 2, size2 - 3, this));
            //    creep_list.Add(new creep(size1 - 3, size2 - 2, this));
            //    creep_list.Add(new creep(size1 - 4, size2 - 2, this));
            //    creep_list.Add(new creep(size1 - 2, size2 - 4, this));
            //}

            //if (curent_lvl == 7)
            //{
            //    int I = size1 - 2;
            //    for(int j = 1; j < size2 - 1; j++)
            //        creep_list.Add(new creep(I, j, this));
            //    int J = size2 - 2;
            //    for(int i = 1; i < size1 - 2; i++)
            //        creep_list.Add(new creep(i, J, this));
            //}
        }

        //метод LOADBots
        public void Load(ContentManager Content)
        {
          creepTex = Content.Load<Texture2D>(@"Textures/creep");
          creep_go_down_tex = Content.Load<Texture2D>(@"Textures/go_down_creep");
          creep_go_up_tex = Content.Load<Texture2D>(@"Textures/go_up_creep");
          creep_go_left_tex = Content.Load<Texture2D>(@"Textures/go_left_creep");
          creep_go_right_tex = Content.Load<Texture2D>(@"Textures/go_right_creep");

          //create_bots();
        }

        public void Update(GameTime gameTime)
        {
          if (_environment.lvl_up == true)
          {
            create_bots();
          }

          UpdateBots(gameTime);
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
          DrawBots(spriteBatch);
        }

        // проверка постая ли ячейка, чтобы туда перейти
        public bool is_empty_for_bots(int I, int J)
        {
            bool empty = false;
            if (I > 0 && I < _environment.size1)
            {
              if (J > 0 && J < _environment.size2)
              {
                if (_environment.M[I, J] == " " || _environment.M[I, J] == "*" || _environment.M[I, J] == "P" || _environment.M[I, J] == "A" || _environment.M[I, J] == "T" || _environment.M[I, J] == "R" || _environment.M[I, J] == "S" || _environment.M[I, J] == "s")
                {
                  empty = true;
                }
              }
            }
            return empty;
        }

        //метод UpdateBots
        public void UpdateBots(GameTime gameTime)
        {
            foreach (Creep X in creep_list)
            {
                if (X.creep_dead == false)
                {
                    X.check_dead_creep(gameTime, this);
                    if (_environment.globalGameTime > 1)
                        X.lets_go_creep(gameTime, this);
                    if (X.creep_napravlenie == "up" || X.creep_napravlenie == "down" || X.creep_napravlenie == "right" || X.creep_napravlenie == "left")
                        X.bot_move(gameTime, this);
                    if (_environment.player_pos_i == X.creep_new_pos_i && _environment.player_pos_j == X.creep_new_pos_j)
                    {
                      _environment.player_dead = true;
                    }
                }
                if (X.creep_dead == true)
                {
                    if (X.creep_new_pos_i != -1 && X.creep_new_pos_j != -1)
                    {
                        _environment.M[X.creep_new_pos_i, X.creep_new_pos_j] = " ";
                        X.creep_new_pos_i = -1;
                        X.creep_new_pos_j = -1;
                    }
                }
            }

            int i = 0;
            while (i < creep_list.Count)
            {
                if (creep_list[i].creep_dead == true) 
                     creep_list.RemoveAt(i);
                else
                    i++;
            }

            if (all_creep_dead())
            {
              if (_environment.curent_lvl < 7)
              {
                _environment.lvl_up = true;
                create_bots();
              }
              else
              {
                _environment.WIN = true;
              }
            }


            if (_environment.need_add_bot)
            {
              _environment.need_add_bot = false;
                bot_add();
            }
               
        }

        //все крипы мертвы
        public bool all_creep_dead()
        {
          foreach (Creep X in creep_list)
          {
            if (X.creep_dead == false)
            {
              return false;
            }
          }
          return true;
        }


        //method DrawBots
        public void DrawBots(SpriteBatch spriteBatch)
        {

            foreach (Creep X in creep_list)
                if (X.creep_dead == false)
                {
                    X.DrawCreep(spriteBatch, this);
                }
        }



        // добавление бота
        protected void bot_add()
        {
            Random rdn = new Random();
            int added = 0;
            while (added == 0)
            {
              int I = rdn.Next(1, _environment.size1 - 2);
              int J = rdn.Next(1, _environment.size2 - 2);
              if (_environment.M[I, J] == " " && correct_pos(I, J) == true)
                {
                  _environment.M[I, J] = "C";
                    creep_list.Add(new Creep(I, J, this, _environment));
                    added++;
                }
            }
        }

        // чтобы не появлялся возле игрока
        bool correct_pos(int I, int J)
        {
            bool player = false;
            if ((J - 2) > 1 && (J + 2) < _environment.size2)
            {
              if ((I - 2) > 1 && (I + 2) < _environment.size1)
                {
                  for (int i = (I - 2); i <= (I + 2); i++)
                  {
                    for (int j = (J - 2); j <= (J + 2); j++)
                    {
                      if (_environment.M[i, j] == "P")
                      {
                        player = true;
                      }
                    }
                  }
                }
                else
                    player = true;
            }
            else
                player = true;

            if (player == true)
                return false;
            else
                return true;
        }
    }
}
