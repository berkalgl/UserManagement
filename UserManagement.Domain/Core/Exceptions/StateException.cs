using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Core.Exceptions
{
    public sealed class StateException : Exception
    {
        public StateTransitionError ErrorCode { get; set; }
        public StateException(string message)
            : base(message)
        {
        }

        public StateException(string message, StateTransitionError errorcode) : this(message)
        {
            ErrorCode = errorcode;
        }
        public enum StateTransitionError
        {
            TransitionPermission = 1,
            Trigger = 2,
            MultipleTransitions = 3,
            UnHandledTriggerAction = 4,
            StateNotRegistered = 5,
            RolePermission = 6
        }
    }
}
