using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packets {
    /* GOAL:
     * When a packet with a expected result runs out of response time then it should automatically return with a TIME_OUT.
     * This is used to verify the result of an action in a async manner.
     */
    public enum ResultCode: ushort {
        SUCCESS,
        FAILED,
        NOT_ALLOWED,
        NOT_AVAILABLE,
        TIME_OUT,
    }
}
