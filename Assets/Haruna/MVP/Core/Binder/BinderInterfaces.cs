
namespace Haruna.UnityMVP.Model
{
	public interface IMvpArrayBinder
	{
		void SetData(MArray data);
		MArray GetData();
		bool HasEditorError();
	}

	public interface IMvpObjectBinder
	{
		void SetData(MObject data);
		MObject GetData();
		bool HasEditorError();
	}

	public interface IMvpStringBinder
	{
		void SetData(MString data);
		MString GetData();
		bool HasEditorError();
	}
	public interface IMvpFloatBinder
	{
		void SetData(MFloat data);
		MFloat GetData();
		bool HasEditorError();
	}
	public interface IMvpBoolBinder
	{
		void SetData(MBool data);
		MBool GetData();
		bool HasEditorError();
	}

	public interface IMvpCustomTypeBinder<T>
	{
		void SetData(MValue<T> data);
		MValue<T> GetData();
		bool HasEditorError();
	}

	public interface IMvpTokenBinder
	{
		void SetData(MToken data);
		MToken GetData();
		bool HasEditorError();
	}
}