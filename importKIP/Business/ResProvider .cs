using ImportKIP.Resources;

namespace ImportKIP
{
    public class ResProvider
    {
        static ProjectResources resources = new ProjectResources();

        public ProjectResources ProjectResources
        {
            get
            {
                return resources;
            }
        }
    }
}
