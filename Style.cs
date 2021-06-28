using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    class Style
    {
        private readonly MusicBeeApiInterface mbApiInterface;

        public Style(MusicBeeApiInterface mbApiInterface)
        {
            this.mbApiInterface = mbApiInterface;
        }

        public Color GetElementColor(SkinElement skinElement, ElementState elementState, ElementComponent elementComponent)
        {
            //get current skin colors
            int colorValue = this.mbApiInterface.Setting_GetSkinElementColour(skinElement, elementState, elementComponent);
            return Color.FromArgb(colorValue);
        }

        public void StyleControl(Control formControl)
        {
            // apply current skin colors to tag panel
            formControl.Font = this.mbApiInterface.Setting_GetDefaultFont();
            formControl.BackColor = this.GetElementColor(Plugin.SkinElement.SkinTrackAndArtistPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground);
            formControl.ForeColor = this.GetElementColor(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground);

        }
        
    }
}
