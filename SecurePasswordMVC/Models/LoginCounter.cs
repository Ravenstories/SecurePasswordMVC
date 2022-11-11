namespace SecurePasswordMVC.Models
{
    public class LoginCounter
    {
        /// <summary>
        /// Simple counter and celling so I only had to change it one place. 
        /// </summary>
        private static int counter;
        public static int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
        private static int maxTries = 5;

        public static int MaxTries 
        { 
            get { return maxTries; } 
            set { maxTries = value; } 
        }

    }
}
