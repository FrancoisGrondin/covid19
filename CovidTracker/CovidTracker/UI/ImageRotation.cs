using System;
using Xamarin.Forms;

namespace CovidTracker
{
    public class ImageRotation : TriggerAction<Image>
    {
        public AnimationAction Action { get; set; }
        public enum AnimationAction { Start, Stop }

        protected override void Invoke(Image sender)
        {
            if (sender != null) {
                if (Action == AnimationAction.Start) {
                    PerformAnimation(sender);
                }
                else if (Action == AnimationAction.Stop) {
                    CancelAnimation(sender);
                }
            }
        }

        private void PerformAnimation(Image image)
        {
            image.RotateTo(4800, 20000, Easing.Linear);
        }

        private void CancelAnimation(Image myElement)
        {
            ViewExtensions.CancelAnimations(myElement);
        }
    }
}
