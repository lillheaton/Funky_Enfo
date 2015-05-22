using Microsoft.Xna.Framework.Content;

namespace Funky.Enfo
{
    public class AssetsManager
    {
        private ContentManager contentManager;

        public AssetsManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            this.Init();
        }

        private void Init()
        {
            // Load fonts, images, sound etc
        }
    }
}