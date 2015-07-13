using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedSquawk.Client
{
	public sealed class Timer : CancellationTokenSource {
		public bool IsDisposed { get; private set; }

		public Timer(Action<object> callback, object state, int dueTime, int period) {
			Task.Delay(dueTime, Token).ContinueWith(async (t, s) => {
				Tuple<Action<object>, object> tuple = (Tuple<Action<object>, object>)s;

				while (!IsCancellationRequested) {
					Task.Run(() => tuple.Item1(tuple.Item2));
					await Task.Delay(period);
				}

			}, Tuple.Create(callback, state), CancellationToken.None,
				TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
				TaskScheduler.Default);
		}

		protected override void Dispose(bool disposing) {
			IsDisposed = true;
			if (disposing) {
				Cancel();
			}

			base.Dispose(disposing);
		}
	}
}

