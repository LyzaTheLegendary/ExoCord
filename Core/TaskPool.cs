using System.Collections.Concurrent;

namespace Core {
    public class TaskPool {
        CancellationTokenSource cts;
        BlockingCollection<Action> taskBuff;

        public TaskPool(int threads) {
            taskBuff = new();
            cts = new();
            for(int i = 0; i < threads; i++)
                Task.Factory.StartNew(MainLoop, TaskCreationOptions.LongRunning);
            
        }
        public void Stop() => cts.Cancel();
        public bool IsAlive() => !cts.IsCancellationRequested;
        public int TasksInQueue() => taskBuff.Count;

        public void PendAction(Action action)
            => taskBuff.Add(action);
        
        private void MainLoop() {
            foreach (Action action in taskBuff.GetConsumingEnumerable()) {
                if (cts.IsCancellationRequested) break;

                try {
                    action.Invoke();
                }catch(Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
