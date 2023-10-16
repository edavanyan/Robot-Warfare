namespace GameInput
{
    public partial class @InputActions
    {
        private static InputActions instance;

        public static InputActions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputActions();
                    instance.Enable();
                }

                return instance;
            }
            private set => instance = value;
        }
    }
}
