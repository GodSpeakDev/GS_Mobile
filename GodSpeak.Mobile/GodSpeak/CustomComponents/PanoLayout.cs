
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GodSpeak
{
    public class PanoLayout : AbsoluteLayout
    {
        public PanoLayout () : base ()
        {
            this.IsClippedToBounds = true;
        }

        public Animation GoForwardAnimation (float xDelta)
        {
            var animation = new Animation ();

            foreach (var child in this.Children) {
                var currentTranslationX = child.TranslationX;
                var childAnimation = new Animation ((obj) => {
                    child.TranslationX = currentTranslationX - xDelta * obj;
                });
                animation.Add (0, 1, childAnimation);
            }

            return animation;
        }
    }
}
