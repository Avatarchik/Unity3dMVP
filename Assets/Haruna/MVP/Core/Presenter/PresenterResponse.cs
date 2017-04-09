using Haruna.UnityMVP.Model;

namespace Haruna.UnityMVP.Presenter
{
	public interface IPresenterResponse
	{
		int StatusCode { get; }
		string ErrorMessage { get; }

		MToken Data { set; get; }
	}

	public class PresenterResponse : IPresenterResponse
	{
		public int StatusCode { set; get; }
		public string ErrorMessage { set; get; }
		public MToken Data { set; get; }

		public PresenterResponse()
		{
			StatusCode = 200;
		}
	}
}