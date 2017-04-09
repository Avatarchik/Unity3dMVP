
namespace Haruna.UnityMVP.Model
{
	public interface IMvpArrayBinder
	{
		void SetData(MArray data);
		MArray GetData();
	}

	public interface IMvpObjectBinder
	{
		void SetData(MObject data);
		MObject GetData();
	}

	public interface IMvpStringBinder
	{
		void SetData(MString data);
		MString GetData();
	}
	public interface IMvpFloatBinder
	{
		void SetData(MFloat data);
		MFloat GetData();
	}
	public interface IMvpBoolBinder
	{
		void SetData(MBool data);
		MBool GetData();
	}

	public interface IMvpCustomTypeBinder<T>
	{
		void SetData(MValue<T> data);
		MValue<T> GetData();
	}

	public interface IMvpTokenBinder
	{
		void SetData(MToken data);
		MToken GetData();
	}
}