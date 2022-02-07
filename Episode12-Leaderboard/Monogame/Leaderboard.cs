using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shmup
{
    internal class Leaderboard
    {
        private class ScoreData
        {
            public int Score;
            public string Name;
            public ScoreData(string line)
            {
                DecodeLine(line);
            }
            public ScoreData(string name, int score)
            {
                Name = name.Trim();
                Score = score;
            }
            private void DecodeLine(string line)
            {
                // eg line = "FRED;258"
                string[] parts = line.Split(';');
                Name = parts[0].Trim();                    // "FRED"
                Score = Convert.ToInt32(parts[1].Trim());  // 258
            }
            public string GetScoreData(string dest)
            {
                if (dest == "file")
                    return $"{Name};{Score}\n"; // "FRED;258\n"
                else
                    return $"{Name} : {Score}";
            }
        }
        public bool CurrentPlayerEntered = false;
        private List<ScoreData> scoreList = new List<ScoreData>();
        public string Name = "";
        private float PosY = 80f;
        private readonly int maxLines = 5;
        private RectangleF rect = new RectangleF(50, Shared.HEIGHT * 0.85f, Shared.WIDTH - 100f, 24);
        public Leaderboard()
        {
            PopulateScoreList();
        }
        public void AddEntry()
        {
            if (Name != "")
            {
                CurrentPlayerEntered = true;
                ScoreData scoreTemp = new ScoreData(Name, Shared.Score);
                InsertData(scoreTemp);
                Shared.InputText = "";
                WriteScoreList();
                PopulateScoreList();
            }
        }
        private void AddScoreData(string line)
        {
            // create new scoreData object from line in text file
            ScoreData scoreTemp = new ScoreData(line);
            // insert new object into correct position
            InsertData(scoreTemp);
        }
        private void InsertData(ScoreData scoredata)
        {
            int insertAt = scoreList.Count; // assume end of list
            for (int i = 0; i < scoreList.Count; i++)
            {
                if (scoredata.Score >= scoreList[i].Score) // current score > listed
                {
                    insertAt = i;
                    break;
                }
            }
            // if above loop did not find a greater score then insert at end of list
            scoreList.Insert(insertAt, scoredata);
        }
        private void PopulateScoreList()
        {
            /// Read file called HighScore.txt
            scoreList.Clear();
            if (File.Exists("Highscore.txt"))
            {
                string[] lines = File.ReadAllLines("Highscore.txt");
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                        AddScoreData(line);
                }
            }
        }
        private void WriteScoreList()
        {
            using (StreamWriter outputFile = new StreamWriter("Highscore.txt"))
            {
                for(int i = 0;i < scoreList.Count; i++)
                {
                    if (i > maxLines - 1) // max 5 lines to be written
                        break;
                    outputFile.WriteLine(scoreList[i].GetScoreData("file"));
                }
            }
        }
        public void Update(KeyboardState keyboardState)
        {
            if (CurrentPlayerEntered)
            {
                if (keyboardState.IsKeyDown(Keys.C))
                {
                    Shared.GameState = Shared.GameStates["menu"];
                    CurrentPlayerEntered = false;
                    Shared.InputText = "";
                }
            }
            else
            {
                Name = Shared.InputText;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Shared.DrawString(spriteBatch, "LEADERBOARD", "size50", Shared.WIDTH * 0.5f, 0 ,Color.Yellow,"centre");    // 'Leaderboard'
            PosY = 80;
            //foreach(ScoreData score in scoreList)
            for(int i = 0; i < scoreList.Count; i++)
            {
                if (i > maxLines - 1)
                    break;
                Shared.DrawString(spriteBatch, scoreList[i].GetScoreData("display"), "size24", Shared.WIDTH * 0.5f, PosY, Color.White, "centre");
                PosY += 30;
            }
            if(CurrentPlayerEntered)    // If got input from user
            {
                Shared.DrawString(spriteBatch, "Press C to continue", "size18", 0, Shared.HEIGHT * 0.8f, Color.White, "centre");
            }
            else
            {
                Shared.DrawString(spriteBatch, "Type your name and press Enter", "size18", 0, Shared.HEIGHT * 0.8f, Color.White, "centre");
                // displayBox(SpriteBatch spriteBatch, string text, string size, RectangleF rect, Color lineColour, Color backColour, Color textColour)
                Shared.DisplayBox(spriteBatch, "NAME: " + Name, "size18", rect, Color.White, Shared.DARKBLUE, Color.White);
            }
        }
    }
}
