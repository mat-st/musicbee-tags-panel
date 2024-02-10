using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    internal class Style
    {
        private readonly MusicBeeApiInterface mbApiInterface;

        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class.
        /// </summary>
        /// <param name="mbApiInterface">The MusicBee API interface.</param>
        public Style(MusicBeeApiInterface mbApiInterface)
        {
            this.mbApiInterface = mbApiInterface;
        }

        /// <summary>
        /// Gets the color of the specified skin element.
        /// </summary>
        /// <param name="skinElement">The skin element.</param>
        /// <param name="elementState">The element state.</param>
        /// <param name="elementComponent">The element component.</param>
        /// <returns>The color of the specified skin element.</returns>
        public Color GetElementColor(SkinElement skinElement, ElementState elementState, ElementComponent elementComponent)
        {
            //get current skin colors
            int colorValue = this.mbApiInterface.Setting_GetSkinElementColour(skinElement, elementState, elementComponent);
            return Color.FromArgb(colorValue);
        }

        /// <summary>
        /// Styles the specified form control.
        /// </summary>
        /// <param name="formControl">The form control.</param>
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
