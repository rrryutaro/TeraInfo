using Terraria.UI;

namespace TeraInfo
{
    class UIModState : UIState
    {
        internal UserInterface userInterface;

        public UIModState(UserInterface userInterface)
        {
            this.userInterface = userInterface;
        }

        public void ReverseChildren()
        {
            Elements.Reverse();
        }
    }
}
