using Casting;
using Packets;
using System.Collections.Concurrent;

namespace Network {
    struct ResultEntry {
        public int timeoutInSeconds;
        public Action<ResultCode> action;
    }
    public class ResultManager {
        ConcurrentDictionary<uint, ResultEntry> m_resultCache = new();

        public void AddResultTracker(uint id,  int timeoutInSeconds, Action<ResultCode> action) {
            if(m_resultCache.Count > 100) {
                CleanUpCache();
            }

            m_resultCache[id] = new ResultEntry {
                timeoutInSeconds = timeoutInSeconds,
                action = action,
            };
        }

        public void HandleResult(uint id, Msg resultData) {
            if (!m_resultCache.TryRemove(id, out ResultEntry result))
                return;

            result.action.Invoke((ResultCode)resultData.GetStruct<ushort>());
        }

        private void CleanUpCache() {
            int time = DateTime.Now.Millisecond;
            foreach (var kvp in m_resultCache) {
                if ((time / 60) - kvp.Value.timeoutInSeconds > 0) {
                    m_resultCache.Remove(kvp.Key, out _);
                }
            }
            
        }
    }
}
