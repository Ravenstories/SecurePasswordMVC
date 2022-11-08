namespace SecurePasswordMVC.Models
{
    public class LoginCounter
    {
        private static int counter;

        public static int Counter
        {
            get { return counter; }
            set { counter = value; }
        }

        public bool TriesExeeded { get; set; }

    }
}
