using System;

namespace Player
{
    public class XPoints
    {
        private int xp;
        public int NextLevelXp { get; set; }

        public event Action<int> OnLevelUp;

        public XPoints()
        {
            NextLevelXp = 5;
        }

        public int Xp
        {
            get => xp;
            set
            {
                xp = value;
                if (xp >= NextLevelXp)
                {
                    NextLevelXp += Level * 5;
                    Level++;
                    OnLevelUp?.Invoke(Level);
                }
            }
        }
        public int Level { get; set; }
    }
}
