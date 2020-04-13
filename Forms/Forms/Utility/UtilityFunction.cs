using Acr.UserDialogs;

namespace Forms.Utility
{
    public static class UtilityFunction
    {
        public static void ToastMessage(string Message)
        {
            ToastConfig toast = new ToastConfig(Message);
            toast.SetDuration(3000);
            toast.SetBackgroundColor(System.Drawing.Color.DimGray);
            UserDialogs.Instance.Toast(toast);
        }
    }
}
