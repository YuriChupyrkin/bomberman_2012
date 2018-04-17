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
using System.Threading;

namespace BomberMan
{
  class hero
  {
    public string napravlenie;

    double time_;
    int count_steps;
    bool player_moving;
    public double dy;
    KeyboardState oldState;
    string napr_for_draw;

    public Rectangle hero_rec;
    public Rectangle hero_rec2;
    public Texture2D Hero_texture;
    public Texture2D Hero_go_left_tex;
    public Texture2D Hero_go_right_tex;
    public Texture2D Hero_go_down_tex;
    public Texture2D Hero_go_up_tex;
    public Vector2 Hero_pos;

    public List<Bomb> bomb_list;
    private environment _environment;

    //конструктор
    public hero(int speedAnimation, environment environment)
    {
      this._environment = environment;
      napravlenie = "stop";
      time_ = 0;
      count_steps = 0;
      player_moving = false;
      bomb_list = new List<Bomb>();
      _environment.GlobalGT = null;
      napr_for_draw = "stop";
    }

    public void Load(ContentManager Content)
    {
      Hero_texture = Content.Load<Texture2D>(@"Textures/hero");
      Hero_go_left_tex = Content.Load<Texture2D>(@"Textures/go_left_hero");
      Hero_go_right_tex = Content.Load<Texture2D>(@"Textures/go_right_hero");
      Hero_go_up_tex = Content.Load<Texture2D>(@"Textures/go_up_hero");
      Hero_go_down_tex = Content.Load<Texture2D>(@"Textures/go_down_hero");
      Hero_pos = new Vector2(40, 40);
    }

    // анимация при передвижении
    public void Moving(GameTime gametime, string naprv)
    {
      napravlenie = naprv;
      if (naprv == "right")
      {
        Hero_pos.X += _environment.player_speed;
        hero_rec2 = new Rectangle((int)Hero_pos.X, (int)Hero_pos.Y, Hero_texture.Width, Hero_texture.Height);
      }
      if (naprv == "left")
      {
        Hero_pos.X -= _environment.player_speed;
        hero_rec2 = new Rectangle((int)Hero_pos.X, (int)Hero_pos.Y, Hero_texture.Width, Hero_texture.Height);
      }
      if (naprv == "down")
      {
        Hero_pos.Y += _environment.player_speed;
        hero_rec2 = new Rectangle((int)Hero_pos.X, (int)Hero_pos.Y, Hero_texture.Width, Hero_texture.Height);
      }
      if (naprv == "up")
      {
        Hero_pos.Y -= _environment.player_speed;
        hero_rec2 = new Rectangle((int)Hero_pos.X, (int)Hero_pos.Y, Hero_texture.Width, Hero_texture.Height);
      }
      _environment.TotalTime += (float)gametime.ElapsedGameTime.TotalSeconds;
      if (_environment.TotalTime > _environment.TimeForFrame)
      {
        _environment.frame++;
        _environment.frame = _environment.frame % (_environment.FrameCount - 1);
        _environment.TotalTime -= _environment.TimeForFrame;
      }
    }


    // функция апдейта
    public void Update(GameTime gameTime)
    {
      KeyboardState State = Keyboard.GetState();

      if (_environment.lvl_up == true)
      {
        bomb_list.Clear();
        Hero_pos = new Vector2(40, 40);
      }

      int i = 0;
      while (i < bomb_list.Count)
      {
        if (bomb_list[i].fired == true)
          bomb_list.RemoveAt(i);
        else
          i++;
      }

      count_bomb();

      foreach (Bomb X in bomb_list)
        if (X.bomb_roll_napr != "stop")
          X.rolling_2(gameTime);

      foreach (Bomb X in bomb_list)
        if (X.fire == true)
          X.bang_fire(gameTime, this);

      if (_environment.lvl_up == false && _environment.key_lock == false && _environment.WIN == false)
      {
        if (Keyboard.GetState().IsKeyDown(Keys.Right) && _environment.player_dead == false)
        {
          if (player_moving == false)
          {
            if (is_empty(_environment.player_pos_i, _environment.player_pos_j + 1))
            {
              player_moving = true;
              this.napravlenie = "right";
            }
          }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left) && _environment.player_dead == false)
        {
          if (player_moving == false)
          {
            if (is_empty(_environment.player_pos_i, _environment.player_pos_j - 1))
            {
              player_moving = true;
              this.napravlenie = "left";
            }
          }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down) && _environment.player_dead == false)
        {
          if (player_moving == false)
          {
            if (is_empty(_environment.player_pos_i + 1, _environment.player_pos_j))
            {
              player_moving = true;
              this.napravlenie = "down";
            }
          }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up) && _environment.player_dead == false)
        {
          if (player_moving == false)
            if (is_empty(_environment.player_pos_i - 1, _environment.player_pos_j))
            {
              player_moving = true;
              this.napravlenie = "up";
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space) && _environment.player_dead == false)
          if (check_bomb_pos(_environment.player_pos_i, _environment.player_pos_j))
          {
            if (_environment.curent_num_bomb < _environment.max_num_bomb)
            {
              bomb_list.Add(new Bomb(_environment, _environment.player_pos_i, _environment.player_pos_j, 0));
              _environment.M[_environment.player_pos_i, _environment.player_pos_j] = "B";
              _environment.plant_bomb_mus.Play(0.1f, 0.0f, 0.0f);
            }
          }
      }

      if (this.napravlenie != "stop")
        move(gameTime);

      //тут будем передавать постоянно игровое время для бомб
      foreach (Bomb X in bomb_list)
        if (X.bomb_pos_i != -1 && X.bomb_pos_j != -1)
          X.bomb_time += (float)gameTime.ElapsedGameTime.TotalSeconds;

      foreach (Bomb X in bomb_list)
        if (X.banged == false)
          if (X.bomb_pos_i != -1 && X.bomb_pos_j != -1)
          {
            X.bang_control(gameTime, this);
          }

      float t = (float)gameTime.TotalGameTime.TotalSeconds * 3;
      dy = (int)(Math.Sin(t) * 5);


      if (napravlenie == "stop" && !Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
        napr_for_draw = "stop";

      if (napravlenie == "left")
        napr_for_draw = "left";

      if (napravlenie == "right")
        napr_for_draw = "right";

      if (napravlenie == "up")
        napr_for_draw = "up";

      if (napravlenie == "down")
        napr_for_draw = "down";

      oldState = State;
    }

    //проверяем, установлена ли бомба в этой ячейке
    public bool check_bomb_pos(int I, int J)
    {
      foreach (Bomb X in bomb_list)
        if (X.bomb_pos_i == I && X.bomb_pos_j == J)
          return false;
      return true;
    }

    //подсчет бобм
    public void count_bomb()
    {
      _environment.curent_num_bomb = 0;
      for (int i = 0; i < _environment.size1; i++)
      {
        for (int j = 0; j < _environment.size2; j++)
        {
          if (_environment.M[i, j] == "B")
          {
            _environment.curent_num_bomb++;
          }
        }
      }
    }

    //рисование
    public void DrawAnimation(SpriteBatch spriteBatch)
    {

      for (int i = 0; i < _environment.size1; i++)
        for (int j = 0; j < _environment.size2; j++)
        {
          if (_environment.M[i, j] == "0")
          {
            _environment.wallPos.Y = _environment.Y_cell * i;
            _environment.wallPos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.wallTex, _environment.wallPos, Color.White);
            spriteBatch.End();
          }

          if (_environment.M[i, j] == "W")
          {
            _environment.brickPos.Y = _environment.Y_cell * i;
            _environment.brickPos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.brickTex, _environment.brickPos, Color.White);
            spriteBatch.End();
          }

          if (_environment.M[i, j] == "*")
          {
            _environment.firePos.Y = _environment.Y_cell * i;
            _environment.firePos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.fireTex, _environment.firePos, Color.White);
            spriteBatch.End();
          }

          if (_environment.M[i, j] == "T")
          {
            _environment.bon_tolk_Pos.Y = _environment.Y_cell * i + (int)dy;
            _environment.bon_tolk_Pos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.bon_tolk_Tex, _environment.bon_tolk_Pos, Color.White);
            spriteBatch.End();
          }

          if (_environment.M[i, j] == "R")
          {
            _environment.bon_rad_Pos.Y = _environment.Y_cell * i + (int)dy;
            _environment.bon_rad_Pos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.bon_rad_Tex, _environment.bon_rad_Pos, Color.White);
            spriteBatch.End();
          }

          if (_environment.M[i, j] == "A")
          {
            _environment.bon_bomb_Pos.Y = _environment.Y_cell * i + (int)dy;
            _environment.bon_bomb_Pos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.bon_bomb_Tex, _environment.bon_bomb_Pos, Color.White);
            spriteBatch.End();
          }


          if (_environment.M[i, j] == "S")
          {
            _environment.bon_speed_Pos.Y = _environment.Y_cell * i + (int)dy;
            _environment.bon_speed_Pos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.bon_speed_Tex, _environment.bon_speed_Pos, Color.White);
            spriteBatch.End();
          }

          if (_environment.M[i, j] == "s")
          {
            _environment.bon_speed_bot_Pos.Y = _environment.Y_cell * i + (int)dy;
            _environment.bon_speed_bot_Pos.X = _environment.Y_cell * j;
            spriteBatch.Begin();
            spriteBatch.Draw(_environment.bon_speed_bot_Tex, _environment.bon_speed_bot_Pos, Color.White);
            spriteBatch.End();
          }

          /*  if (M[i, j] == "c")
            {
                tmpPos.Y = Y_cell * i;
                tmpPos.X = Y_cell * j;
                spriteBatch.Begin();
                spriteBatch.Draw(tmpTex, tmpPos, Color.White);
                spriteBatch.End();
            } */
        }

      foreach (Bomb X in bomb_list)
      {
        _environment.bombPos.Y = _environment.Y_cell * X.bomb_pos_i;
        _environment.bombPos.X = _environment.Y_cell * X.bomb_pos_j;
        spriteBatch.Begin();
        spriteBatch.Draw(_environment.bombTex, _environment.bombPos, Color.White);
        spriteBatch.End();
      }

      if (!_environment.player_dead)
      {
        int frameWidth = Hero_go_right_tex.Width / _environment.FrameCount;
        hero_rec = new Rectangle(frameWidth * _environment.frame, 0, frameWidth, Hero_go_right_tex.Height);
        spriteBatch.Begin();

        if (napr_for_draw == "stop")
          spriteBatch.Draw(Hero_texture, Hero_pos, Color.White);

        if (napr_for_draw == "right")
          spriteBatch.Draw(Hero_go_right_tex, Hero_pos, hero_rec, Color.White);

        if (napr_for_draw == "left")
          spriteBatch.Draw(Hero_go_left_tex, Hero_pos, hero_rec, Color.White);

        if (napr_for_draw == "down")
          spriteBatch.Draw(Hero_go_down_tex, Hero_pos, hero_rec, Color.White);

        if (napr_for_draw == "up")
        {
          spriteBatch.Draw(Hero_go_up_tex, Hero_pos, hero_rec, Color.White);
        }
        if (_environment.key_lock == true)
        {
          if (_environment.curent_lvl != 7)
          {
            _environment.LevelFontPos = new Vector2(250, 250);
            spriteBatch.DrawString(_environment.LevelFont, "Level " + _environment.curent_lvl, _environment.LevelFontPos, Color.DarkGreen);
          }
          else
          {
            _environment.LevelFontPos = new Vector2(40, 250);
            spriteBatch.DrawString(_environment.LevelFont, "Final Level", _environment.LevelFontPos, Color.DarkGreen);
          }
        }
        spriteBatch.End();
      }

      if (_environment.player_dead)
      {
        spriteBatch.Begin();
        _environment.LevelFontPos = new Vector2(40, 250);
        spriteBatch.DrawString(_environment.LevelFont, "GAME OVER!", _environment.LevelFontPos, Color.Red);
        spriteBatch.End();
      }

      if (_environment.WIN)
      {
        spriteBatch.Begin();
        _environment.LevelFontPos = new Vector2(150, 250);
        spriteBatch.DrawString(_environment.LevelFont, "You win!!!", _environment.LevelFontPos, Color.Red);
        spriteBatch.End();
      }
    }


    // перемещение на экране 
    void move(GameTime gameTime)
    {
      double interval = 10;
      time_ += (float)gameTime.ElapsedGameTime.Milliseconds;
      if (time_ > interval)
      {
        time_ = 0;
        Moving(gameTime, napravlenie);
        count_steps++;
      }

      if (count_steps * _environment.player_speed >= _environment.X_cell)
      {
        count_steps = 0;
        if (napravlenie == "right")
          _environment.move_in_mas(0, 1);
        if (napravlenie == "left")
          _environment.move_in_mas(0, -1);
        if (napravlenie == "down")
          _environment.move_in_mas(1, 0);
        if (napravlenie == "up")
          _environment.move_in_mas(-1, 0);
        napravlenie = "stop";
        player_moving = false;
      }
    }

    // проверка постая ли ячейка, чтобы туда перейти
    public bool is_empty(int I, int J)
    {
      bool empty = false;
      if (I > 0 && I < _environment.size1)
        if (J > 0 && J < _environment.size2)
          if (_environment.M[I, J] == " " || _environment.M[I, J] == "*" || _environment.M[I, J] == "c" || _environment.M[I, J] == "C" || _environment.M[I, J] == "A" || _environment.M[I, J] == "T" || _environment.M[I, J] == "R" || _environment.M[I, J] == "S" || _environment.M[I, J] == "s")
            empty = true;
      if (_environment.M[I, J] == "B")
      {
        if (_environment.bonus_tolk == true)
          start_tolk_bomb(I, J);
      }

      return empty;
    }


    //// проверка постая ли ячейка, чтобы туда перейти
    //protected bool is_empty_for_bomb(int I, int J)
    //{
    //  bool empty = false;
    //  if (I > 0 && I < _environment.size1)
    //    if (J > 0 && J < _environment.size2)
    //      if (_environment.M[I, J] == " " || _environment.M[I, J] == "*" || _environment.M[I, J] == "A" || _environment.M[I, J] == "T" || _environment.M[I, J] == "R" || _environment.M[I, J] == "S" || _environment.M[I, J] == "s")
    //      {
    //        empty = true;
    //      }
    //  if (_environment.M[I, J] == "B")
    //  {
    //    if (_environment.bonus_tolk == true)
    //      start_tolk_bomb(I, J);
    //  }

    //  return empty;
    //}


    // толкание бомбы
    public void start_tolk_bomb(int I, int J)
    {
      for (int i = 0; i < _environment.size1; i++)
      {
        for (int j = 0; j < _environment.size2; j++)
        {
          if (i == I && j == J)
          {
            if (_environment.M[i - 1, j] == "P")
            {
              foreach (Bomb X in bomb_list)
                if (X.bomb_pos_i == I && X.bomb_pos_j == J)
                {
                  X.bomb_roll_napr = "down";
                  X.bomb_roll_i = i;
                  X.bomb_roll_j = j;
                }
            }
            if (_environment.M[i + 1, j] == "P")
            {
              foreach (Bomb X in bomb_list)
                if (X.bomb_pos_i == I && X.bomb_pos_j == J)
                {
                  X.bomb_roll_napr = "up";
                  X.bomb_roll_i = i;
                  X.bomb_roll_j = j;
                }
            }
            if (_environment.M[i, j + 1] == "P")
            {
              foreach (Bomb X in bomb_list)
                if (X.bomb_pos_i == I && X.bomb_pos_j == J)
                {
                  X.bomb_roll_napr = "right";
                  X.bomb_roll_i = i;
                  X.bomb_roll_j = j;
                }
            }
            if (_environment.M[i, j - 1] == "P")
            {
              foreach (Bomb X in bomb_list)
              {
                if (X.bomb_pos_i == I && X.bomb_pos_j == J)
                {
                  X.bomb_roll_napr = "left";
                  X.bomb_roll_i = i;
                  X.bomb_roll_j = j;
                }
              }
            }
          }
        }
      }
    }




    //public class bomb
    //{
    //  public int bomb_pos_i;
    //  public int bomb_pos_j;
    //  public double bomb_time;
    //  public bool banged;
    //  public string bomb_roll_napr;
    //  public int bomb_roll_i;
    //  public int bomb_roll_j;
    //  public double bomb_time_roll;
    //  public double curent_time_bomb;
    //  public bool fire;
    //  public int rad_fire;
    //  public double fire_time;
    //  public int fire_pos_i;
    //  public int fire_pos_j;
    //  public bool left_fire;
    //  public bool right_fire;
    //  public bool up_fire;
    //  public bool down_fire;
    //  public bool right_space;
    //  public bool left_space;
    //  public bool up_space;
    //  public bool down_space;
    //  public bool fired;


    //  //конструктор
    //  public bomb(int i, int j, double t)
    //  {
    //    bomb_pos_i = i;
    //    bomb_pos_j = j;
    //    bomb_time = t;
    //    banged = false;
    //    fire = false;
    //    bomb_roll_napr = "stop";
    //    bomb_roll_i = -1;
    //    bomb_roll_j = -1;
    //    bomb_time_roll = 0;
    //    curent_time_bomb = t;
    //    rad_fire = 0;
    //    fire_time = 0;
    //    left_fire = true;
    //    right_fire = true;
    //    up_fire = true;
    //    down_fire = true;
    //    right_space = true;
    //    left_space = true;
    //    up_space = true;
    //    down_space = true;
    //    fired = false;
    //  }

    //  // контроль взрыва бомбы
    //  public void bang_control(GameTime gameTime, hero A)
    //  {
    //    this.curent_time_bomb += (float)gameTime.ElapsedGameTime.TotalSeconds;
    //    if (this.curent_time_bomb > 3)
    //      if (!this.banged)
    //        this.bang(this.bomb_pos_i, this.bomb_pos_j, A);
    //  }

    //  public void bang_fire(GameTime gameTime, hero A)
    //  {
    //    fire_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
    //    if (fire_time > 10)
    //    {
    //      if (rad_fire == 0)
    //      {
    //        if (A.player_pos_i == this.fire_pos_i && A.player_pos_j == this.fire_pos_j)
    //        {
    //          A.player_dead = true;
    //          A.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
    //        }
    //        A.M[this.fire_pos_i, this.fire_pos_j] = "*";

    //      }
    //      else
    //      {
    //        if (this.fire_pos_j + this.rad_fire < A.size2 - 1 && A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] != "0")
    //        {
    //          if (right_fire == true)
    //          {
    //            if (A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] == "P")
    //            {
    //              A.player_dead = true;
    //              A.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
    //            }
    //            // if (A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] == "B")
    //            {
    //              foreach (bomb X in A.bomb_list)
    //                if (X.bomb_pos_i == this.fire_pos_i && X.bomb_pos_j == this.fire_pos_j + this.rad_fire)
    //                  X.bang(this.fire_pos_i, this.fire_pos_j + this.rad_fire, A);
    //            }
    //            if (!A.is_empty/*_for_bomb*/(this.fire_pos_i, this.fire_pos_j + this.rad_fire))
    //              right_fire = false;
    //            A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] = "*";
    //          }
    //        }

    //        if (this.fire_pos_j - this.rad_fire > 0 && A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] != "0")
    //        {
    //          if (left_fire == true)
    //          {
    //            if (A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] == "P")
    //            {
    //              A.player_dead = true;
    //              A.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
    //            }
    //            // if (A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] == "B")
    //            {
    //              foreach (bomb X in A.bomb_list)
    //                if (X.bomb_pos_i == this.fire_pos_i && X.bomb_pos_j == this.fire_pos_j - this.rad_fire)
    //                  X.bang(this.fire_pos_i, this.fire_pos_j - this.rad_fire, A);
    //            }

    //            if (!A.is_empty/*_for_bomb*/(this.fire_pos_i, this.fire_pos_j - this.rad_fire))
    //              left_fire = false;
    //            A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] = "*";
    //          }
    //        }

    //        if (this.fire_pos_i - this.rad_fire > 0 && A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] != "0")
    //        {
    //          if (up_fire == true)
    //          {
    //            if (A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] == "P")
    //            {
    //              A.player_dead = true;
    //              A.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
    //            }
    //            // if (A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] == "B")
    //            {
    //              foreach (bomb X in A.bomb_list)
    //                if (X.bomb_pos_i == this.fire_pos_i - this.rad_fire && X.bomb_pos_j == this.fire_pos_j)
    //                  X.bang(this.fire_pos_i - this.rad_fire, this.fire_pos_j, A);
    //            }
    //            if (!A.is_empty/*_for_bomb*/(this.fire_pos_i - this.rad_fire, this.fire_pos_j))
    //              up_fire = false;
    //            A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] = "*";
    //          }
    //        }
    //        if (this.fire_pos_i + this.rad_fire < A.size1 - 1 && A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] != "0")
    //        {
    //          if (down_fire == true)
    //          {
    //            if (A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] == "P")
    //            {
    //              A.player_dead = true;
    //              A.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
    //            }
    //            //if (A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] == "B")
    //            {
    //              foreach (bomb X in A.bomb_list)
    //                if (X.bomb_pos_i == this.fire_pos_i + this.rad_fire && X.bomb_pos_j == this.fire_pos_j)
    //                  X.bang(this.fire_pos_i + this.rad_fire, this.fire_pos_j, A);
    //            }
    //            if (!A.is_empty/*_for_bomb*/(this.fire_pos_i + this.rad_fire, this.fire_pos_j))
    //              down_fire = false;
    //            A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] = "*";
    //          }
    //        }

    //      }
    //    }
    //    if (fire_time > 60)
    //    {
    //      if (rad_fire == 0)
    //      {
    //        A.M[this.fire_pos_i, this.fire_pos_j] = " ";
    //      }
    //      else
    //      {
    //        if (this.fire_pos_j + this.rad_fire < A.size2 - 1 && A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] != "0")
    //        {
    //          if (right_space == true)
    //          {
    //            if (A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] == "P")
    //              A.player_dead = true;
    //            A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] = " ";
    //            if (right_fire == false)
    //              right_space = false;
    //          }
    //        }
    //        if (this.fire_pos_j - this.rad_fire > 0 && A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] != "0")
    //        {
    //          if (left_space == true)
    //          {
    //            if (A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] == "P")
    //              A.player_dead = true;
    //            A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] = " ";
    //            if (left_fire == false)
    //              left_space = false;
    //          }
    //        }
    //        if (this.fire_pos_i - this.rad_fire > 0 && A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] != "0")
    //        {
    //          if (up_space == true)
    //          {
    //            if (A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] == "P")
    //              A.player_dead = true;
    //            A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] = " ";
    //            if (up_fire == false)
    //              up_space = false;
    //          }
    //        }
    //        if (this.fire_pos_i + this.rad_fire < A.size1 - 1 && A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] != "0")
    //        {
    //          if (down_space == true)
    //          {
    //            if (A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] == "P")
    //              A.player_dead = true;
    //            A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] = " ";
    //            if (down_fire == false)
    //              down_space = false;
    //          }
    //        }
    //      }
    //      if (this.rad_fire < A.rad_bang)
    //      {
    //        this.fire_time = 0;
    //        this.rad_fire++;
    //      }
    //      else
    //      {
    //        this.fire = false;
    //        this.fired = true;
    //      }
    //    }
    //  }

    //  //взрыв бомбы
    //  public void bang(int I, int J, hero A)
    //  {

    //    if (this.banged == false)
    //      if (this.bomb_pos_i == I && this.bomb_pos_j == J)
    //        if (this.bomb_roll_i == this.bomb_pos_i || this.bomb_roll_i == -1)
    //          if (this.bomb_roll_j == this.bomb_pos_j || this.bomb_roll_j == -1)
    //          {
    //            A.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
    //            this.fire_pos_i = this.bomb_pos_i;
    //            this.fire_pos_j = this.bomb_pos_j;
    //            this.bomb_pos_i = -1;
    //            this.bomb_pos_j = -1;
    //            this.bomb_roll_i = -1;
    //            this.bomb_roll_j = -1;
    //            bomb_time = 0;
    //            bomb_time_roll = 0;
    //            curent_time_bomb = 0;
    //            this.banged = true;
    //            bomb_roll_napr = "stop";
    //            fire = true;
    //            A.bangSound.Play(0.1f, 0.0f, 0.0f);
    //          }

    //  }


    //  //момент качения бомбы
    //  public void rolling_2(GameTime gameTime, hero A)
    //  {
    //    if (this.bomb_pos_i == this.bomb_roll_i && this.bomb_pos_j == this.bomb_roll_j)
    //    {
    //      if (bomb_roll_napr == "down")
    //      {
    //        if (A.is_empty_for_bomb(this.bomb_pos_i + 1, this.bomb_pos_j))
    //        {
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
    //          this.bomb_roll_i++;
    //          this.bomb_pos_i = this.bomb_roll_i;
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
    //          bomb_time_roll = 0;
    //        }
    //        else
    //        {
    //          this.bomb_roll_i = -1;
    //          this.bomb_roll_j = -1;
    //          this.bomb_roll_napr = "stop";
    //        }
    //      }
    //      if (bomb_roll_napr == "up")
    //      {
    //        if (A.is_empty_for_bomb(this.bomb_pos_i - 1, this.bomb_pos_j))
    //        {
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
    //          this.bomb_roll_i--;
    //          this.bomb_pos_i = this.bomb_roll_i;
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
    //          bomb_time_roll = 0;
    //        }
    //        else
    //        {
    //          this.bomb_roll_i = -1;
    //          this.bomb_roll_j = -1;
    //          this.bomb_roll_napr = "stop";
    //        }
    //      }
    //      if (bomb_roll_napr == "right")
    //      {
    //        if (A.is_empty_for_bomb(this.bomb_pos_i, this.bomb_pos_j - 1))
    //        {
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
    //          this.bomb_roll_j--;
    //          this.bomb_pos_j = this.bomb_roll_j;
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
    //          bomb_time_roll = 0;
    //        }
    //        else
    //        {
    //          this.bomb_roll_i = -1;
    //          this.bomb_roll_j = -1;
    //          this.bomb_roll_napr = "stop";
    //        }
    //      }
    //      if (bomb_roll_napr == "left")
    //      {
    //        if (A.is_empty_for_bomb(this.bomb_pos_i, this.bomb_pos_j + 1))
    //        {
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
    //          this.bomb_roll_j++;
    //          this.bomb_pos_j = this.bomb_roll_j;
    //          A.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
    //          bomb_time_roll = 0;
    //        }
    //        else
    //        {
    //          this.bomb_roll_i = -1;
    //          this.bomb_roll_j = -1;
    //          this.bomb_roll_napr = "stop";
    //        }
    //      }
    //    }

    //  }

    //  // end of class
    //}
  }
}
