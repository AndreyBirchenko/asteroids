using Client.Runtime.Utils;

namespace Client.Runtime.UFO
{
    public class UfoModel : BaseModel
    {
        public UfoModel(BaseView view) : base(view)
        {
        }

        public UfoAIBehaviour AIBehaviour { get; set; }
    }
}