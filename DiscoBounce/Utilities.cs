using System;

namespace DiscoBounce
{
    class Utilities
    {
        public Random Random;

        private static Utilities instance;
        public static Utilities Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utilities();
                }
                return instance;
            }
        }

        public Utilities()
        {
            Random = new Random();
        }
    }
}
