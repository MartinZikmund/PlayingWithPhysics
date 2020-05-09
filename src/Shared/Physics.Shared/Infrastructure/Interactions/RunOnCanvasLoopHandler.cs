using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.UI.Xaml;
using MvvmCross.Base;

namespace Physics.Shared.UI.Infrastructure.Interactions
{
    public class RunOnCanvasLoopHandler : IInteractionRequestHandler<RunOnCanvasLoopRequest>
    {
        private readonly CanvasAnimatedControl _control;

        public RunOnCanvasLoopHandler(CanvasAnimatedControl control) => _control = control;

        public async Task HandleAsync(RunOnCanvasLoopRequest args)
        {
            await _control.RunOnGameLoopThreadAsync(() =>
            {
                try
                {
                    args.Action();
                    args.SetResult(true);
                }
                catch(Exception ex)
                {
                    args.SetException(ex);
                }
            });
        }

        public void OnRequested(object sender, MvxValueEventArgs<RunOnCanvasLoopRequest> eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
