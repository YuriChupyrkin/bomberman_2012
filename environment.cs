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
    class environment
    {
        public string[,] M;
        public int size1;
        public int size2;
        public int X_cell;
        public int Y_cell;
        public int player_pos_i;
        public int player_pos_j;
        public int max_num_bomb;
        public int curent_num_bomb;
        public bool bonus_tolk;
        public int rad_bang;
        public bool player_dead;
        public int player_speed;
        public GameTime GlobalGT;
        public double globalGameTime;
        public Texture2D wallTex;
        public Vector2 wallPos;
        public Texture2D brickTex;
        public Vector2 brickPos;
        public Texture2D fireTex;
        public Vector2 firePos;
        public SoundEffect bangSound;
        public SoundEffect bot_dead_sound;
        public int FrameCount; //Количество всех фреймов в изображении (у нас это 3) 
        public int frame;//какой фрейм нарисован в данный момент 
        public float TimeForFrame;//Сколько времени нужно показывать один фрейм (скорость) 
        public float TotalTime;//сколько времени прошло с показа предыдущего фрейма
        public Rectangle hero_rec;
        public Rectangle hero_rec2;
        public Texture2D Hero_texture;
        public Texture2D Hero_go_left_tex;
        public Texture2D Hero_go_right_tex;
        public Texture2D Hero_go_down_tex;
        public Texture2D Hero_go_up_tex;
        public Texture2D bombTex;
        public Vector2 bombPos;
        public Vector2 Hero_pos;
        public Texture2D tmpTex;
        public Vector2 tmpPos;
        public Texture2D bon_tolk_Tex;
        public Vector2 bon_tolk_Pos;
        public Texture2D bon_rad_Tex;
        public Vector2 bon_rad_Pos;
        public Texture2D bon_bomb_Tex;
        public Vector2 bon_bomb_Pos;
        public Texture2D bon_speed_Tex;
        public Vector2 bon_speed_Pos;
        public Texture2D bon_speed_bot_Tex;
        public Vector2 bon_speed_bot_Pos;
        public int bonus_tolk_time;
        public int bonus_rad_bang_time;
        public int bonus_bomb_time;
        public int bonus_speed_time;
        public int bonus_speed_bot_time;
        public double int_game_time;
        public int creep_speed;
        public int creep_kill;
        public int curent_lvl;
        public bool lvl_up;
        public bool lvl_start;
        public double lvl_start_time;
        public bool key_lock;
        public SpriteFont LevelFont;
        public Vector2 LevelFontPos;
        public double bot_add_time;
        public bool need_add_bot;
        public bool WIN;
        public SoundEffect bonus_up_mus;
        public SoundEffect plant_bomb_mus;
        public SoundEffect player_dead_mus;

        public environment(int sizeX = 16, int sizeY = 25, int cellWidth = 40, int cellHeight = 40)
        {
            frame = 0;
            TimeForFrame = (float)1 / 5;
            TotalTime = 0;
            FrameCount = 5; // <---- tut kol kadrov

            size1 = sizeX;
            size2 = sizeY;
            X_cell = cellWidth;
            Y_cell = cellHeight;
            max_num_bomb = 10;
            curent_num_bomb = 0;
            bonus_tolk = true;
            rad_bang = 15;
            player_dead = false;
            player_speed = 2;
            globalGameTime = 0;
            int_game_time = 0;

            creep_speed = 1;
            creep_kill = 0;

            curent_lvl = 1;
            lvl_up = false;
            lvl_start = true;
            lvl_start_time = 0;
            key_lock = true;
            bot_add_time = 10000;
            need_add_bot = false;
            WIN = false;



            M = new string[size1, size2];
            for (int i = 0; i < size1; i++)
                for (int j = 0; j < size2; j++)
                    M[i, j] = " ";
        }

        //время бонусов
        public void set_bonus_time()
        {
            if (curent_lvl == 1)
            {
                bonus_tolk_time = 1500;
                bonus_rad_bang_time = 45;
                bonus_bomb_time = 80;
                bonus_speed_bot_time = 1500;
                bonus_speed_time = 50;
            }
            if (curent_lvl == 2)
            {
                bonus_tolk_time = 1500;
                bonus_rad_bang_time = 45;
                bonus_bomb_time = 75;
                bonus_speed_bot_time = 1500;
                bonus_speed_time = 60;
            }

            if (curent_lvl == 3)
            {
                bonus_tolk_time = 80;
                bonus_rad_bang_time = 50;
                bonus_bomb_time = 70;
                bonus_speed_bot_time = 80;
                bonus_speed_time = 60;
            }

            if (curent_lvl == 4)
            {
                bonus_tolk_time = 80;
                bonus_rad_bang_time = 55;
                bonus_bomb_time = 65;
                bonus_speed_bot_time = 70;
                bonus_speed_time = 60;
                bot_add_time = 60;
            }

            if (curent_lvl == 5)
            {
                bonus_tolk_time = 80;
                bonus_rad_bang_time = 60;
                bonus_bomb_time = 60;
                bonus_speed_bot_time = 55;
                bonus_speed_time = 70;
                bot_add_time = 55;
            }

            if (curent_lvl == 6)
            {
                bonus_tolk_time = 80;
                bonus_rad_bang_time = 60;
                bonus_bomb_time = 60;
                bonus_speed_bot_time = 10000;
                bonus_speed_time = 80;
                bot_add_time = 35;
            }

            if (curent_lvl == 7)
            {
                bonus_tolk_time = 100;
                bonus_rad_bang_time = 100;
                bonus_bomb_time = 100;
                bonus_speed_bot_time = 10000;
                bonus_speed_time = 100;
                bot_add_time = 25;
            }
        }

        // создаем игровое поле
        public void create_world()
        {
            set_bonus_time();
            int pos;
            Random rdn = new Random();
            for (int i = 1; i < size1; i++)
                for (int j = 1; j < size2; j++)
                {
                    pos = rdn.Next(1, 100);
                    if (pos % rdn.Next(1, 50) == 0 || pos % rdn.Next(1, 50) == 1)
                        M[i, j] = "W";
                    else
                        M[i, j] = " ";
                }

            for (int j = 1; j < size2 - 1; j++)
            {
                M[0, j] = "0";
                M[size1 - 1, j] = "0";
            }

            for (int i = 1; i < size1 - 1; i++)
            {
                M[i, 0] = "0";
                M[i, size2 - 1] = "0";
            }
            M[0, 0] = "0";
            M[size1 - 1, 0] = "0";
            M[0, size2 - 1] = "0";
            M[size1 - 1, size2 - 1] = "0";

            M[1, 1] = "P";
            player_pos_i = 1;
            player_pos_j = 1;
            M[1, 2] = " ";
            M[1, 3] = " ";
            M[2, 1] = " ";
            M[3, 1] = " ";
            M[2, 2] = " ";
            M[3, 3] = " ";

        }

        //передвижение в массиве
        public void move_in_mas(int I, int J)
        {
            if (M[player_pos_i, player_pos_j] == "C")
                player_dead = true;
            player_pos_i += I;
            player_pos_j += J;
            if (M[player_pos_i, player_pos_j] == "T")
            {
                bonus_tolk = true;
                bonus_up_mus.Play(0.1f, 0.0f, 0.0f);
            }

            if (M[player_pos_i, player_pos_j] == "R")
            {
                rad_bang++;
                bonus_up_mus.Play(0.1f, 0.0f, 0.0f);
            }

            if (M[player_pos_i, player_pos_j] == "A")
            {
                max_num_bomb++;
                bonus_up_mus.Play(0.1f, 0.0f, 0.0f);
            }

            if (M[player_pos_i, player_pos_j] == "S")
            {
                player_speed = 2;
                bonus_up_mus.Play(0.1f, 0.0f, 0.0f);
            }

            M[player_pos_i, player_pos_j] = "P";

            if (M[player_pos_i - I, player_pos_j - J] != "B")
                M[player_pos_i - I, player_pos_j - J] = " ";
        }


        // игровое время, выдающее бонусы
        public void game_control_time(GameTime gameTime)
        {
            int_game_time += gameTime.ElapsedGameTime.TotalSeconds;
            if (((int)int_game_time + 1) % bonus_tolk_time == 0)
                give_bonus("T");
            if (((int)int_game_time + 1) % bonus_rad_bang_time == 0)
                give_bonus("R");
            if (((int)int_game_time + 1) % bonus_bomb_time == 0)
                give_bonus("A");
            if (((int)int_game_time + 1) % bonus_speed_time == 0)
                give_bonus("S");
            if (((int)int_game_time + 1) % bonus_speed_bot_time == 0)
                give_bonus("s");
            if (((int)int_game_time + 1) % bot_add_time == 0)
            {
                need_add_bot = true;
                bot_add_time--;
            }

            if (lvl_start == true)
            {
                lvl_start_time += gameTime.ElapsedGameTime.TotalSeconds;
                if (((int)lvl_start_time + 1) % 3 == 0)
                {
                    lvl_start_time = 0;
                    M[1, 1] = "P";
                    player_pos_i = 1;
                    player_pos_j = 1;
                    Hero_pos = new Vector2(40, 40);
                    M[1, 0] = "0";
                    M[0, 1] = "0";
                    key_lock = false;
                    lvl_start = false;
                }
            }
        }

        // выдаем бонусы
        void give_bonus(string str_bonus)
        {
            Random rdn = new Random();
            int count = 0;
            for (int i = 0; i < size1; i++)
                for (int j = 0; j < size2; j++)
                    if (M[i, j] == str_bonus)
                        count++;

            if (count == 0)
            {
                int bon_up = 0;
                while (bon_up == 0)
                {
                    int I = rdn.Next(1, size1 - 2);
                    int J = rdn.Next(1, size2 - 2);
                    if (M[I, J] == " ")
                    {
                        M[I, J] = str_bonus;
                        bon_up++;
                    }
                }
            }

        }


   
        //end file
    }
}
