using System;

namespace PlayerController
{
    public class XPoints
    {
        private int xp;
        private int nextLevelXp = 5;

        public event Action<int> OnLevelUp;

        public int Xp
        {
            get => xp;
            set
            {
                xp = value;
                if (xp >= nextLevelXp)
                {
                    nextLevelXp += Level * 5;
                    Level++;
                    OnLevelUp?.Invoke(Level);
                }
            }
        }
        public int Level { get; set; }
    }
}
