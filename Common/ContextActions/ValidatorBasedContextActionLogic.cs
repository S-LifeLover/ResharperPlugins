using Common.Validators;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.Util;

namespace Common.ContextActions
{
    internal sealed class ValidatorBasedContextActionLogic : IContextAction
    {
        // ToDo_AP: добавить проверки на нулл (нужно узнать, как подключать решарперы)
        public ValidatorBasedContextActionLogic(
            IValidator validator, IBulbItem bulbItem)
        {
            _validator = validator;
            _bulbItems = new[] { bulbItem };
        }

        // ToDo_AP: добавить проверки на нулл (нужно узнать, как подключать решарперы)
        public bool IsAvailable(IUserDataHolder cache)
        {
            return _validator.Validate();
        }

        public IBulbItem[] Items
        {
            get { return _bulbItems; }
        }

        private readonly IValidator _validator;
        private readonly IBulbItem[] _bulbItems;
    }
}