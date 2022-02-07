using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;
using System.IO;

namespace Shmup
{
    internal class Leaderboard
    {
        #region Private internal class
        private class ScoreData
        {
            /// <summary>
            /// This is a nested class for the sole use
            /// of the Leaderboard class.
            /// It provides an object with Name and Score properties
            /// It has 2 constructors, one takes a string, the other 
            /// a string and int.
            /// There is one public method: GetScoreData
            /// </summary>
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
                Name = parts[0].Trim();                     // "FRED"
                //Score = 0 if TryParse fails
                int.TryParse(parts[1].Trim(), out Score);   // 258
            }
            public string GetScoreData(string dest)
            {
                if (dest == "file")
                    return $"{Name};{Score}\n"; // "FRED;258\n"
                else
                    return $"{Name} : {Score}"; // "FRED : 258"
            }
        }
        #endregion
        #region Class variables
        private bool currentPlayerEntered = false;                  // Has player entered a name?
        private List<ScoreData> scoreList = new List<ScoreData>();  // list of ScoreData objects
        private string name = "";                                   // player name
        private float posY = 80f;                                   // y coordinate of leaderboard display
        private readonly int maxLines = 5;                          // max no of lines to write/display
        private RectangleF rectangle = new RectangleF(50, Shared.HEIGHT * 0.85f, Shared.WIDTH - 100f, 24);
        #endregion
        #region Constructor
        public Leaderboard()
        {
            PopulateScoreList();
        }
        #endregion
        #region Private class methods
        private void AddScoreData(string line)
        {
            // create new scoreData object from line in text file
            ScoreData scoreTemp = new ScoreData(line);
            // insert new object into correct position
            InsertData(scoreTemp);
        }
        private void InsertData(ScoreData scoredata)
        {
            int insertAt = scoreList.Count;                 // assume end of list
            for (int i = 0; i < scoreList.Count; i++)       // iterate list
            {
                if (scoredata.Score >= scoreList[i].Score)  // current score > listed
                {
                    insertAt = i;                           // re-define insertion point
                    break;                                  // break out of loop
                }
            }
            // if above loop did not find a greater score then insert at end of list
            scoreList.Insert(insertAt, scoredata);          // add ScoreData object to list
        }
        private void PopulateScoreList()
        {
            /// Read file called HighScore.txt
            scoreList.Clear();                              // remove all items in score_list
            if (File.Exists("Highscore.txt"))
            {
                // fill list with scoreData objects in high -> low order
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
            /// Write top 6 scores into text file, over-write original
            using (StreamWriter outputFile = new StreamWriter("Highscore.txt"))
            {
                for (int i = 0; i < scoreList.Count; i++)
                {
                    if (i > maxLines - 1) // max 5 lines to be written
                        break;
                    outputFile.WriteLine(scoreList[i].GetScoreData("file"));
                }
            }
        }
        #endregion
        #region Public Methods
        public void AddEntry()
        {
            /// Called from ShmupMain TextInputHandler
            /// leaderboard.AddEntry()
            if (name != "")
            {
                currentPlayerEntered = true;
                ScoreData scoreTemp = new ScoreData(name, Shared.Score);
                InsertData(scoreTemp);
                Shared.InputText = "";
                WriteScoreList();
                PopulateScoreList();
            }
        }
        public void Update(KeyboardState keyboardState)
        {
            if (currentPlayerEntered)                   // user has completed entering their name
            {
                if (keyboardState.IsKeyDown(Keys.C))    // c key pressed
                {
                    Shared.GameState = Shared.GameStates["menu"];
                    currentPlayerEntered = false;
                    Shared.InputText = "";
                }
            }
            else
            {
                name = Shared.InputText;                // This value is controlled by TextInputHandler in ShmupMain
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Shared.DrawString(  spriteBatch:spriteBatch, text:"LEADERBOARD",
                                size:"size50", posX:Shared.WIDTH * 0.5f, posY:0,
                                color:Color.Yellow, align:"centre");    // 'Leaderboard'
            posY = 80;
            for(int i = 0; i < scoreList.Count; i++)
            {
                if (i > maxLines - 1)
                    break;
                Shared.DrawString(  spriteBatch:spriteBatch, text:scoreList[i].GetScoreData("display"),
                                    size:"size24", posX:Shared.WIDTH * 0.5f, posY:posY,
                                    color:Color.White, align:"centre");
                posY += 30;
            }
            if(currentPlayerEntered)    // If got input from user
            {
                Shared.DrawString(  spriteBatch:spriteBatch, text:"Press C to continue",
                                    size:"size18", posX:0, posY:Shared.HEIGHT * 0.8f,
                                    color:Color.White, align:"centre");
            }
            else
            {
                Shared.DrawString(  spriteBatch:spriteBatch, text:"Type your name and press Enter",
                                    size:"size18", posX:0, posY:Shared.HEIGHT * 0.8f,
                                    color:Color.White, align:"centre");

                Shared.DisplayBox(  spriteBatch:spriteBatch, text:"NAME: " + name,
                                    size:"size18", rect:rectangle,
                                    lineColour:Color.White,
                                    backColour:Shared.DARKBLUE,
                                    textColour:Color.White);
            }
        }
        #endregion
    }
}
