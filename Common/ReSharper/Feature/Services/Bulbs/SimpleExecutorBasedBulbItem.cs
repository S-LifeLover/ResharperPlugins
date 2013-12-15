using System;
using Common.Executors;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.TextControl;

namespace Common.ReSharper.Feature.Services.Bulbs
{
    internal sealed class SimpleExecutorBasedBulbItem : BulbItemImpl
    {
        public SimpleExecutorBasedBulbItem(string text, IExecutor executor)
        {
            _text = text;
            _executor = executor;
        }

        // ToDo_AP: непонятно почему решарпер ругается на эту строчку
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            _executor.Execute();
            return null;
        }

        public override string Text
        {
            get { return _text; }
        }

        private readonly string _text;
        private readonly IExecutor _executor;
    }
}