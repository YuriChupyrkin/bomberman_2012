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
    class bots: environment
    {

        public Texture2D creepTex;
        public Texture2D creep_go_up_tex;
        public Texture2D creep_go_down_tex;
        public Texture2D creep_go_left_tex;
        public Texture2D creep_go_right_tex;
        public List<creep> creep_list;


                public class creep
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


                   //конструктор
                    public creep(int I, int J, bots B)
                    {
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
                        B.M[I, J] = "C";
                        creep_napravlenie = "stop";
                        creepPos.X = J * B.Y_cell;
                        creepPos.Y = I * B.Y_cell;
                        creep_count_step = 0;
                    }

                    // проверяем ботов
                    public void check_dead_creep(GameTime gameTime, bots B)
                    {
                        for (int i = 0; i < B.size1; i++)
                            for (int j = 0; j < B.size2; j++)
                            {
                                if (i == this.creep_pos_i && j == this.creep_pos_j)
                                    if (B.M[i, j] != "C")
                                    {
                                        if (B.M[i, j] == "P")
                                        {
                                            B.player_dead = true;
                                        }
                                        if (B.M[i, j] == "*")
                                        {
                                            this.creep_dead = true;
                                            B.creep_kill++;
                                            B.bot_dead_sound.Play(0.1f, 0.0f, 0.0f);
                                        }
                                        else
                                        {
                                            this.creep_dead = true;
                                            //B.creep_kill++;
                                        }
                                    }
                                if (i == this.creep_new_pos_i && j == this.creep_new_pos_j)
                                {
                                    if (B.M[i, j] == "*")
                                    {
                                        this.creep_dead = true;
                                        B.creep_kill++;
                                        B.bot_dead_sound.Play(0.1f, 0.0f, 0.0f);
                                    }
                                  /*  if (B.M[i, j] == "P")
                                    {
                                        B.player_dead = true;
                                    } */
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
                                if (B.is_empty_for_bots(this.creep_pos_i + 1, this.creep_pos_j))
                                {
                                    this.creep_napravlenie = "down";
                                    if (B.is_empty_for_bots(this.creep_pos_i + 1, this.creep_pos_j))
                                    {
                                        if (B.M[this.creep_pos_i + 1, this.creep_pos_j] == "s")
                                            B.creep_speed = 2;
                                        B.M[this.creep_pos_i + 1, this.creep_pos_j] = "c";
                                        this.creep_new_pos_i = this.creep_pos_i + 1;
                                        this.creep_new_pos_j = this.creep_pos_j;
                                        this.bot_move(gameTime, B);
                                    }
                                }
                            if (napravl == 2 || napravl == 6)
                                if (B.is_empty_for_bots(this.creep_pos_i - 1, this.creep_pos_j))
                                {
                                    this.creep_napravlenie = "up";
                                    if (B.is_empty_for_bots(this.creep_pos_i - 1, this.creep_pos_j))
                                    {
                                        if (B.M[this.creep_pos_i - 1, this.creep_pos_j] == "s")
                                            B.creep_speed = 2;
                                        B.M[this.creep_pos_i - 1, this.creep_pos_j] = "c";
                                        this.creep_new_pos_i = this.creep_pos_i - 1;
                                        this.creep_new_pos_j = this.creep_pos_j;
                                        this.bot_move(gameTime, B);
                                    }
                                }
                            if (napravl == 3 || napravl == 7)
                                if (B.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j + 1))
                                {
                                    this.creep_napravlenie = "right";
                                    if (B.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j + 1))
                                    {
                                        if (B.M[this.creep_pos_i, this.creep_pos_j + 1] == "s")
                                            B.creep_speed = 2;
                                        B.M[this.creep_pos_i, this.creep_pos_j + 1] = "c";
                                        this.creep_new_pos_i = this.creep_pos_i;
                                        this.creep_new_pos_j = this.creep_pos_j + 1;
                                        this.bot_move(gameTime, B);
                                    }
                                }
                            if (napravl == 4 || napravl == 8)
                                if (B.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j - 1))
                                {
                                    this.creep_napravlenie = "left";
                                    if (B.is_empty_for_bots(this.creep_pos_i, this.creep_pos_j - 1))
                                    {
                                        if (B.M[this.creep_pos_i, this.creep_pos_j - 1] == "s")
                                            B.creep_speed = 2;
                                        B.M[this.creep_pos_i, this.creep_pos_j - 1] = "c";
                                        this.creep_new_pos_i = this.creep_pos_i;
                                        this.creep_new_pos_j = this.creep_pos_j - 1;
                                        this.bot_move(gameTime, B);
                                    }
                                }
                        }                   
                        
                    }

                    //двигаемся по шагам
                    public void bot_move(GameTime gameTime, bots B)
                    {
                        this.creep_moving(gameTime, B);
                        this.creep_count_step++;

                        if (this.creep_count_step * B.creep_speed  >= B.X_cell)
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
                        if (B.M[this.creep_pos_i, this.creep_pos_j] == "P")
                            B.player_dead = true;
                        B.M[this.creep_pos_i, this.creep_pos_j] = "C";
                        B.M[this.creep_pos_i - I, this.creep_pos_j - J] = " ";
                        this.creep_napravlenie = "stop";
                        this.creepPos.X = creep_pos_j * B.Y_cell;
                        this.creepPos.Y = creep_pos_i * B.Y_cell;
                    }

                    //анимированое передвижение
                    public void creep_moving(GameTime gameTime, bots B)
                    {
                        if (this.creep_napravlenie == "right")
                        {
                            this.creepPos.X += B.creep_speed;
                        }
                        if (this.creep_napravlenie == "left")
                        {
                            this.creepPos.X -= B.creep_speed;
                        }
                        if (this.creep_napravlenie == "down")
                        {
                            this.creepPos.Y +=  B.creep_speed;
                        }
                        if (this.creep_napravlenie == "up")
                        {
                            this.creepPos.Y -=  B.creep_speed;
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
                        int frameWidth = B.Hero_go_right_tex.Width / this.FrameCount;
                        Rectangle creep_rec = new Rectangle(frameWidth * this.frame, 0, frameWidth, B.Hero_go_right_tex.Height);

                        spriteBatch.Begin();

                        if(creep_napravlenie  == "stop")
                            spriteBatch.Draw(B.creepTex, this.creepPos, Color.White);

                        if(creep_napravlenie == "left")
                            spriteBatch.Draw(B.creep_go_left_tex, this.creepPos, creep_rec, Color.White);

                        if (creep_napravlenie == "right")
                            spriteBatch.Draw(B.creep_go_right_tex, this.creepPos, creep_rec, Color.White);

                        if (creep_napravlenie == "down")
                            spriteBatch.Draw(B.creep_go_down_tex, this.creepPos, creep_rec, Color.White);

                        if (creep_napravlenie == "up")
                            spriteBatch.Draw(B.creep_go_up_tex, this.creepPos, creep_rec, Color.White);
                        spriteBatch.End();
                    }
                }

        //конструктор
        public bots()
        {
            creep_list = new List<creep>();
        }

        //создаем в начале 3 бота
        public void create_bots()
        {
            if (curent_lvl == 1)
            {
                creep_list.Add(new creep(1, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, 1, this));
                creep_list.Add(new creep(size1 - 2, size2 - 2, this));
            }

            if (curent_lvl == 2)
            {
                creep_list.Add(new creep(1, size2 - 2, this));
                creep_list.Add(new creep(2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, 1, this));
                creep_list.Add(new creep(size1 - 3, 1, this));
                creep_list.Add(new creep(size1 - 2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 3, size2 - 2, this));
            }

            if (curent_lvl == 3)
            {
                creep_list.Add(new creep(1, size2 - 2, this));
                creep_list.Add(new creep(1, size2 - 3, this));
                creep_list.Add(new creep(2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, 1, this));
                creep_list.Add(new creep(size1 - 2, 2, this));
                creep_list.Add(new creep(size1 - 3, 1, this));
                creep_list.Add(new creep(size1 - 2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, size2 - 3, this));
                creep_list.Add(new creep(size1 - 3, size2 - 2, this));
            }

            if (curent_lvl == 4)
            {
                creep_list.Add(new creep(1, size2 - 2, this));
                creep_list.Add(new creep(1, size2 - 3, this));
                creep_list.Add(new creep(2, size2 - 2, this));
                creep_list.Add(new creep(3, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, 1, this));
                creep_list.Add(new creep(size1 - 2, 2, this));
                creep_list.Add(new creep(size1 - 3, 1, this));
                creep_list.Add(new creep(size1 - 4, 1, this));
                creep_list.Add(new creep(size1 - 2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, size2 - 3, this));
                creep_list.Add(new creep(size1 - 3, size2 - 2, this));
                creep_list.Add(new creep(size1 - 4, size2 - 2, this));
            }

            if (curent_lvl == 5)
            {
                creep_list.Add(new creep(1, size2 - 2, this));
                creep_list.Add(new creep(1, size2 - 3, this));
                creep_list.Add(new creep(2, size2 - 2, this));
                creep_list.Add(new creep(3, size2 - 2, this));
                creep_list.Add(new creep(1, size2 - 4, this));
                creep_list.Add(new creep(size1 - 2, 1, this));
                creep_list.Add(new creep(size1 - 2, 2, this));
                creep_list.Add(new creep(size1 - 3, 1, this));
                creep_list.Add(new creep(size1 - 4, 1, this));
                creep_list.Add(new creep(size1 - 2, 3, this));
                creep_list.Add(new creep(size1 - 2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, size2 - 3, this));
                creep_list.Add(new creep(size1 - 3, size2 - 2, this));
                creep_list.Add(new creep(size1 - 4, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, size2 - 4, this));
            }

            if (curent_lvl == 6)
            {
                creep_list.Add(new creep(1, size2 - 2, this));
                creep_list.Add(new creep(1, size2 - 3, this));
                creep_list.Add(new creep(2, size2 - 2, this));
                creep_list.Add(new creep(3, size2 - 2, this));
                creep_list.Add(new creep(1, size2 - 4, this));
                creep_list.Add(new creep(size1 - 2, 1, this));
                creep_list.Add(new creep(size1 - 2, 2, this));
                creep_list.Add(new creep(size1 - 3, 1, this));
                creep_list.Add(new creep(size1 - 4, 1, this));
                creep_list.Add(new creep(size1 - 2, 3, this));
                creep_list.Add(new creep(size1 - 2, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, size2 - 3, this));
                creep_list.Add(new creep(size1 - 3, size2 - 2, this));
                creep_list.Add(new creep(size1 - 4, size2 - 2, this));
                creep_list.Add(new creep(size1 - 2, size2 - 4, this));
            }

            if (curent_lvl == 7)
            {
                int I = size1 - 2;
                for(int j = 1; j < size2 - 1; j++)
                    creep_list.Add(new creep(I, j, this));
                int J = size2 - 2;
                for(int i = 1; i < size1 - 2; i++)
                    creep_list.Add(new creep(i, J, this));
            }
        }

        //метод LOADBots
        public void LoadBots(ContentManager Content)
        {
            creepTex = Content.Load<Texture2D>(@"Textures/creep");
            creep_go_down_tex = Content.Load<Texture2D>(@"Textures/go_down_creep");
            creep_go_up_tex = Content.Load<Texture2D>(@"Textures/go_up_creep");
            creep_go_left_tex = Content.Load<Texture2D>(@"Textures/go_left_creep");
            creep_go_right_tex = Content.Load<Texture2D>(@"Textures/go_right_creep");
        }

        // проверка постая ли ячейка, чтобы туда перейти
        protected bool is_empty_for_bots(int I, int J)
        {
            bool empty = false;
            if (I > 0 && I < size1)
                if (J > 0 && J < size2)
                    if (M[I, J] == " " || M[I, J] == "*" || M[I, J] == "P" || M[I, J] == "A" || M[I, J] == "T" || M[I, J] == "R" || M[I, J] == "S" || M[I, J] == "s")
                        empty = true;
            return empty;
        }

        //метод UpdateBots
        public void UpdateBots(GameTime gameTime)
        {
            foreach (creep X in creep_list)
            {
                if (X.creep_dead == false)
                {
                    X.check_dead_creep(gameTime, this);
                    if (globalGameTime > 1)
                        X.lets_go_creep(gameTime, this);
                    if (X.creep_napravlenie == "up" || X.creep_napravlenie == "down" || X.creep_napravlenie == "right" || X.creep_napravlenie == "left")
                        X.bot_move(gameTime, this);
                    if (player_pos_i == X.creep_new_pos_i && player_pos_j == X.creep_new_pos_j)
                    {
                        player_dead = true;
                    }
                }
                if (X.creep_dead == true)
                {
                    if (X.creep_new_pos_i != -1 && X.creep_new_pos_j != -1)
                    {
                        this.M[X.creep_new_pos_i, X.creep_new_pos_j] = " ";
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
                if (curent_lvl < 7)
                    lvl_up = true;
                else
                    WIN = true;
            }
            

            if (need_add_bot)
            {
                need_add_bot = false;
                bot_add();
            }
               
        }

        //все крипы мертвы
        public bool all_creep_dead()
        {
            foreach (creep X in creep_list)
                if (X.creep_dead == false)
                    return false;
            return true;
        }


        //method DrawBots
        public void DrawBots(SpriteBatch spriteBatch)
        {

            foreach (creep X in creep_list)
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
                int I = rdn.Next(1, size1 - 2);
                int J = rdn.Next(1, size2 - 2);
                if (M[I, J] == " " && correct_pos(I, J) == true)
                {
                    M[I, J] = "C";
                    creep_list.Add(new creep(I, J, this));
                    added++;
                }
            }
        }

        // чтобы не появлялся возле игрока
        bool correct_pos(int I, int J)
        {
            bool player = false;
            if ((J - 2) > 1 && (J + 2) < size2)
            {
                if ((I - 2) > 1 && (I + 2) < size1)
                {
                    for (int i = (I - 2); i <= (I + 2); i++)
                        for (int j = (J - 2); j <= (J + 2); j++)
                            if (M[i, j] == "P")
                                player = true;
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



        // end of class
    }
}
