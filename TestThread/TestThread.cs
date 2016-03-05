using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace TestThread
{
	public class App : Application
	{
		public static CancellationTokenSource CancellationToken { get; set; }
		private Label _label = new Label();

		public App ()
		{

			ContentPage cp = new ContentPage ();
			StackLayout sl = new StackLayout ();
			_label.FontSize = 30;
			sl.Children.Add (_label);
			cp.Content = sl;
			MainPage = cp;
			return;

		}

		protected override void OnStart ()
		{
			// Handle when your app starts
			Task.Run (async() => timer ());
		}

		private int _counter = 0;
		private async  Task timer(){

			App.CancellationToken = new CancellationTokenSource ();
			while (!App.CancellationToken.IsCancellationRequested)
			{
				try
				{
					//await Task.Delay(60000 - (int)(watch.ElapsedMilliseconds%1000), token);
					App.CancellationToken.Token.ThrowIfCancellationRequested();
					await Task.Delay(1000 , App.CancellationToken.Token).ContinueWith(async (arg) =>{

						if (!App.CancellationToken.Token.IsCancellationRequested) {
							App.CancellationToken.Token.ThrowIfCancellationRequested();
							/*
							 * HERE YOU CAN DO YOUR ACTION
							 */

							Device.BeginInvokeOnMainThread(()=> _label.Text = (++_counter).ToString());
							Debug.WriteLine ("TimerRunning " + _counter.ToString() ); 
						}
					});
					//Debug.WriteLine (DateTime.Now.ToLocalTime ().ToString () + " DELAY: " + delay);

				}
				catch (Exception ex)
				{
					Debug.WriteLine ("EX 1: " + ex.Message);
				}

//				try{
//
//					//Device.BeginInvokeOnMainThread(() => this.lblTimerText.Text = (DateTime.Now - startTime).ToString());
//					//Device.BeginInvokeOnMainThread(() => this._labelOra.Text = watch.Elapsed.ToString());
//					if (!App.CancellationToken.Token.IsCancellationRequested) {
//						App.CancellationToken.Token.ThrowIfCancellationRequested();
//						Device.BeginInvokeOnMainThread(()=> _label.Text = (++_counter).ToString());
//						Debug.WriteLine ("TimerRunning " + _counter.ToString());// + watch.Elapsed.ToString ());
//					}
//				}
//				catch (Exception ex){
//					Debug.WriteLine("EX 2 " + ex.Message);
//				}
			}
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
			if (App.CancellationToken != null) {
				App.CancellationToken.Cancel ();
				//App.CancellationToken = null;
			}
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			Task.Run (async() => timer ());
		}
	}
}

