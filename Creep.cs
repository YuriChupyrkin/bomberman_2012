using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan
{
  class Creep
  {
    public int creep_pos_i;
    public int creep_pos_j;
    public int creep_new_pos_i;
    public int creep_new_pos_j;
    public bool creep_dead;
    public double creep_move_time;
    Vector2 creepPos;
    public string creep_napravlenie;
    public int creep_count_step;

    public int FrameCount; //Количество всех фреймов в изображении (у нас это 3) 
    public int frame;//какой фрейм нарисован в данный момент 
    public float TimeForFrame;//Сколько времени нужно показывать один фрейм (скорость) 
    public float TotalTime;//сколько времени прошло с показа предыдущего фрейма

    private environment _environment;
    private bots _bots;

    //конструктор
    public Creep(int I, int J, bots bots, environment environment)
    {
      this._environment = environment;
      this._bots = bots;

      frame = 0;
      TimeForFrame = (float)1 / 5;
      TotalTime = 0;
      FrameCount = 5;


      creep_pos_i = I;
      creep_pos_j = J;
      creep_new_pos_i = I;
      creep_new_pos_j = J;
      creep_dead = false;
      creep_move_time = 0;
      _environment.M[I, J] = "C";
      creep_napravlenie = "stop";
      creepPos.X = J * _environment.Y_cell;
      creepPos.Y = I * _environment.Y_cell;
      creep_count_step = 0;
    }

    // проверяем ботов
    public void check_dead_creep(GameTime gameTime, bots B)
    {
      for (int i = 0; i < _environment.size1; i++)
      {
        for (int j = 0; j < _environment.size2; j++)
        {
          if (i == this.creep_pos_i && j == this.creep_pos_j)
          {
            if (_environment.M[i, j] != "C")
            {
              if (_environment.M[i, j] == "P")
              {
                _environment.player_dead = true;
              }
              if (_environment.M[i, j] == "*")
              {
                this.creep_dead = true;
                _environment.creep_kill++;
                // TODO: Move from env
                _environment.bot_dead_sound.Play(0.1f, 0.0f, 0.0f);
              }
              else
              {
                this.creep_dead = true;
                //B.creep_kill++;
              }
            }
            if (i == this.creep_new_pos_i && j == this.creep_new_pos_j)
            {
              if (_environment.M[i, j] == "*")
              {
                this.creep_dead = true;
                _environment.creep_kill++;
                _environment.bot_dead_sound.Play(0.1f, 0.0f, 0.0f);
              }
              /*  if (B.M[i, j] == "P")
                {
                    B.player_dead = true;
                } */
            }
          }
        }
      }
    }

    //выбираем сторону, куда пойдет крип
    public void lets_go_creep(GameTime gameTime, bots B)
    {
      if (this.creep_napravlenie == "stop")
      {
        Random rdn = new Random();
        int napravl = 0; // направление (влево, вправо, вверх, вниз)
        napravl = rdn.Next(0, 9);
        if (napravl == 1 || napravl == 5)
        {
          if (_bots.is_empty_for_bots(this.creep_pos_i + 1, this.creep_pos_j))
          {
            this.creep_napravlenie = "down";
            if (_bots.is_empty_for_bots(this.creep_pos_i + 1, this.creep_pos_j))
            {
              if (_environment.M[this.creep_pos_i + 1, this.creep_pos_j] == "s")
              {
                _environment.creep_speed = 2;
              }
              _environment.M[this.creep_pos_i + 1, this.creep_pos_j] = "c";
              this.creep_new_pos_i = this.creep_pos_i + 1;
              this.creep_new_pos_j = this.creep_pos_j;
              this.bot_move(gameTime, B);
            }
          }
        }
        if (napravl == 2 || napravl == 6)
        {
          if (_bots.is_empty_for_bots(this.creep_pos_i - 1, this.creep_pos_j))
          {
            this.creep_napravlenie = "up";
            if (_bots.is_empty_for_bots(this.creep_pos_i - 1, this.creep_pos_j))
            {
              if (_environment.M[this.creep_pos_i - 1, this.creep_pos_j] == "s")
              {
                _environment.creep_speed = 2;
              }
              _environment.M[this.creep_pos_i - 1, this.creep_pos_j] = "c";
              this.creep_new_pos_i = this.creep_pos_i - 1;
              this.creep_new_pos_j = this.creep_pos_j;
              this.bot_move(gameTime, B);
            }
          }
        }
        if (napravl == 3 || napravl == 7)
        {
          if (_bots.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j + 1))
          {
            this.creep_napravlenie = "right";
            if (_bots.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j + 1))
            {
              if (_environment.M[this.creep_pos_i, this.creep_pos_j + 1] == "s")
              {
                _environment.creep_speed = 2;
              }

              _environment.M[this.creep_pos_i, this.creep_pos_j + 1] = "c";
              this.creep_new_pos_i = this.creep_pos_i;
              this.creep_new_pos_j = this.creep_pos_j + 1;
              this.bot_move(gameTime, B);
            }
          }
        }
        if (napravl == 4 || napravl == 8)
        {
          if (_bots.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j - 1))
          {
            this.creep_napravlenie = "left";
            if (_bots.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j - 1))
            {
              if (_environment.M[this.creep_pos_i, this.creep_pos_j - 1] == "s")
              {
                _environment.creep_speed = 2;
              }

              _environment.M[this.creep_pos_i, this.creep_pos_j - 1] = "c";
              this.creep_new_pos_i = this.creep_pos_i;
              this.creep_new_pos_j = this.creep_pos_j - 1;
              this.bot_move(gameTime, B);
            }
          }
        }
      }
    }

    //двигаемся по шагам
    public void bot_move(GameTime gameTime, bots B)
    {
      this.creep_moving(gameTime, B);
      this.creep_count_step++;

      if (this.creep_count_step * _environment.creep_speed >= _environment.X_cell)
      {
        this.creep_count_step = 0;
        if (this.creep_napravlenie == "right")
          this.creep_move_in_mas(0, 1, B);
        if (this.creep_napravlenie == "left")
          this.creep_move_in_mas(0, -1, B);
        if (this.creep_napravlenie == "down")
          this.creep_move_in_mas(1, 0, B);
        if (this.creep_napravlenie == "up")
          this.creep_move_in_mas(-1, 0, B);
        //this.creep_napravlenie = "stop";
      }

    }

    //в массиве
    public void creep_move_in_mas(int I, int J, bots B)
    {
      this.creep_pos_i += I;
      this.creep_pos_j += J;
      if (_environment.M[this.creep_pos_i, this.creep_pos_j] == "P")
      {
        _environment.player_dead = true;
      }
      _environment.M[this.creep_pos_i, this.creep_pos_j] = "C";
      _environment.M[this.creep_pos_i - I, this.creep_pos_j - J] = " ";
      this.creep_napravlenie = "stop";
      this.creepPos.X = creep_pos_j * _environment.Y_cell;
      this.creepPos.Y = creep_pos_i * _environment.Y_cell;
    }

    //анимированое передвижение
    public void creep_moving(GameTime gameTime, bots B)
    {
      if (this.creep_napravlenie == "right")
      {
        this.creepPos.X += _environment.creep_speed;
      }
      if (this.creep_napravlenie == "left")
      {
        this.creepPos.X -= _environment.creep_speed;
      }
      if (this.creep_napravlenie == "down")
      {
        this.creepPos.Y += _environment.creep_speed;
      }
      if (this.creep_napravlenie == "up")
      {
        this.creepPos.Y -= _environment.creep_speed;
      }
      TotalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (TotalTime > TimeForFrame)
      {
        frame++;
        frame = frame % (FrameCount - 1);
        TotalTime -= TimeForFrame;
      }
    }

    //рисуем
    public void DrawCreep(SpriteBatch spriteBatch, bots B)
    {
      int frameWidth = _environment.Hero_go_right_tex.Width / this.FrameCount;
      Rectangle creep_rec = new Rectangle(frameWidth * this.frame, 0, frameWidth, _environment.Hero_go_right_tex.Height);

      spriteBatch.Begin();

      if (creep_napravlenie == "stop")
      {
        spriteBatch.Draw(B.creepTex, this.creepPos, Color.White);
      }

      if (creep_napravlenie == "left")
      {
        spriteBatch.Draw(B.creep_go_left_tex, this.creepPos, creep_rec, Color.White);
      }

      if (creep_napravlenie == "right")
      {
        spriteBatch.Draw(B.creep_go_right_tex, this.creepPos, creep_rec, Color.White);
      }

      if (creep_napravlenie == "down")
      {
        spriteBatch.Draw(B.creep_go_down_tex, this.creepPos, creep_rec, Color.White);
      }

      if (creep_napravlenie == "up")
      {
        spriteBatch.Draw(B.creep_go_up_tex, this.creepPos, creep_rec, Color.White);
      }
      spriteBatch.End();
    }
  }
}

