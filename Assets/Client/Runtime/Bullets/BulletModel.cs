using Client.Runtime.Utils;

namespace Client.Runtime.Bullets
{
    public class BulletModel : BaseModel
    {
        private BulletView _view;

        public BulletModel(BulletView view) : base(view)
        {
            _view = view;
        }

        public BaseModel Shooter { get; set; }
    }
}