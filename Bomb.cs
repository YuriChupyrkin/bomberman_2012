using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan
{
  class Bomb
  {
    public int bomb_pos_i;
    public int bomb_pos_j;
    public double bomb_time;
    public bool banged;
    public string bomb_roll_napr;
    public int bomb_roll_i;
    public int bomb_roll_j;
    public double bomb_time_roll;
    public double curent_time_bomb;
    public bool fire;
    public int rad_fire;
    public double fire_time;
    public int fire_pos_i;
    public int fire_pos_j;
    public bool left_fire;
    public bool right_fire;
    public bool up_fire;
    public bool down_fire;
    public bool right_space;
    public bool left_space;
    public bool up_space;
    public bool down_space;
    public bool fired;

    private environment _environment;

    public Bomb(environment environment, int i, int j, double t)
    {
      this._environment = environment;

      bomb_pos_i = i;
      bomb_pos_j = j;
      bomb_time = t;
      banged = false;
      fire = false;
      bomb_roll_napr = "stop";
      bomb_roll_i = -1;
      bomb_roll_j = -1;
      bomb_time_roll = 0;
      curent_time_bomb = t;
      rad_fire = 0;
      fire_time = 0;
      left_fire = true;
      right_fire = true;
      up_fire = true;
      down_fire = true;
      right_space = true;
      left_space = true;
      up_space = true;
      down_space = true;
      fired = false;
    }

    // контроль взрыва бомбы
    public void bang_control(GameTime gameTime, hero A)
    {
      this.curent_time_bomb += (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (this.curent_time_bomb > 3)
      {
        if (!this.banged)
        {
          this.bang(this.bomb_pos_i, this.bomb_pos_j);
        }
      }
    }

    public void bang_fire(GameTime gameTime, hero hero)
    {
      fire_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
      if (fire_time > 10)
      {
        if (rad_fire == 0)
        {
          if (_environment.player_pos_i == this.fire_pos_i && _environment.player_pos_j == this.fire_pos_j)
          {
            _environment.player_dead = true;
            _environment.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
          }
          _environment.M[this.fire_pos_i, this.fire_pos_j] = "*";

        }
        else
        {
          if (this.fire_pos_j + this.rad_fire < _environment.size2 - 1 && _environment.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] != "0")
          {
            if (right_fire == true)
            {
              if (_environment.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] == "P")
              {
                _environment.player_dead = true;
                _environment.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
              }
              // if (A.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] == "B")
              {
                foreach (Bomb X in hero.bomb_list)
                {
                  if (X.bomb_pos_i == this.fire_pos_i && X.bomb_pos_j == this.fire_pos_j + this.rad_fire)
                  {
                    X.bang(this.fire_pos_i, this.fire_pos_j + this.rad_fire);
                  }
                }
              }
              if (!hero.is_empty/*_for_bomb*/(this.fire_pos_i, this.fire_pos_j + this.rad_fire))
                right_fire = false;
              _environment.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] = "*";
            }
          }

          if (this.fire_pos_j - this.rad_fire > 0 && _environment.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] != "0")
          {
            if (left_fire == true)
            {
              if (_environment.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] == "P")
              {
                _environment.player_dead = true;
                _environment.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
              }
              // if (A.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] == "B")
              {
                foreach (Bomb X in hero.bomb_list)
                {
                  if (X.bomb_pos_i == this.fire_pos_i && X.bomb_pos_j == this.fire_pos_j - this.rad_fire)
                  {
                    X.bang(this.fire_pos_i, this.fire_pos_j - this.rad_fire);
                  }
                }
              }

              if (!hero.is_empty/*_for_bomb*/(this.fire_pos_i, this.fire_pos_j - this.rad_fire))
                left_fire = false;
              _environment.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] = "*";
            }
          }

          if (this.fire_pos_i - this.rad_fire > 0 && _environment.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] != "0")
          {
            if (up_fire == true)
            {
              if (_environment.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] == "P")
              {
                _environment.player_dead = true;
                _environment.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
              }
              // if (A.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] == "B")
              {
                foreach (Bomb X in hero.bomb_list)
                {
                  if (X.bomb_pos_i == this.fire_pos_i - this.rad_fire && X.bomb_pos_j == this.fire_pos_j)
                  {
                    X.bang(this.fire_pos_i - this.rad_fire, this.fire_pos_j);
                  }
                }
              }
              if (!hero.is_empty/*_for_bomb*/(this.fire_pos_i - this.rad_fire, this.fire_pos_j))
                up_fire = false;
              _environment.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] = "*";
            }
          }
          if (this.fire_pos_i + this.rad_fire < _environment.size1 - 1 && _environment.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] != "0")
          {
            if (down_fire == true)
            {
              if (_environment.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] == "P")
              {
                _environment.player_dead = true;
                _environment.player_dead_mus.Play(0.1f, 0.0f, 0.0f);
              }
              //if (A.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] == "B")
              {
                foreach (Bomb X in hero.bomb_list)
                {
                  if (X.bomb_pos_i == this.fire_pos_i + this.rad_fire && X.bomb_pos_j == this.fire_pos_j)
                  {
                    X.bang(this.fire_pos_i + this.rad_fire, this.fire_pos_j);
                  }
                }
              }
              if (!hero.is_empty/*_for_bomb*/(this.fire_pos_i + this.rad_fire, this.fire_pos_j))
                down_fire = false;
              _environment.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] = "*";
            }
          }

        }
      }
      if (fire_time > 60)
      {
        if (rad_fire == 0)
        {
          _environment.M[this.fire_pos_i, this.fire_pos_j] = " ";
        }
        else
        {
          if (this.fire_pos_j + this.rad_fire < _environment.size2 - 1 && _environment.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] != "0")
          {
            if (right_space == true)
            {
              if (_environment.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] == "P")
                _environment.player_dead = true;
              _environment.M[this.fire_pos_i, this.fire_pos_j + this.rad_fire] = " ";
              if (right_fire == false)
                right_space = false;
            }
          }
          if (this.fire_pos_j - this.rad_fire > 0 && _environment.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] != "0")
          {
            if (left_space == true)
            {
              if (_environment.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] == "P")
                _environment.player_dead = true;
              _environment.M[this.fire_pos_i, this.fire_pos_j - this.rad_fire] = " ";
              if (left_fire == false)
                left_space = false;
            }
          }
          if (this.fire_pos_i - this.rad_fire > 0 && _environment.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] != "0")
          {
            if (up_space == true)
            {
              if (_environment.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] == "P")
                _environment.player_dead = true;
              _environment.M[this.fire_pos_i - this.rad_fire, this.fire_pos_j] = " ";
              if (up_fire == false)
                up_space = false;
            }
          }
          if (this.fire_pos_i + this.rad_fire < _environment.size1 - 1 && _environment.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] != "0")
          {
            if (down_space == true)
            {
              if (_environment.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] == "P")
                _environment.player_dead = true;
              _environment.M[this.fire_pos_i + this.rad_fire, this.fire_pos_j] = " ";
              if (down_fire == false)
                down_space = false;
            }
          }
        }
        if (this.rad_fire < _environment.rad_bang)
        {
          this.fire_time = 0;
          this.rad_fire++;
        }
        else
        {
          this.fire = false;
          this.fired = true;
        }
      }
    }

    //взрыв бомбы
    public void bang(int I, int J)
    {

      if (this.banged == false)
      {
        if (this.bomb_pos_i == I && this.bomb_pos_j == J)
        {
          if (this.bomb_roll_i == this.bomb_pos_i || this.bomb_roll_i == -1)
          {
            if (this.bomb_roll_j == this.bomb_pos_j || this.bomb_roll_j == -1)
            {
              _environment.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
              this.fire_pos_i = this.bomb_pos_i;
              this.fire_pos_j = this.bomb_pos_j;
              this.bomb_pos_i = -1;
              this.bomb_pos_j = -1;
              this.bomb_roll_i = -1;
              this.bomb_roll_j = -1;
              bomb_time = 0;
              bomb_time_roll = 0;
              curent_time_bomb = 0;
              this.banged = true;
              bomb_roll_napr = "stop";
              fire = true;
              _environment.bangSound.Play(0.1f, 0.0f, 0.0f);
            }
          }
        }
      }
    }


    //момент качения бомбы
    public void rolling_2(GameTime gameTime)
    {
      if (this.bomb_pos_i == this.bomb_roll_i && this.bomb_pos_j == this.bomb_roll_j)
      {
        if (bomb_roll_napr == "down")
        {
          if (is_empty_for_bomb(this.bomb_pos_i + 1, this.bomb_pos_j))
          {
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
            this.bomb_roll_i++;
            this.bomb_pos_i = this.bomb_roll_i;
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
            bomb_time_roll = 0;
          }
          else
          {
            this.bomb_roll_i = -1;
            this.bomb_roll_j = -1;
            this.bomb_roll_napr = "stop";
          }
        }
        if (bomb_roll_napr == "up")
        {
          if (is_empty_for_bomb(this.bomb_pos_i - 1, this.bomb_pos_j))
          {
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
            this.bomb_roll_i--;
            this.bomb_pos_i = this.bomb_roll_i;
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
            bomb_time_roll = 0;
          }
          else
          {
            this.bomb_roll_i = -1;
            this.bomb_roll_j = -1;
            this.bomb_roll_napr = "stop";
          }
        }
        if (bomb_roll_napr == "right")
        {
          if (is_empty_for_bomb(this.bomb_pos_i, this.bomb_pos_j - 1))
          {
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
            this.bomb_roll_j--;
            this.bomb_pos_j = this.bomb_roll_j;
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
            bomb_time_roll = 0;
          }
          else
          {
            this.bomb_roll_i = -1;
            this.bomb_roll_j = -1;
            this.bomb_roll_napr = "stop";
          }
        }
        if (bomb_roll_napr == "left")
        {
          if (is_empty_for_bomb(this.bomb_pos_i, this.bomb_pos_j + 1))
          {
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = " ";
            this.bomb_roll_j++;
            this.bomb_pos_j = this.bomb_roll_j;
            _environment.M[this.bomb_pos_i, this.bomb_pos_j] = "B";
            bomb_time_roll = 0;
          }
          else
          {
            this.bomb_roll_i = -1;
            this.bomb_roll_j = -1;
            this.bomb_roll_napr = "stop";
          }
        }
      }

    }

    // проверка постая ли ячейка, чтобы туда перейти
    private bool is_empty_for_bomb(int I, int J)
    {
      bool empty = false;
      if (I > 0 && I < _environment.size1)
        if (J > 0 && J < _environment.size2)
          if (_environment.M[I, J] == " " || _environment.M[I, J] == "*" || _environment.M[I, J] == "A" || _environment.M[I, J] == "T" || _environment.M[I, J] == "R" || _environment.M[I, J] == "S" || _environment.M[I, J] == "s")
          {
            empty = true;
          }
      if (_environment.M[I, J] == "B")
      {
        if (_environment.bonus_tolk == true)
        {
          //start_tolk_bomb(I, J);
        }
      }

      return empty;
    }
  }
}
