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
            var defaultFont = this.mbApiInterface.Setting_GetDefaultFont();
            var trackAndArtistPanelColor = this.GetElementColor(SkinElement.SkinTrackAndArtistPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground);
            var inputControlColor = this.GetElementColor(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground);

            formControl.Font = defaultFont;
            formControl.BackColor = trackAndArtistPanelColor;
            formControl.ForeColor = inputControlColor;
        }

    }
}
