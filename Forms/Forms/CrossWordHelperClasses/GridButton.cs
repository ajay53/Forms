using Xamarin.Forms;

namespace CrossWordHelper
{
    public class GridButton : Key
    {
        public int row = 0;
        public int column = 0;

        public GridButton()
        {
            UpColor = Color.Transparent;
            DownColor = new Color(1.0);
            BackgroundColor = UpColor;

        }
        
    }
}
