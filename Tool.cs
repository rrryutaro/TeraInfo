﻿using System;
using Terraria;
using Terraria.UI;

namespace TeraInfo
{
    // UIState needs UserInterface for Scrollbar fixes
    // Tool should store data? does it even matter?
    abstract class Tool
    {
        internal UserInterface userInterface;
        internal UIModState uistate;
        //	Type uistateType;

        public Tool(Type uistateType)
        {
            userInterface = new UserInterface();
            uistate = (UIModState)Activator.CreateInstance(uistateType, new object[] { userInterface });
            uistate.Activate();
            userInterface.SetState(uistate);
        }

        /// <summary>
        /// Initializes this Tool. Called during Load.
        /// Useful for initializing data.
        /// </summary>
        internal virtual void Initialize()
        {
        }

        /// <summary>
        /// Initializes this Tool. Called during Load after Initialize only on SP and Clients.
        /// Useful for initializing UI.
        /// </summary>
        internal virtual void ClientInitialize() { }

        internal virtual void ScreenResolutionChanged()
        {
            userInterface?.Recalculate();
        }

        internal virtual void UIUpdate()
        {
            //if (visible)
            {
                userInterface?.Update(Main._drawInterfaceGameTime);
            }
        }

        internal virtual void UIDraw()
        {
            //if (visible)
            {
                uistate.ReverseChildren();
                uistate.Draw(Main.spriteBatch);
                uistate.ReverseChildren();
            }
        }

        internal virtual void DrawUpdateToggle() { }

        internal virtual void Toggled() { }

        internal virtual void PostSetupContent()
        {
            if (!Main.dedServ)
            {

            }
        }
    }
}
