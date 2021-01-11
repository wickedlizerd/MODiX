using System;

using Remora.Results;

namespace Modix.Bot.Controls
{
    public static class ControlCreationResult
    {
        public static ControlCreationResult<TControl> FromControl<TControl>(TControl control)
                where TControl : ControlBase
            => ControlCreationResult<TControl>.FromControl(control);
    }

    public class ControlCreationResult<TControl>
            : ResultBase<ControlCreationResult<TControl>>
        where TControl : ControlBase
    {
        public static ControlCreationResult<TControl> FromControl(TControl control)
            => new ControlCreationResult<TControl>(control);

        protected ControlCreationResult(TControl control)
        {
            _control = control;
        }

        protected ControlCreationResult(string? errorReason, Exception? exception = null)
            : base(errorReason, exception)
        {
            _control = default!;
        }

        public TControl Control
            => IsSuccess ? _control : throw new InvalidOperationException("Cannot retrieve a control from a failed creation result");

        private readonly TControl _control;
    }
}
